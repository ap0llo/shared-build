using System.Collections.Generic;
using Cake.AzurePipelines.Module;
using Cake.Core;
using Cake.DotNetLocalTools.Module;
using Cake.Frosting;
using Grynwald.SharedBuild;

return new CakeHost()
    .UseContext<BuildContext>()
    .UseModule<AzurePipelinesModule>()
    .UseModule<LocalToolsModule>()
    .InstallToolsFromManifest(".config/dotnet-tools.json")
    .Run(args);


class BuildContext : DefaultBuildContext
{

    public override IReadOnlyCollection<IPushTarget> PushTargets { get; } = new IPushTarget[]
    {
        new PushTarget(
            type: PushTargetType.AzureArtifacts,
            feedUrl: "https://pkgs.dev.azure.com/ap0llo/OSS/_packaging/BuildInfrastructure/nuget/v3/index.json",
            isActive: context => context.Git.IsMasterBranch || context.Git.IsReleaseBranch
        )
    };

    public BuildContext(ICakeContext context) : base(context)
    { }
}
