using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing;

namespace Grynwald.SharedBuild.Test.Mocks
{
    /// <summary>
    /// Mock implementation of <see cref="ICakeContext"/>
    /// </summary>
    internal class FakeCakeContext : ICakeContext
    {
        /// <summary>
        /// Mock object for <see cref="ICakeContext.FileSystem"/>
        /// </summary>
        public FakeFileSystem FileSystem { get; }

        /// <summary>
        /// Mock object for <see cref="ICakeContext.Environment"/>
        /// </summary>
        public FakeEnvironment Environment { get; }

        public IGlobber Globber => throw new System.NotImplementedException();

        public ICakeLog Log => throw new System.NotImplementedException();

        public ICakeArguments Arguments => throw new System.NotImplementedException();

        /// <summary>
        /// Mock object for <see cref="ICakeContext.ProcessRunner" />
        /// </summary>
        public FakeProcessRunner ProcessRunner { get; }

        public IRegistry Registry => throw new System.NotImplementedException();

        /// <summary>
        /// Mock object for <see cref="ICakeContext.Tools" />
        /// </summary>
        public FakeToolLocator Tools { get; }

        public ICakeDataResolver Data => throw new System.NotImplementedException();

        public ICakeConfiguration Configuration => throw new System.NotImplementedException();

        /// <inheritdoc />
        IProcessRunner ICakeContext.ProcessRunner => ProcessRunner;

        /// <inheritdoc />
        IFileSystem ICakeContext.FileSystem => FileSystem;

        /// <inheritdoc />
        ICakeEnvironment ICakeContext.Environment => Environment;

        /// <inheritdoc />
        IToolLocator ICakeContext.Tools => Tools;

        public FakeCakeContext()
        {
            Environment = FakeEnvironment.CreateWindowsEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            ProcessRunner = new FakeProcessRunner();
            Tools = new FakeToolLocator();
        }
    }
}
