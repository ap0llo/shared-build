using Cake.Common.Tools.DotNetCore;

namespace Grynwald.SharedBuild.Tools.DotNet
{
    public class DotNetFormatSettings : DotNetCoreSettings
    {
        public bool VerifyNoChanges { get; set; } = false;
    }
}
