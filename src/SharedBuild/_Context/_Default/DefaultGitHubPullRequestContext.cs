using System;
using Cake.Common.Build;
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
        else if (context.GitHubActions.IsActive)
        {
            IsPullRequest = context.GitHubActions().Environment.PullRequest.IsPullRequest;

            if (IsPullRequest)
            {
                // GitHub Actions has no predefined variable that specifies the PR number for a PR build.
                // Instead, the PR_NUMBER variable needs to be defined and populated in the GitHub Actions workflow, like this
                //
                //  env:
                //    PR_NUMBER: ${{ github.event.number }}
                //
                // If the PR_NUMBER variable is not set, emit a warning

                var prNumberVar = context.Environment.GetEnvironmentVariable("PR_NUMBER");
                if (!String.IsNullOrWhiteSpace(prNumberVar) && int.TryParse(prNumberVar, out var prNumber))
                {
                    Number = prNumber;
                }
                else
                {
                    context.Log.Warning("Current build seems to be a PR build but the Pull Request number could not be determined. " +
                                        "Make sure the PR_NUMBER variable is set in the GitHub Actions workflow. " +
                                        "This is required since GitHub Actions does not a provide a predefined variable for this.");
                }
            }
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
