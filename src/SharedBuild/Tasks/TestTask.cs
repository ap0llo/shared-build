using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cake.Common.Build;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using Grynwald.SharedBuild.Tools.TemporaryFiles;
using Microsoft.VisualBasic;

namespace Grynwald.SharedBuild.Tasks;

[TaskName(TaskNames.Test)]
[IsDependentOn(typeof(BuildTask))]
public class TestTask : AsyncFrostingTask<IBuildContext>
{
    public override async Task RunAsync(IBuildContext context)
    {
        context.EnsureDirectoryDoesNotExist(context.Output.TestResultsDirectory);

        await RunTestsAsync(context);

        if (context.TestSettings.CollectCodeCoverage)
        {
            await GenerateCoverageReportAsync(context);
        }
    }

    public override void OnError(Exception exception, IBuildContext context)
    {
        // If test execution failed, publish test results anyways (so the error can be inspected)
        // but do not throw in PublishTestResults() when there are not test results
        PublishTestResultsAsync(context, failOnMissingTestResults: false).GetAwaiter().GetResult();

        throw exception;
    }

    protected virtual DotNetTestSettings GetDotNetTestSettings(IBuildContext context)
    {
        var testSettings = new DotNetTestSettings()
        {
            Configuration = context.BuildSettings.Configuration,
            NoBuild = true,
            NoRestore = true,
            Loggers = ["trx"],
            ResultsDirectory = context.Output.TestResultsDirectory
        };

        if (context.TestSettings.CollectCodeCoverage)
        {
            // Assumes that the "coverlet.collector" package is installed in all test projects
            testSettings.Collectors = ["XPlat Code Coverage"];
        }

        return testSettings;
    }

    private async Task RunTestsAsync(IBuildContext context)
    {
        context.Log.Information($"Running tests for {context.SolutionPath}");

        //
        // Run tests
        //
        var testSettings = GetDotNetTestSettings(context);

        context.DotNetTest(context.SolutionPath.FullPath, testSettings);

        //
        // Publish Test Results
        //
        await PublishTestResultsAsync(context, failOnMissingTestResults: true);
    }

    private static async Task PublishTestResultsAsync(IBuildContext context, bool failOnMissingTestResults)
    {
        var testResults = context.FileSystem.GetFilePaths(context.Output.TestResultsDirectory, "*.trx", SearchScope.Current);

        if (!testResults.Any() && failOnMissingTestResults)
            throw new Exception($"No test results found in '{context.Output.TestResultsDirectory}'");

        if (context.AzurePipelines.IsActive)
        {
            context.Log.Information("Publishing Test Results to Azure Pipelines");

            var testRunNames = GetTestRunNames(context, testResults);

            foreach (var testResult in testResults)
            {
                // Publish test results to Azure Pipelines test UI
                context.Log.Debug($"Publishing Test Results from '{testResult}' with title '{testRunNames[testResult]}'");
                context.AzurePipelines.Commands.PublishTestResults(new()
                {
                    Configuration = context.BuildSettings.Configuration,
                    TestResultsFiles = [testResult],
                    TestRunner = AzurePipelinesTestRunnerType.VSTest,
                    TestRunTitle = testRunNames[testResult]
                });

                // Publish result file as downloadable artifact
                context.Log.Debug($"Publishing Test Result file '{testResult}' as pipelne artifact");
                context.AzurePipelines.Commands.UploadArtifact(
                    folderName: "",
                    file: testResult,
                    context.AzurePipelines.ArtifactNames.TestResults
                );
            }
        }
        else if (context.GitHubActions.IsActive)
        {
            context.Log.Information("Publishing Test Results to GitHub Actions");

            //var testRunNames = GetTestRunNames(context, testResults);

            foreach (var testResult in testResults)
            {
                //TODO
                // // Publish test results to Azure Pipelines test UI
                // context.Log.Debug($"Publishing Test Results from '{testResult}' with title '{testRunNames[testResult]}'");
                // context.AzurePipelines.Commands.PublishTestResults(new()
                // {
                //     Configuration = context.BuildSettings.Configuration,
                //     TestResultsFiles = [testResult],
                //     TestRunner = AzurePipelinesTestRunnerType.VSTest,
                //     TestRunTitle = testRunNames[testResult]
                // });

                // Publish result file as pipeline artifact
                context.Log.Debug($"Publishing Test Result file '{testResult}' as pipeline artifact");
                await context.GitHubActions().Commands.UploadArtifact(
                    testResult,
                    context.GitHubActions.ArtifactNames.TestResults
                );
            }
        }
    }

