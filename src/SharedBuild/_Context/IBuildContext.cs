using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Frosting;

namespace Grynwald.SharedBuild
{
    public interface IBuildContext : IFrostingContext, IPrintableObject
    {
        /// <summary>
        /// Gets whether the current build is running in a CI environment
        /// </summary>
        bool IsRunningInCI { get; }

        /// <summary>
        /// Gets the current project's root directory
        /// </summary>
        DirectoryPath RootDirectory { get; }

        /// <summary>
        /// Gets the path of the project's Visual Studio solution
        /// </summary>
        FilePath SolutionPath { get; }

        /// <summary>
        /// Gets the interface to Azure Pipelines when build is running in Azure Pipelines
        /// </summary>
        IAzurePipelinesContext AzurePipelines { get; }

        /// <summary>
        /// Gets the build settings to use
        /// </summary>
        IBuildSettings BuildSettings { get; }

        /// <summary>
        /// Gets the test settings to use
        /// </summary>
        ITestSettings TestSettings { get; }

        /// <summary>
        /// Gets information about the current Git workspace
        /// </summary>
        IGitContext Git { get; }

        /// <summary>
        /// Gets information about the current project on GitHub
        /// </summary>
        IGitHubContext GitHub { get; }

        /// <summary>
        /// Gets settings about the project's outputs (e.g. the outut path)
        /// </summary>
        IOutputContext Output { get; }

        /// <summary>
        /// Gets the sources to push NuGet packages to
        /// </summary>
        IReadOnlyCollection<IPushTarget> PushTargets { get; }

        /// <summary>
        /// Gets the settings for automatic formatting of source files.
        /// </summary>
        ICodeFormattingSettings CodeFormattingSettings { get; }
    }
}
