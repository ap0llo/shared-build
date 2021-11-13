using System;
using Cake.AzurePipelines.Module;
using Cake.Core;
using Cake.Frosting;
using Grynwald.SharedBuild;

return new CakeHost()
    .UseContext<BuildContext>()
    .UseModule<AzurePipelinesModule>()
    //TODO: Use Tool manifest
    .InstallTool(new Uri("dotnet:?package=Grynwald.ChangeLog&version=0.4.135"))
    .InstallTool(new Uri("dotnet:?package=dotnet-reportgenerator-globaltool&version=5.0.0"))
    .Run(args);


class BuildContext : DefaultBuildContext
{
    public BuildContext(ICakeContext context) : base(context)
    {
    }
}
