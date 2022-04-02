using Cake.Common.Tools.DotNet;
using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.FormatCode)]
    [IsDependeeOf(typeof(GenerateTask))]
    [TaskDescription("Applies formatting rules to all code files")]
    public class FormatCodeTask : FrostingTask<IBuildContext>
    {
        public override bool ShouldRun(IBuildContext context) => context.CodeFormattingSettings.EnableAutomaticFormatting;

        public override void Run(IBuildContext context)
        {
            context.DotNetFormat(context.SolutionPath.FullPath);
        }
    }
}
