using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Format;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks;

[TaskName(TaskNames.ValidateCodeFormatting)]
[TaskDescription("Validates that all code files are formatted properly.")]
[IsDependeeOf(typeof(ValidateTask))]
public class ValidateCodeFormattingTask : FrostingTask<IBuildContext>
{
    public override bool ShouldRun(IBuildContext context)
    {
        if (!context.CodeFormattingSettings.EnableAutomaticFormatting)
        {
            return false;
        }


        // Running dotnet format on Azure Pipelines when using the .NET 9 SDK and Nerdbank.GitVersioning causes the build to hang
        // See: https://github.com/dotnet/sdk/issues/44951
        // To work around this, skip the task when running on Azure Pipelines
        if (context.AzurePipelines.IsRunningOnAzurePipelines)
        {
            context.Log.Warning($"Skipping task {TaskNames.ValidateCodeFormatting} since the build is running on Azure Pipelines. This is a workaround for https://github.com/dotnet/sdk/issues/44951");
            return false;
        }

        return true;
    }

    public override void Run(IBuildContext context)
    {
        var settings = new DotNetFormatSettings()
        {
            VerifyNoChanges = true,
            NoRestore = true
        };

        // "dotnet format" expects the excluded directories to be passed as realtive paths (relative to the process' working directory)
        // To ensure that, start "dotnet format" in the repository root directory and convert all exclude paths to relative paths.
        // Since the DotNetFormat() alias does not seem to handle this out-of-the-box, use ArgumentCustomization to add the --exclude parameter
        settings.WorkingDirectory = context.RootDirectory;
        if (context.CodeFormattingSettings.ExcludedDirectories?.Count > 0)
        {
            settings.ArgumentCustomization = args =>
            {
                args.Append("--exclude");
                foreach (var excludedDirectory in context.CodeFormattingSettings.ExcludedDirectories)
                {
                    var absolutePath = excludedDirectory.MakeAbsolute(context.Environment.WorkingDirectory);
                    var relativePath = settings.WorkingDirectory.GetRelativePath(absolutePath);
                    args.AppendQuoted(relativePath.ToString());
                }
                return args;
            };

        }

        context.DotNetFormat(context.SolutionPath.FullPath, settings);
    }
}
