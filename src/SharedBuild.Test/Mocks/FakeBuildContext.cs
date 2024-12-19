using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Grynwald.SharedBuild.Test.Mocks
{
    /// <summary>
    /// Mock implementation of <see cref="IBuildContext"/> (context including members specific to the "SharedBuild" project
    /// </summary>
    internal class FakeBuildContext : FakeCakeContext, IBuildContext
    {
        private FilePath? m_SolutionPath;
        private DirectoryPath? m_RootDirectory;


        public bool IsRunningInCI => throw new NotImplementedException();

        public DirectoryPath RootDirectory
        {
            get => m_RootDirectory ?? throw new InvalidOperationException($"Property '{nameof(RootDirectory)}' was not set");
            set => m_RootDirectory = value;
        }

        public FilePath SolutionPath
        {
            get => m_SolutionPath ?? throw new InvalidOperationException($"Property '{nameof(SolutionPath)}' was not set");
            set => m_SolutionPath = value;
        }

        /// <summary>
        /// Gets the mock for <see cref="IBuildContext.AzurePipelines"/>
        /// </summary>
        public FakeAzurePipelinesContext AzurePipelines { get; } = new();

        /// <inheritdoc />
        IAzurePipelinesContext IBuildContext.AzurePipelines => AzurePipelines;

        public IBuildSettings BuildSettings => throw new NotImplementedException();

        public ITestSettings TestSettings => throw new NotImplementedException();

        public IGitContext Git => throw new NotImplementedException();

        public IGitHubContext GitHub => throw new NotImplementedException();

        public IOutputContext Output => throw new NotImplementedException();

        public IReadOnlyCollection<IPushTarget> PushTargets => throw new NotImplementedException();

        /// <summary>
        /// Gets the mock for <see cref="IBuildContext.CodeFormattingSettings"/>
        /// </summary>
        public FakeCodeFormattingSettings CodeFormattingSettings { get; } = new();

        /// <inheritdoc />
        ICodeFormattingSettings IBuildContext.CodeFormattingSettings => CodeFormattingSettings;


        public void PrintToLog(ICakeLog log)
        {
            throw new NotImplementedException();
        }
    }
}
