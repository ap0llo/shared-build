using System;
using Cake.Common.Build.AzurePipelines;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild.Test.Mocks;

internal class FakeAzurePipelinesContext : IAzurePipelinesContext
{
    public IAzurePipelinesArtifactNames ArtifactNames => throw new NotImplementedException();

    public bool IsActive
    {
        get => IsRunningOnAzurePipelines;
        set => IsRunningOnAzurePipelines = value;
    }

    public bool IsRunningOnAzurePipelines { get; set; }

    public AzurePipelinesEnvironmentInfo Environment => throw new NotImplementedException();

    public IAzurePipelinesCommands Commands => throw new NotImplementedException();

    public void PrintToLog(ICakeLog log)
    {
        throw new NotImplementedException();
    }
}
