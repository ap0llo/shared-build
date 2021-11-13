using System;
using System.Linq;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Grynwald.SharedBuild.Tools.DotNet
{
    internal class DotNetFormatter : DotNetCoreTool<DotNetFormatSettings>
    {
        public DotNetFormatter(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        { }


        public void Format(string? project, DotNetFormatSettings? settings)
        {
            settings ??= new();
            RunCommand(settings, GetArguments(project, settings));
        }


        private ProcessArgumentBuilder GetArguments(string? project, DotNetFormatSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("format");

            if (!String.IsNullOrEmpty(project))
            {
                builder.AppendQuoted(project);
            }

            if (settings.VerifyNoChanges)
            {
                builder.Append("--verify-no-changes");
            }

            return builder;
        }
    }
}
