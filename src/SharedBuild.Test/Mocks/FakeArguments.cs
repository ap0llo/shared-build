using System;
using System.Collections.Generic;
using Cake.Core;

namespace Grynwald.SharedBuild.Test.Mocks
{
    internal class FakeArguments : ICakeArguments
    {
        public ICollection<string> GetArguments(string name) => Array.Empty<string>();

        public IDictionary<string, ICollection<string>> GetArguments() => throw new NotImplementedException();

        public bool HasArgument(string name) => throw new NotImplementedException();
    }
}
