using System;
using Cake.Core.IO;

namespace Grynwald.SharedBuild.Tools.TemporaryFiles;

public class TemporaryDirectory(DirectoryPath path, IFileSystem fileSystem) : IDisposable
{
    public DirectoryPath Path { get; } = path ?? throw new ArgumentNullException(nameof(path));

    public void Dispose()
    {
        if (fileSystem.GetDirectory(Path).Exists)
        {
            fileSystem.GetDirectory(Path).Delete(recursive: true);
        }
    }
}
