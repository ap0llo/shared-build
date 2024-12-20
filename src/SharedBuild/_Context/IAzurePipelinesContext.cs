using Cake.Common.Build.AzurePipelines;

namespace Grynwald.SharedBuild;

public interface IAzurePipelinesContext : IAzurePipelinesProvider, IPrintableObject
{
    /// <summary>
    /// Gets the names to use for publishing pipeline artifacts
    /// </summary>
    IAzurePipelinesArtifactNames ArtifactNames { get; }

    /// <summary>
    /// Gets whether the build is currently running on Azure Pipelines
    /// </summary>
    bool IsActive { get; }
}
