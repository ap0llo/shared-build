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
            public void IsPullRequest_returns_expected_value_when_building_a_GitHub_PR_on_Azure_Pipelines(string? tfBuild, string? system_PullRequest_PullRequestId, string? build_Repository_Provider, bool expected)
            {
                // ARRANGE
                var context = new FakeCakeContext();
                context.Environment.SetEnvironmentVariable("TF_BUILD", tfBuild);
                context.Environment.SetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID", system_PullRequest_PullRequestId);
                context.Environment.SetEnvironmentVariable("BUILD_REPOSITORY_PROVIDER", build_Repository_Provider);

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
            public void PullRequestNumber_returns_expected_value(string? tfBuild, string? system_PullRequest_PullRequestId, string? build_Repository_Provider, string? system_PullRequest_PullRequestNumber, int expected)
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
        }
    }
}
