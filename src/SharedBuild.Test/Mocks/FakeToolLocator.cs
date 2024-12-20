using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Grynwald.SharedBuild.Test.Mocks;

/// <summary>
/// Mock implementation of <see cref="IToolLocator"/>
/// </summary>
internal class FakeToolLocator : IToolLocator
{
    private Dictionary<string, FilePath> m_Tools = new Dictionary<string, FilePath>();


    public void RegisterFile(FilePath path) => throw new NotImplementedException();

    public FilePath Resolve(string tool) => throw new NotImplementedException();

    public FilePath Resolve(IEnumerable<string> toolExeNames)
    {
        foreach (var name in toolExeNames)
        {
            if (m_Tools.TryGetValue(name, out var resolved))
                return resolved;
        }

        return null!;
    }


    public void AddTool(string toolName, FilePath path)
    {
        m_Tools.Add(toolName, path);
    }
}