    private async Task GenerateCoverageReportAsync(IBuildContext context)
    {
        context.EnsureDirectoryDoesNotExist(context.Output.CodeCoverageReportDirectory, new() { Force = true, Recursive = true });

        var coverageFiles = context.FileSystem.GetFilePaths(context.Output.TestResultsDirectory, "coverage.cobertura.xml", SearchScope.Recursive);

        if (!coverageFiles.Any())
            throw new Exception($"No coverage files found in '{context.Output.TestResultsDirectory}'");

        context.Log.Information($"Found {coverageFiles.Count} coverage files");

        //
        // Generate Coverage Report and merged code coverage file
        //
        context.Log.Information("Merging coverage files");
        var htmlReportType = context.AzurePipelines.IsActive
            ? ReportGeneratorReportType.HtmlInline_AzurePipelines
            : ReportGeneratorReportType.Html;

        context.ReportGenerator(
            reports: coverageFiles,
            targetDir: context.Output.CodeCoverageReportDirectory,
            settings: new ReportGeneratorSettings()
            {
                ReportTypes = [htmlReportType, ReportGeneratorReportType.Cobertura],
                HistoryDirectory = context.Output.CodeCoverageHistoryDirectory
            }
        );

        var coverageReportPath = context.Output.CodeCoverageReportDirectory.CombineWithFilePath("Cobertura.xml");

        //
        // Publish Code coverage report
        //
        if (context.AzurePipelines.IsActive)
        {
            PublishCodeCoverageToAzurePipelines(context, coverageReportPath);
        }
        else if (context.GitHubActions.IsActive)
        {
            await PublishCodeCoverageToGitHubActionsAsync(context, coverageReportPath);
        }
    }

    protected virtual void PublishCodeCoverageToAzurePipelines(IBuildContext context, FilePath coverageReportPath)
    {
        context.Log.Information("Publishing Code Coverage Results to Azure Pipelines");
        context.AzurePipelines.Commands.PublishCodeCoverage(new()
        {
            CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
            SummaryFileLocation = coverageReportPath,
            ReportDirectory = context.Output.CodeCoverageReportDirectory
        });
    }

    protected virtual async Task PublishCodeCoverageToGitHubActionsAsync(IBuildContext context, FilePath coverageReportPath)
    {
        context.Log.Information("Publishing Code Coverage Results to GitHub Actions");

        using var temporaryDirectory = context.CreateTemporaryDirectory();

        // Generate Markdown coverage report
        context.ReportGenerator(
            reports: [coverageReportPath],
            targetDir: temporaryDirectory.Path,
            settings: new ReportGeneratorSettings()
            {
                ReportTypes = [ReportGeneratorReportType.MarkdownSummaryGithub],
                HistoryDirectory = context.Output.CodeCoverageHistoryDirectory,
            }
        );
        var markdownSummaryPath = context.FileSystem.GetFilePaths(temporaryDirectory.Path, "*.md").Single();

        // Publish coverage file and Summary are artifacts
        await context.GitHubActions().Commands.UploadArtifact(coverageReportPath, context.GitHubActions.ArtifactNames.TestResults);
        await context.GitHubActions().Commands.UploadArtifact(markdownSummaryPath, context.GitHubActions.ArtifactNames.TestResults);
    }

    private static IReadOnlyDictionary<FilePath, string> GetTestRunNames(IBuildContext context, IEnumerable<FilePath> testResultPaths)
    {
        var testRunNamer = new TestRunNamer(context.Log, context.Environment, context.FileSystem);

        var previousNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var testRunNames = new Dictionary<FilePath, string>();

        foreach (var testResultPath in testResultPaths)
        {
            var baseName = testRunNamer.GetTestRunName(testResultPath);
            var name = baseName;

            // Test run names should be unique, otherwise Azure Pipeline will overwrite results for a previous test with the same name
            // To avoid this, append a number at the end of the name until it is unique.
            var counter = 1;
            while (previousNames.Contains(name))
            {
                name = $"{baseName} ({counter++})";
            }

            previousNames.Add(name);
            testRunNames.Add(testResultPath, name);
        }

        return testRunNames;
    }
}
