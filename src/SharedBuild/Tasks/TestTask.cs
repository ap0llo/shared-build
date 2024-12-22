using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cake.Common.Build;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using Grynwald.SharedBuild.Tools;
using Grynwald.SharedBuild.Tools.TemporaryFiles;
using Octokit;

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
            await GenerateCodeCoverageOutputAsync(context);
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

    protected virtual async Task PublishTestResultsAsync(IBuildContext context, bool failOnMissingTestResults)
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
                context.Log.Debug($"Publishing Test Result file '{testResult}' as pipeline artifact");
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

            var testRunNames = GetTestRunNames(context, testResults);

            // GitHub Actions only allows a single upload for each artifact name
            // => Copy all results to a temporary directory and publish the directory
            using var temporaryDirectory = context.CreateTemporaryDirectory();

            foreach (var testResult in testResults)
            {
                var fileName = testRunNames[testResult] + testResult.GetExtension();
                context.CopyFile(testResult, temporaryDirectory.Path.CombineWithFilePath(fileName));
            }
            await context.GitHubActions().Commands.UploadArtifact(
                temporaryDirectory.Path,
                context.GitHubActions.ArtifactNames.TestResults
            );

            //TODO: Generate a human-readable test result and publish into GitHub action's step summary
        }
    }

    protected virtual IReadOnlyDictionary<FilePath, string> GetTestRunNames(IBuildContext context, IEnumerable<FilePath> testResultPaths)
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

    /// <summary>
    /// Merges the individual code coverage reports from all test projects into a single coverage result file and generates a HTML report.
    /// If the build is running in a CI system, the coverage is also published as pipeline artifact
    /// </summary>
    private async Task GenerateCodeCoverageOutputAsync(IBuildContext context)
    {
        var mergedCoverageResult = MergeCoverageFiles(context);

        var htmlReportPath = GenerateCodeCoverageHtmlReport(context, mergedCoverageResult);

        //
        // Publish Code coverage report to CI artifacts if necessary
        //
        if (context.AzurePipelines.IsActive)
        {
            PublishCodeCoverageToAzurePipelines(context, mergedCoverageResult, htmlReportPath);
        }
        else if (context.GitHubActions.IsActive)
        {
            await PublishCodeCoverageToGitHubActionsAsync(context, mergedCoverageResult, htmlReportPath);
        }
    }

    /// <summary>
    /// Merges all code coverage outputs into a single Cobertura report file
    /// </summary>
    protected virtual FilePath MergeCoverageFiles(IBuildContext context)
    {
        var coverageFiles = context.FileSystem.GetFilePaths(context.Output.TestResultsDirectory, "coverage.cobertura.xml", SearchScope.Recursive);

        if (!coverageFiles.Any())
            throw new Exception($"No coverage files found in '{context.Output.TestResultsDirectory}'");

        context.Log.Information($"Found {coverageFiles.Count} coverage files");

        var mergedCoverageFilePath = context.Output.CodeCoverageOutputDirectory.CombineWithFilePath("Cobertura.xml");
        context.EnsureFileDoesNotExist(mergedCoverageFilePath);

        //
        // Generate merged code coverage file
        //
        context.Log.Information("Merging coverage files");
        context.ReportGenerator(
            reports: coverageFiles,
            targetDir: context.Output.CodeCoverageOutputDirectory,
            settings: new ReportGeneratorSettings()
            {
                ReportTypes = [ReportGeneratorReportType.Cobertura],
                HistoryDirectory = GetCodeCoverageHistoryDirectory(context)
            }
        );

        if (!context.FileExists(mergedCoverageFilePath))
        {
            throw new CakeException($"Failed to merge code coverage output files. Expected output file '{mergedCoverageFilePath}' does not exist after merging files");
        }

        return mergedCoverageFilePath;
    }

    /// <summary>
    /// Generates a Code Coverage HTML report
    /// </summary>
    protected virtual DirectoryPath GenerateCodeCoverageHtmlReport(IBuildContext context, FilePath coverageFilePath)
    {
        var reportDirectory = context.Output.CodeCoverageOutputDirectory.Combine("Report");
        context.Log.Information($"Generating code coverage HTML report to '{reportDirectory}'");
        context.EnsureDirectoryDoesNotExist(reportDirectory);

        context.ReportGenerator(
            reports: [coverageFilePath],
            targetDir: reportDirectory,
            settings: new ReportGeneratorSettings()
            {
                ReportTypes = [ReportGeneratorReportType.Html],
                HistoryDirectory = GetCodeCoverageHistoryDirectory(context)
            }
        );

        return reportDirectory;
    }

    private DirectoryPath GetCodeCoverageHistoryDirectory(IBuildContext context) => context.Output.CodeCoverageOutputDirectory.Combine("History");


    protected virtual void PublishCodeCoverageToAzurePipelines(IBuildContext context, FilePath coverageReportPath, DirectoryPath htmlReportPath)
    {
        context.Log.Information("Publishing Code Coverage Results to Azure Pipelines");

        //
        // Generate a version of the HTML report tailored for Azure Pipelines and publish it as code coverage
        // so it is shown in the "Code Coverage" Azure Pipelines Web UI
        //

        using var temporaryDirectory = context.CreateTemporaryDirectory();

        context.Log.Verbose("Generating tailored HTML code coverage report for Azure Pipelines");
        context.ReportGenerator(
            reports: [coverageReportPath],
            targetDir: temporaryDirectory.Path,
            settings: new ReportGeneratorSettings()
            {
                ReportTypes = [ReportGeneratorReportType.HtmlInline_AzurePipelines],
                HistoryDirectory = GetCodeCoverageHistoryDirectory(context)
            }
        );

        context.Log.Verbose("Publishing code coverage to Azure Pipelines Web UI");
        context.AzurePipelines.Commands.PublishCodeCoverage(new()
        {
            CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
            SummaryFileLocation = coverageReportPath,
            ReportDirectory = temporaryDirectory.Path
        });

        using var stagingDirectory = context.CreateTemporaryDirectory();

        context.CopyFileToDirectory(coverageReportPath, stagingDirectory.Path);
        context.CopyDirectory(htmlReportPath, stagingDirectory.Path.Combine(htmlReportPath.GetDirectoryName()));

        //
        // Publish HTML report and coverage file as pipeline artifact to make it downloadable
        //
        context.Log.Verbose($"Publishing code coverage as pipeline artifact");
        context.AzurePipelines.Commands.UploadArtifact("", stagingDirectory.Path.ToString(), "CodeCoverage");
    }

    protected virtual async Task PublishCodeCoverageToGitHubActionsAsync(IBuildContext context, FilePath coverageReportPath, DirectoryPath htmlReportPath)
    {
        context.Log.Information("Publishing Code Coverage Results to GitHub Actions");

        // GitHUb Actions only allows artifact uploads once for each name
        // => Copy all files together into a temporary directory and publish that directory
        using var temporaryDirectory = context.CreateTemporaryDirectory();

        context.CopyFileToDirectory(coverageReportPath, temporaryDirectory.Path);
        context.CopyDirectory(htmlReportPath, temporaryDirectory.Path.Combine(htmlReportPath.GetDirectoryName()));

        // Publish coverage file and Summary as artifacts
        await context.GitHubActions().Commands.UploadArtifact(temporaryDirectory.Path, "CodeCoverage");
    }
}
