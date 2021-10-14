using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.PrintBuildContext)]
    public class PrintBuildContextTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context) => context.PrintToLog();
    }
}
