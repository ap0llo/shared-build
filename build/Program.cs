using System.Collections.Generic;
using Cake.AzurePipelines.Module;
using Cake.Core;
using Cake.DotNetLocalTools.Module;
using Cake.Frosting;
using Grynwald.SharedBuild;

return new CakeHost()
    .UseContext<BuildContext>()
    // Usage of AzurePipelinesModule temporarily commented out since it does not yet support Cake 3.0
    //.UseModule<AzurePipelinesModule>()
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
            isActive: context => context.Git.IsMainBranch || context.Git.IsReleaseBranch
        )
    };

    public BuildContext(ICakeContext context) : base(context)
    {
        // Exclude the "deps" directory from code formatting (contains submodules for which this project's formatting rules do not apply)
        CodeFormattingSettings.ExcludedDirectories = new[]
        {
            RootDirectory.Combine("deps")
        };
    }
}
