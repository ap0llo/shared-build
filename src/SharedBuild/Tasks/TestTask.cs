﻿using System;
using System.Linq;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.Test)]
    [IsDependentOn(typeof(BuildTask))]
    public class TestTask : FrostingTask<IBuildContext>
    {
        public override void Run(IBuildContext context)
        {
            context.EnsureDirectoryDoesNotExist(context.Output.TestResultsDirectory);

            RunTests(context);
        }

        public override void OnError(Exception exception, IBuildContext context)
        {
            // If test execution failed, publish test results anyways (so the error can be inspected)
            // but do not throw in PublishTestResults() when there are not test results
            PublishTestResults(context, failOnMissingTestResults: false);

            base.OnError(exception, context);
        }


        private void RunTests(IBuildContext context)
        {
            context.Log.Information($"Running tests for {context.SolutionPath}");

            //
            // Run tests
            //
            var testSettings = new DotNetCoreTestSettings()
            {
                Configuration = context.BuildSettings.Configuration,
                NoBuild = true,
                NoRestore = true,
                Loggers = new[] { "trx" },
                ResultsDirectory = context.Output.TestResultsDirectory
            };

            context.DotNetCoreTest(context.SolutionPath.FullPath, testSettings);

            //
            // Publish Test Resilts
            //
            PublishTestResults(context, failOnMissingTestResults: true);
        }

        private static void PublishTestResults(IBuildContext context, bool failOnMissingTestResults)
        {
            var testResults = context.FileSystem.GetFilePaths(context.Output.TestResultsDirectory, "*.trx", SearchScope.Current);

            if (!testResults.Any() && failOnMissingTestResults)
                throw new Exception($"No test results found in '{context.Output.TestResultsDirectory}'");

            if (context.AzurePipelines.IsActive)
            {
                context.Log.Information("Publishing Test Results to Azure Pipelines");

                // Publish test results to Azure Pipelines test UI
                context.AzurePipelines.Commands.PublishTestResults(new()
                {
                    Configuration = context.BuildSettings.Configuration,
                    TestResultsFiles = testResults.ToList(),
                    TestRunner = AzurePipelinesTestRunnerType.VSTest
                });

                // Publish result files as downloadable artifact
                foreach (var testResult in testResults)
                {
                    context.Log.Debug($"Publishing '{testResult}' as build artifact");
                    context.AzurePipelines.Commands.UploadArtifact(
                        folderName: "",
                        file: testResult,
                        context.AzurePipelines.ArtifactNames.TestResults
                    );
                }
            }
        }
    }
}
