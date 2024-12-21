using System;
using Cake.Common.IO;
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

        context.CreateDirectory(path);
        return new TemporaryDirectory(path, context.FileSystem);
    }
}
