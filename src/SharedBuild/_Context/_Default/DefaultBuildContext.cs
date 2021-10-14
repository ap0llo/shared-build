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
        public virtual bool IsRunningInCI => AzurePipelines.IsActive;

        /// <inheritdoc />
        public virtual DirectoryPath RootDirectory { get; }

        /// <inheritdoc />
        public virtual FilePath SolutionPath
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
        public virtual IAzurePipelinesContext AzurePipelines { get; }

        /// <inheritdoc />
        public virtual IBuildSettings BuildSettings { get; }

        /// <inheritdoc />
        public virtual IGitContext Git { get; }

        /// <inheritdoc />
        public virtual IGitHubContext GitHub { get; }

        /// <inheritdoc />
        public virtual IOutputContext Output { get; }

        /// <inheritdoc />
        public virtual IReadOnlyCollection<IPushTarget> PushTargets { get; } = Array.Empty<IPushTarget>();


        /// <summary>
        /// Initializes a new instance of <see cref="DefaultBuildContext"/>
        /// </summary>
        public DefaultBuildContext(ICakeContext context) : base(context)
        {
            RootDirectory = context.Environment.WorkingDirectory;

            AzurePipelines = new DefaultAzurePipelinesContext(this);
            Git = new DefaultGitContext(this);
            GitHub = new DefaultGitHubContext(this);
            Output = new DefaultOutputContext(this);
            BuildSettings = new DefaultBuildSettings(this);
        }


        /// <inheritdoc />
        public virtual void PrintToLog(ICakeLog log)
        {
            var indentedLog = new IndentedCakeLog(log);

            log.Information($"{nameof(IsRunningInCI)}: {IsRunningInCI}");

            log.Information($"{nameof(RootDirectory)}: {RootDirectory.FullPath}");

            log.Information($"{nameof(SolutionPath)}: {SolutionPath.FullPath}");

            log.Information(nameof(Output));
            Output.PrintToLog(indentedLog);

            log.Information(nameof(BuildSettings));
            BuildSettings.PrintToLog(indentedLog);

            log.Information(nameof(Git));
            Git.PrintToLog(indentedLog);

            log.Information(nameof(AzurePipelines));
            AzurePipelines.PrintToLog(indentedLog);

            log.Information(nameof(GitHub));
            GitHub.PrintToLog(indentedLog);

            log.Information($"{nameof(PushTargets)}:");
            var index = 0;
            foreach (var pushTarget in PushTargets)
            {
                indentedLog.Information($"{nameof(PushTargets)}[{index}]:");
                pushTarget.PrintToLog(new IndentedCakeLog(indentedLog));
                index++;
            }
        }
    }
}
