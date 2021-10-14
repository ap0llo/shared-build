using Cake.Core.Diagnostics;
using Cake.Frosting;
using Cake.GitVersioning;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.SetBuildNumber)]
    [TaskDescription("Sets the build number when running in a CI system")]
    public class SetBuildNumberTask : FrostingTask<IBuildContext>
    {
        public override bool ShouldRun(IBuildContext context) => context.IsRunningInCI;

        public override void Run(IBuildContext context)
        {
            context.Log.Information("Setting Build Number using Nerdbank.GitVersioning");
            context.GitVersioningCloud(context.RootDirectory.FullPath, new() { AllVariables = true });
        }
    }
}
