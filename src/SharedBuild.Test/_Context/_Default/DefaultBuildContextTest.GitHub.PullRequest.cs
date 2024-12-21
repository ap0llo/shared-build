using Cake.Core.Diagnostics;
using Grynwald.SharedBuild.Test.Mocks;
using Xunit;

namespace Grynwald.SharedBuild.Test;

/// <summary>
/// Tests for <see cref="DefaultGitHubPullRequestContext"/>
/// </summary>
public partial class DefaultBuildContextTest
{
    public class GitHub
    {
        public class PullRequest
        {
            [Theory]
            [InlineData(null, null, null, false)]
            [InlineData("true", null, null, false)]
            [InlineData("true", "0", null, false)]
            [InlineData("true", "23", null, false)]
            [InlineData("true", "23", "TfsGit", false)]
            [InlineData("true", "23", "GitHub", true)]
            public void IsPullRequest_returns_expected_value_when_building_a_GitHub_PR_on_Azure_Pipelines(string? tfBuild, string? system_PullRequest_PullRequestNumber, string? build_Repository_Provider, bool expected)
            {
                // ARRANGE
                var context = new FakeCakeContext();
                context.Environment.SetEnvironmentVariable("TF_BUILD", tfBuild);
                context.Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER", system_PullRequest_PullRequestNumber);
                context.Environment.SetEnvironmentVariable("BUILD_REPOSITORY_PROVIDER", build_Repository_Provider);

                var sut = new DefaultBuildContext(context);

                // ACT
                var isPullRequest = sut.GitHub.PullRequest.IsPullRequest;

                // ASSERT
                Assert.Equal(expected, isPullRequest);
            }

            [Theory]
            [InlineData(null, null, false)]
            [InlineData("true", null, false)]
            [InlineData("true", "pull_request", true)]
            [InlineData("true", "some_event_name", false)]
            [InlineData("", "pull_request", false)]
            [InlineData("false", "pull_request", false)]
            public void IsPullRequest_returns_expected_value_when_building_a_GitHub_PR_on_GitHub_Actions(string? githubActionsEnvironmentVariable, string? githubEventNameEnvironmentVariable, bool expected)
            {
                // ARRANGE
                var context = new FakeCakeContext();
                context.Environment.SetEnvironmentVariable("GITHUB_ACTIONS", githubActionsEnvironmentVariable);
                context.Environment.SetEnvironmentVariable("GITHUB_EVENT_NAME", githubEventNameEnvironmentVariable);

                var sut = new DefaultBuildContext(context);

                // ACT
                var isPullRequest = sut.GitHub.PullRequest.IsPullRequest;

                // ASSERT
                Assert.Equal(expected, isPullRequest);
            }


            [Theory]
            [InlineData(null, null, null, null, 0)]
            [InlineData("true", null, null, null, 0)]
            [InlineData("true", "0", null, null, 0)]
            [InlineData("true", "23", null, null, 0)]
            [InlineData("true", "23", "TfsGit", null, 0)]
            [InlineData("true", "23", "GitHub", "42", 42)]
            public void PullRequestNumber_returns_expected_value_when_building_a_GitHub_PR_on_Azure_Pipelines(string? tfBuild, string? system_PullRequest_PullRequestId, string? build_Repository_Provider, string? system_PullRequest_PullRequestNumber, int expected)
            {
                // ARRANGE
                var context = new FakeCakeContext();
                context.Environment.SetEnvironmentVariable("TF_BUILD", tfBuild);
                context.Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID", system_PullRequest_PullRequestId);
                context.Environment.SetEnvironmentVariable("BUILD_REPOSITORY_PROVIDER", build_Repository_Provider);
                context.Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER", system_PullRequest_PullRequestNumber);

                var sut = new DefaultBuildContext(context);

                // ACT
                var prNumber = sut.GitHub.PullRequest.Number;

                // ASSERT
                Assert.Equal(expected, prNumber);
            }

            [Theory]
            [InlineData("true", "23", 23)]
            [InlineData("false", "23", 0)]
            [InlineData("true", "", 0)]
            public void PullRequestNumber_returns_expected_value_when_building_a_GitHub_PR_on_GitHub_Actions(string? githubActionsEnvironmentVariable, string? prNumberEnvironmentVariable, int expected)
            {
                // ARRANGE
                var context = new FakeCakeContext();
                context.Environment.SetEnvironmentVariable("GITHUB_ACTIONS", githubActionsEnvironmentVariable);
                context.Environment.SetEnvironmentVariable("GITHUB_EVENT_NAME", "pull_request");

                // Note that GitHub Actions has no predefined variable that specifies the PR number for a PR build.
                // Instead, the PR_NUMBER variable needs to be defined and populated in the GitHub Actions workflow, e.g.
                //  env:
                //    PR_NUMBER: ${{ github.event.number }}
                context.Environment.SetEnvironmentVariable("PR_NUMBER", prNumberEnvironmentVariable);

                var sut = new DefaultBuildContext(context);

                // ACT
                var prNumber = sut.GitHub.PullRequest.Number;

                // ASSERT
                Assert.Equal(expected, prNumber);
            }

            [Fact]
            public void A_warning_is_emitted_if_the_Pull_Request_number_could_not_be_determined_when_building_a_GitHub_PR_on_GitHub_Actions()
            {
                // ARRANGE
                var context = new FakeCakeContext();
                context.Environment.SetEnvironmentVariable("GITHUB_ACTIONS", "true");
                context.Environment.SetEnvironmentVariable("GITHUB_EVENT_NAME", "pull_request");
                // Clear value of "PR_NUMBER"
                context.Environment.SetEnvironmentVariable("PR_NUMBER", null);


                // ACT
                _ = new DefaultBuildContext(context);

                // ASSERT
                var warning = Assert.Single(context.Log.Entries, x => x.Level == LogLevel.Warning);
                Assert.Equal(
                    "Current build seems to be a PR build but the Pull Request number could not be determined. " +
                    "Make sure the PR_NUMBER variable is set in the GitHub Actions workflow. " +
                    "This is required since GitHub Actions does not a provide a predefined variable for this.",
                    warning.Message);
            }

            [Fact]
            public void No_warning_is_emitted_if_the_Pull_Request_number_could_not_be_determined_when_not_building_a_GitHub_PR_on_GitHub_Actions()
            {
                // ARRANGE
                var context = new FakeCakeContext();
                context.Environment.SetEnvironmentVariable("GITHUB_ACTIONS", "true");
                context.Environment.SetEnvironmentVariable("GITHUB_EVENT_NAME", "not pull_request");
                // Clear value of "PR_NUMBER"
                context.Environment.SetEnvironmentVariable("PR_NUMBER", null);


                // ACT
                _ = new DefaultBuildContext(context);

                // ASSERT
                Assert.DoesNotContain(context.Log.Entries, x => x.Level == LogLevel.Warning);
            }
        }
    }
}
