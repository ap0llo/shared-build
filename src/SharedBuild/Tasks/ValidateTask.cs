using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks;

[TaskName(TaskNames.Validate)]
[TaskDescription("Validates all files under source control")]
public class ValidateTask : FrostingTask;
