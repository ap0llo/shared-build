using Cake.Core;

namespace Grynwald.SharedBuild.Tools.DotNet
{
    public static class DotNetAliases
    {
        public static void DotNetFormat(this ICakeContext context, string? project, DotNetFormatSettings? settings)
        {
            var formatter = new DotNetFormatter(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            formatter.Format(project, settings);
        }
    }
}
