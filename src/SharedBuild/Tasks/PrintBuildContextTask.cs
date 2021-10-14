using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName("PrintBuildContext")]
    public class PrintBuildContextTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context) => context.PrintToLog();
    }
}
