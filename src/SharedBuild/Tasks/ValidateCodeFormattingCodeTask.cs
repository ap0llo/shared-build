using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Format;
using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.ValidateCodeFormatting)]
    [TaskDescription("Validates that all code files are formatted properly.")]
    [IsDependeeOf(typeof(ValidateTask))]
    public class ValidateCodeFormattingTask : FrostingTask<IBuildContext>
    {
        public override void Run(IBuildContext context)
        {
            var settings = new DotNetFormatSettings()
            {
                VerifyNoChanges = true
            };
            context.DotNetFormat(context.SolutionPath.FullPath, settings);
        }
    }
}
