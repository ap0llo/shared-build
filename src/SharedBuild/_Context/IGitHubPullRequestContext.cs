namespace Grynwald.SharedBuild;

public interface IGitHubPullRequestContext : IPrintableObject
{
    /// <summary>
    /// Determines whether the current build is building a Pull Request
    /// </summary>
    bool IsPullRequest { get; }

    /// <summary>
    /// Gets the number of the GitHub Pull Request when <see cref="IsPullRequest"/> is <c>true</c>, otherwise returns false.
    /// </summary>
    int Number { get; }
}
