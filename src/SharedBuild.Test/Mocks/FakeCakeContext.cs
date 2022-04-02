using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Grynwald.SharedBuild.Test.Mocks
{
    /// <summary>
    /// Mock implementation of <see cref="ICakeContext"/>
    /// </summary>
    internal class FakeCakeContext : ICakeContext
    {
        public IFileSystem FileSystem => throw new System.NotImplementedException();

        public ICakeEnvironment Environment => throw new System.NotImplementedException();

        public IGlobber Globber => throw new System.NotImplementedException();

        public ICakeLog Log => throw new System.NotImplementedException();

        public ICakeArguments Arguments => throw new System.NotImplementedException();

        public IProcessRunner ProcessRunner => throw new System.NotImplementedException();

        public IRegistry Registry => throw new System.NotImplementedException();

        public IToolLocator Tools => throw new System.NotImplementedException();

        public ICakeDataResolver Data => throw new System.NotImplementedException();

        public ICakeConfiguration Configuration => throw new System.NotImplementedException();
    }
}
