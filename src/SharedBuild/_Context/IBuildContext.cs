using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Frosting;

namespace Grynwald.SharedBuild
{
    public interface IBuildContext : IFrostingContext
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
        AzurePipelinesContext AzurePipelines { get; }

        /// <summary>
        /// Gets the build settings to use
        /// </summary>
        BuildSettings BuildSettings { get; }

        /// <summary>
        /// Gets information about the current Git workspace
        /// </summary>
        GitContext Git { get; }

        /// <summary>
        /// Gets information about the current project on GitHub
        /// </summary>
        GitHubContext GitHub { get; }

        /// <summary>
        /// Gets settings about the project's outputs (e.g. the outut path)
        /// </summary>
        OutputContext Output { get; }

        IReadOnlyCollection<PushTarget> PushTargets { get; }




        void PrintToLog(int indentWidth = 0);
    }
}
