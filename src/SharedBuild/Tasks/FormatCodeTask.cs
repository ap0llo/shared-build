using Cake.Frosting;
using Grynwald.SharedBuild.Tools.DotNet;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.FormatCode)]
    [IsDependeeOf(typeof(GenerateTask))]
    [TaskDescription("Applies formatting rules to all code files")]
    public class FormatCodeTask : FrostingTask<IBuildContext>
    {
        public override void Run(IBuildContext context)
        {
            var settings = new DotNetFormatSettings();
            context.DotNetFormat(context.SolutionPath.FullPath, settings);
        }
    }
}
