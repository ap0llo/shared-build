namespace Grynwald.SharedBuild
{
    public interface IGitContext : IPrintableObject
    {
        /// <summary>
        /// Gets the name of the currently checked-out git branch
        /// </summary>
        string BranchName { get; }

        /// <summary>
        /// Gets the SHA of the currently checked-out commit
        /// </summary>
        string CommitId { get; }

        /// <summary>
        /// Gets the url of the default git remote
        /// </summary>
        string RemoteUrl { get; }

        /// <summary>
        /// Gets whether the current branch is repostitories main branch (either <c>main</c> or <c>master</c>)
        /// </summary>
        bool IsMainBranch { get; }

        /// <summary>
        /// Gets whether the current branch is a release branch
        /// </summary>
        bool IsReleaseBranch { get; }
    }
}
