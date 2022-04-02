using Cake.Testing;

namespace Grynwald.SharedBuild.Test.Mocks
{
    internal static class FakeCakeContextExtensions
    {
        public static T AddDotNetCli<T>(this T context, string dotnetPath = "X:\\dotnet.exe") where T : FakeCakeContext
        {
            context.FileSystem.CreateFile(dotnetPath);
            context.Tools.AddTool("dotnet.exe", dotnetPath);
            context.Tools.AddTool("dotnet", dotnetPath);
            return context;
        }
    }
}
