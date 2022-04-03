namespace Grynwald.SharedBuild
{
    public interface IGitHubContext : IPrintableObject
    {
        /// <summary>
        /// Gets the host name of the GitHub server
        /// </summary>
        string HostName { get; }

        /// <summary>
        /// Gets the name of the repository owner (user or group) on GitHub
        /// </summary>
        string RepositoryOwner { get; }

        /// <summary>
        /// Gets the name of the repository on Github
        /// </summary>
        string RepositoryName { get; }

        /// <summary>
        /// Gets information about the Pull Request being built
        /// </summary>
        IGitHubPullRequestContext PullRequest { get; }

        /// <summary>
        /// Tries to get the GitHub Access token for the current builds
        /// </summary>
        string? TryGetAccessToken();
    }
}
