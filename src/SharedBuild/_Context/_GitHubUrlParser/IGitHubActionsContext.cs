namespace Grynwald.SharedBuild;

public interface IGitHubActionsContext : IPrintableObject
{
    /// <summary>
    /// Gets whether the build is currently running in GitHub Actions
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Gets the names to use for publishing pipeline artifacts
    /// </summary>
    IArtifactNames ArtifactNames { get; }
}
