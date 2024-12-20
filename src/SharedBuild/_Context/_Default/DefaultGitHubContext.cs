using System;
using Cake.Common;
using Cake.Core.Diagnostics;
using Grynwald.ChangeLog.Integrations.GitHub;

namespace Grynwald.SharedBuild;

public class DefaultGitHubContext : IGitHubContext
{
    private readonly DefaultBuildContext m_Context;
    private readonly Lazy<GitHubProjectInfo> m_ProjectInfo;


    /// <inheritdoc />
    public virtual string HostName => m_ProjectInfo.Value.Host;

    /// <inheritdoc />
    public virtual string RepositoryOwner => m_ProjectInfo.Value.Owner;

    /// <inheritdoc />
    public virtual string RepositoryName => m_ProjectInfo.Value.Repository;

    /// <inheritdoc />
    public IGitHubPullRequestContext PullRequest { get; }


    public DefaultGitHubContext(DefaultBuildContext context)
    {
        m_Context = context ?? throw new ArgumentNullException(nameof(context));
        m_ProjectInfo = new Lazy<GitHubProjectInfo>(() => GitHubUrlParser.ParseRemoteUrl(m_Context.Git.RemoteUrl));
        PullRequest = new DefaultGitHubPullRequestContext(context);
    }


    /// <inheritdoc />
    public virtual string? TryGetAccessToken()
    {
        if (m_Context.EnvironmentVariable("GITHUB_ACCESSTOKEN") is string { Length: > 0 } accessToken)
        {
            return accessToken;
        }
        else
        {
            return null;
        }
    }

    /// <inheritdoc />
    public void PrintToLog(ICakeLog log)
    {
        var indentedLog = new IndentedCakeLog(log);

        log.Information($"{nameof(HostName)}: {HostName}");
        log.Information($"{nameof(RepositoryOwner)}: {RepositoryOwner}");
        log.Information($"{nameof(RepositoryName)}: {RepositoryName}");
        log.Information($"{nameof(PullRequest)}:");
        PullRequest.PrintToLog(indentedLog);
    }
}
