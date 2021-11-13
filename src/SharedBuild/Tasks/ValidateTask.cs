using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.Validate)]
    [TaskDescription("Validates all files under source control")]
    [IsDependentOn(typeof(ValidateCodeFormattingCodeTask))]
    public class ValidateTask : FrostingTask
    {
    }
}
