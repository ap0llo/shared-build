using System.Runtime.InteropServices;
using Grynwald.SharedBuild.Test.Mocks;
using Xunit;

namespace Grynwald.SharedBuild.Test;

/// <summary>
/// Tests for <see cref="DefaultGitHubPullRequestContext"/>
/// </summary>
public partial class DefaultBuildContextTest
{
    [Theory]
    [InlineData("", "", false)]
    [InlineData(null, null, false)]
    [InlineData("", "false", false)]
    [InlineData("true", "true", true)]
    [InlineData("true", "false", true)]
    [InlineData("false", "true", true)]
    public void IsRunningInCI_returns_true_when_build_is_running_in_either_Azure_Pipelines_or_GitHub_Actions(string? tfBuildEnvironmentVariable, string? githubActionsEnvironmentVariable, bool expected)
    {
        // ARRANGE
        var context = new FakeCakeContext();
        context.Environment.SetEnvironmentVariable("GITHUB_ACTIONS", githubActionsEnvironmentVariable);
        context.Environment.SetEnvironmentVariable("TF_BUILD", tfBuildEnvironmentVariable);

        var sut = new DefaultBuildContext(context);

        // ACT
        var actual = sut.IsRunningInCI;

        // ASSERT
        Assert.Equal(expected, actual);
    }
}
