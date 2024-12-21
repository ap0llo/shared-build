using System.Runtime.InteropServices;
using Grynwald.SharedBuild.Test.Mocks;
using Xunit;

namespace Grynwald.SharedBuild.Test;

/// <summary>
/// Tests for <see cref="DefaultGitHubPullRequestContext"/>
/// </summary>
public partial class DefaultBuildContextTest
{
    public class GitHubActions
    {
        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("false", false)]
        [InlineData("true", true)]
        public void IsActive_returns_expected_value(string? githubActionsEnvironmentVariable, bool expected)
        {
            // ARRANGE
            var context = new FakeCakeContext();
            context.Environment.SetEnvironmentVariable("GITHUB_ACTIONS", githubActionsEnvironmentVariable);

            var sut = new DefaultBuildContext(context);

            // ACT
            var actual = sut.GitHubActions.IsActive;

            // ASSERT
            Assert.Equal(expected, actual);
        }
    }
}
