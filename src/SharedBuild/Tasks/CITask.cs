using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks;

[TaskName(TaskNames.CI)]
[TaskDescription("Main entry point for the Continuous Integration Build")]
[IsDependentOn(typeof(PrintBuildContextTask))]
[IsDependentOn(typeof(SetBuildNumberTask))]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(TestTask))]
[IsDependentOn(typeof(ValidateTask))]
[IsDependentOn(typeof(PackTask))]
[IsDependentOn(typeof(GenerateChangeLogTask))]
[IsDependentOn(typeof(PushTask))]
[IsDependentOn(typeof(SetGitHubMilestoneTask))]
[IsDependentOn(typeof(CreateGitHubReleaseTask))]
public class CITask : FrostingTask
{ }
