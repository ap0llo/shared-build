using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.PrintBuildContext)]
    public class PrintBuildContextTask : FrostingTask<IBuildContext>
    {
        public override void Run(IBuildContext context) => context.PrintToLog();
    }
}
