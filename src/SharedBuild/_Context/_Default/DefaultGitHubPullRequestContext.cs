using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild;

public class DefaultGitHubPullRequestContext : IGitHubPullRequestContext
{
    /// <inheritdoc />
    public bool IsPullRequest { get; }

    /// <inheritdoc />
    public int Number { get; }


    public DefaultGitHubPullRequestContext(DefaultBuildContext context)
    {
        //TODO: Can this also be determined when building locally (or on a CI system other than Azure Pipelines)
        if (context.AzurePipelines.IsActive)
        {
            IsPullRequest =
                context.AzurePipelines.Environment.Repository.Provider == AzurePipelinesRepositoryType.GitHub &&
                // Do not use context.AzurePipelines.Environment.PullRequest.IsPullRequest, since that returns false since GitHub's PR ids exceeded int.MaxValue
                // (see https://github.com/cake-build/cake/issues/4410)
                // Instead, check the precense of a PR number
                context.AzurePipelines.Environment.PullRequest.Number > 0;

            Number = context.AzurePipelines.Environment.PullRequest.Number;
        }
        else
        {
            IsPullRequest = false;
            Number = 0;
        }
    }



    public void PrintToLog(ICakeLog log)
    {
        log.Information($"{nameof(IsPullRequest)}: {IsPullRequest}");
        log.Information($"{nameof(Number)}: {Number}");
    }
}
