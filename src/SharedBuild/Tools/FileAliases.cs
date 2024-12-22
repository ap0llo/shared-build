using Cake.Core;
using Cake.Core.IO;

namespace Grynwald.SharedBuild.Tools;

public static class FileAliases
{
    public static void EnsureFileDoesNotExist(this ICakeContext context, FilePath path)
    {
        var file = context.FileSystem.GetFile(path);
        if (file.Exists)
        {
            file.Delete();
        }
    }
}
