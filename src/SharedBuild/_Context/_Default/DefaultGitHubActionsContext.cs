using Cake.Common.Build;
using Cake.Common.Build.GitHubActions;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild;

public class DefaultGitHubActionsContext(DefaultBuildContext context) : IGitHubActionsContext
{
    private readonly IGitHubActionsProvider m_GitHubActionsProvider = context.GitHubActions();

    public bool IsActive => m_GitHubActionsProvider.IsRunningOnGitHubActions;

    /// <inheritdoc />
    public virtual IArtifactNames ArtifactNames { get; } = new DefaultArtifactNames();

    public void PrintToLog(ICakeLog log)
    {
        log.Information($"{nameof(IsActive)}: {IsActive}");

        log.Information($"{nameof(ArtifactNames)}:");
        ArtifactNames.PrintToLog(new IndentedCakeLog(log));
    }
}
