using System.Threading.Tasks;
using Cake.Common.Build;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using Cake.GitVersioning;
using Grynwald.SharedBuild.Tools.ChangeLog;

namespace Grynwald.SharedBuild.Tasks;

[TaskName(TaskNames.GenerateChangeLog)]
public class GenerateChangeLogTask : AsyncFrostingTask<IBuildContext>
{
    public override async Task RunAsync(IBuildContext context)
    {
        //
        // Generate change log
        //
        context.Log.Information("Generating changelog");
        var versionInfo = context.GitVersioningGetVersion(context.RootDirectory.FullPath);
        var version = versionInfo.NuGetPackageVersion;

        var changelogSettings = new ChangeLogSettings()
        {
            RepositoryPath = context.RootDirectory,
            CurrentVersion = version,
            VersionRange = $"[{version}]",
            OutputPath = context.Output.ChangeLogFile,
            Template = ChangeLogTemplate.GitHubRelease
        };

        if (context.GitHub.TryGetAccessToken() is string accessToken)
        {
            context.Log.Information("GitHub access token specified, activating changelog's GitHub integration");
            changelogSettings.IntegrationProvider = ChangeLogIntegrationProvider.GitHub;
            changelogSettings.EnvironmentVariables["CHANGELOG__INTEGRATIONS__GITHUB__ACCESSTOKEN"] = accessToken;
        }
        else
        {
            context.Log.Warning("No GitHub access token specified, generating change log without GitHub integration");
        }

        context.ChangeLog(changelogSettings);

        //
        // Publish changelog as pipeline artifact
        //
        if (context.AzurePipelines.IsActive)
        {
            context.Log.Information("Publishing change log to Azure Pipelines");
            context.AzurePipelines.Commands.UploadArtifact("", context.Output.ChangeLogFile, context.AzurePipelines.ArtifactNames.ChangeLog);
        }
        else if (context.GitHubActions.IsActive)
        {
            context.Log.Information("Publishing change log to GitHub Actions artifacts");
            await context.GitHubActions().Commands.UploadArtifact(context.Output.ChangeLogFile, context.GitHubActions.ArtifactNames.ChangeLog);
        }
    }
}
