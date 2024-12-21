using Cake.Common.Build;
using Cake.Common.Build.AzurePipelines;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild;

public class DefaultAzurePipelinesContext(DefaultBuildContext context) : IAzurePipelinesContext
{
    private readonly IAzurePipelinesProvider m_AzurePipelinesProvider = context.AzurePipelines();


    /// <inheritdoc />
    public virtual IArtifactNames ArtifactNames { get; } = new DefaultArtifactNames();

    /// <inheritdoc />
    public virtual bool IsActive => IsRunningOnAzurePipelines;

    /// <inheritdoc />
    public virtual bool IsRunningOnAzurePipelines => m_AzurePipelinesProvider.IsRunningOnAzurePipelines;

    /// <inheritdoc />
    public virtual AzurePipelinesEnvironmentInfo Environment => m_AzurePipelinesProvider.Environment;

    /// <inheritdoc />
    public virtual IAzurePipelinesCommands Commands => m_AzurePipelinesProvider.Commands;


    /// <inheritdoc />
    public virtual void PrintToLog(ICakeLog log)
    {
        log.Information($"{nameof(IsActive)}: {IsActive}");

        log.Information($"{nameof(ArtifactNames)}:");
        ArtifactNames.PrintToLog(new IndentedCakeLog(log));
    }
}
