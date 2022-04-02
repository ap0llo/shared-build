using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild
{
    public class DefaultCodeFormattingSettings : ICodeFormattingSettings
    {
        public virtual bool EnableAutomaticFormatting => true;


        public void PrintToLog(ICakeLog log)
        {
            log.Information($"{nameof(EnableAutomaticFormatting)}: {EnableAutomaticFormatting}");
        }
    }
}
