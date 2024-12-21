using System;
using Cake.Core;
using Cake.Core.IO;

namespace Grynwald.SharedBuild.Tools.TemporaryFiles;

public static class TemporaryDirectoryAliases
{
    public static TemporaryDirectory CreateTemporaryDirectory(this ICakeContext context)
    {
        var path = context.Environment
            .GetSpecialPath(SpecialPath.LocalTemp)
            .Combine($"{Guid.NewGuid():n}");

        return new TemporaryDirectory(path, context.FileSystem);
    }
}
