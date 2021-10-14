using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName("Default")]
    [IsDependentOn(typeof(BuildTask))]
    [IsDependentOn(typeof(TestTask))]
    [IsDependentOn(typeof(PackTask))]
    public class DefaultTask : FrostingTask<BuildContext>
    { }
}
