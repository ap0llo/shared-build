using System;

namespace Grynwald.SharedBuild
{
    public static class PushTargets
    {
        public static IPushTarget NuGetOrg(Func<IBuildContext, bool> isActive) => new PushTarget(
            type: PushTargetType.NuGetOrg,
            feedUrl: "https://api.nuget.org/v3/index.json",
            isActive: isActive
        );
    }
}
