using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks;

[TaskName(TaskNames.Default)]
[IsDependentOn(typeof(PrintBuildContextTask))]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(TestTask))]
[IsDependentOn(typeof(PackTask))]
public class DefaultTask : FrostingTask<IBuildContext>;
