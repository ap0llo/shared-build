using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;
using Cake.Testing;

namespace Grynwald.SharedBuild.Test.Mocks;

internal class FakeProcessRunner : IProcessRunner
{
    public record ProcessInvocation(FilePath FilePath, ProcessSettings Settings);


    private readonly List<ProcessInvocation> m_ProcessInvocations = new List<ProcessInvocation>();


    public IReadOnlyList<ProcessInvocation> ProcessInvocations => m_ProcessInvocations;


    public IProcess Start(FilePath filePath, ProcessSettings settings)
    {
        m_ProcessInvocations.Add(new ProcessInvocation(filePath, settings));
        return new FakeProcess();
    }
}
