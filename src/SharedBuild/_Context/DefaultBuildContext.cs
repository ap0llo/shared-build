using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;

namespace Grynwald.SharedBuild
{
    public class DefaultBuildContext : FrostingContext, IBuildContext
    {
        /// <inheritdoc />
        public bool IsRunningInCI => AzurePipelines.IsActive;

        /// <inheritdoc />
        public DirectoryPath RootDirectory { get; }

        /// <inheritdoc />
        public FilePath SolutionPath
        {
            get
            {
                var solutionFiles = FileSystem.GetFilePaths(RootDirectory, "*.sln", SearchScope.Current);

                return solutionFiles.Count switch
                {
                    < 0 => throw new InvalidOperationException(), // Cannot happen, count will always be >= 0
                    0 => throw new Exception($"No solution files found in '{RootDirectory}'"),
                    1 => solutionFiles[0],
                    > 1 => throw new Exception($"Multiple solution files found in '{RootDirectory}'")
                };
            }
        }

        /// <inheritdoc />
        public AzurePipelinesContext AzurePipelines { get; }

        /// <inheritdoc />
        public BuildSettings BuildSettings { get; }

        /// <inheritdoc />
        public GitContext Git { get; }

        /// <inheritdoc />
        public GitHubContext GitHub { get; }

        /// <inheritdoc />
        public OutputContext Output { get; }

        /// <summary>
        /// Gets the sources to push the build's NuGet packages to
        /// </summary>
        public IReadOnlyCollection<PushTarget> PushTargets { get; }


        /// <summary>
        /// Initializes a new instance of <see cref="DefaultBuildContext"/>
        /// </summary>
        public DefaultBuildContext(ICakeContext context) : base(context)
        {
            RootDirectory = context.Environment.WorkingDirectory;

            // TODO: Remove hard-coded feed url
            PushTargets = new[]
            {
                new PushTarget(
                    this,
                    PushTargetType.AzureArtifacts,
                    "https://pkgs.dev.azure.com/ap0llo/OSS/_packaging/Cake.GitHubReleases/nuget/v3/index.json",
                    context => context.Git.IsMasterBranch || context.Git.IsReleaseBranch
                ),
                new PushTarget(
                    this,
                    PushTargetType.NuGetOrg,
                    "https://api.nuget.org/v3/index.json",
                    context => context.Git.IsReleaseBranch
                ),
            };


            AzurePipelines = new(this);
            Git = new(this);
            GitHub = new(this);
            Output = new(this);
            BuildSettings = new(this);
        }


        /// <summary>
        /// Prints the context's data to the log
        /// </summary>
        public void PrintToLog(int indentWidth = 0)
        {
            static string prefix(int width) => new String(' ', width);

            Log.Information($"{prefix(indentWidth)}{nameof(IsRunningInCI)}: {IsRunningInCI}");

            Log.Information($"{prefix(indentWidth)}{nameof(RootDirectory)}: {RootDirectory.FullPath}");

            Log.Information($"{prefix(indentWidth)}{nameof(SolutionPath)}: {SolutionPath.FullPath}");

            Log.Information($"{prefix(indentWidth)}{nameof(Output)}:");
            Output.PrintToLog(indentWidth + 2);

            Log.Information($"{prefix(indentWidth)}{nameof(BuildSettings)}:");
            BuildSettings.PrintToLog(indentWidth + 2);

            Log.Information($"{prefix(indentWidth)}{nameof(Git)}:");
            Git.PrintToLog(indentWidth + 2);

            Log.Information($"{prefix(indentWidth)}{nameof(AzurePipelines)}:");
            AzurePipelines.PrintToLog(indentWidth + 2);

            Log.Information($"{prefix(indentWidth)}{nameof(GitHub)}:");
            GitHub.PrintToLog(indentWidth + 2);

            // 
            Log.Information($"{nameof(PushTargets)}:");
            int index = 0;
            foreach (var pushTarget in PushTargets)
            {
                Log.Information($"{prefix(indentWidth + 2)}{nameof(PushTargets)}[{index}]:");
                pushTarget.PrintToLog(indentWidth + 4);
                index++;
            }
        }
    }
}
