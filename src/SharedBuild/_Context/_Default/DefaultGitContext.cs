using System;
using Cake.Core.Diagnostics;
using Cake.Git;

namespace Grynwald.SharedBuild;

public class DefaultGitContext(DefaultBuildContext context) : IGitContext
{
    private readonly DefaultBuildContext m_Context = context ?? throw new ArgumentNullException(nameof(context));

    /// <inheritdoc />
    public virtual string BranchName
    {
        get
        {
            if (m_Context.AzurePipelines.IsActive)
            {
                var branchName = m_Context.AzurePipelines.Environment.Repository.SourceBranch;

                if (branchName.StartsWith("refs/heads/"))
                {
                    branchName = branchName["refs/heads/".Length..];
                }

                return branchName;
            }
            else
            {
                return m_Context.GitBranchCurrent(context.RootDirectory).FriendlyName;
            }
        }
    }

    /// <inheritdoc />
    public virtual string CommitId => m_Context.AzurePipelines.IsActive
        ? m_Context.AzurePipelines.Environment.Repository.SourceVersion
        : m_Context.GitBranchCurrent(context.RootDirectory).Tip.Sha;

    /// <inheritdoc />
    public virtual string RemoteUrl => m_Context.GitRemote(context.RootDirectory, "origin").Url;

    /// <inheritdoc />
    public virtual bool IsMainBranch => BranchName.Equals("main", StringComparison.OrdinalIgnoreCase) || BranchName.Equals("master", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public virtual bool IsReleaseBranch => BranchName.StartsWith("release/", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public virtual void PrintToLog(ICakeLog log)
    {
        log.Information($"{nameof(BranchName)}: {BranchName}");
        log.Information($"{nameof(CommitId)}: {CommitId}");
        log.Information($"{nameof(RemoteUrl)}: {RemoteUrl}");
        log.Information($"{nameof(IsMainBranch)}: {IsMainBranch}");
        log.Information($"{nameof(IsReleaseBranch)}: {IsReleaseBranch}");
    }
}
