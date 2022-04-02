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
        public bool IsRunningInCI => throw new NotImplementedException();

        public DirectoryPath RootDirectory => throw new NotImplementedException();

        public FilePath SolutionPath => throw new NotImplementedException();

        public IAzurePipelinesContext AzurePipelines => throw new NotImplementedException();

        public IBuildSettings BuildSettings => throw new NotImplementedException();

        public ITestSettings TestSettings => throw new NotImplementedException();

        public IGitContext Git => throw new NotImplementedException();

        public IGitHubContext GitHub => throw new NotImplementedException();

        public IOutputContext Output => throw new NotImplementedException();

        public IReadOnlyCollection<IPushTarget> PushTargets => throw new NotImplementedException();

        /// <summary>
        /// Mock object for <see cref="IBuildContext.CodeFormattingSettings"/>
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
