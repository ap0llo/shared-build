using System;
using System.Linq;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using Cake.GitHub;
using Cake.GitVersioning;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.CreateGitHubRelease)]
    [IsDependentOn(typeof(GenerateChangeLogTask))]
    public class CreateGitHubReleaseTask : AsyncFrostingTask<IBuildContext>
    {
        public override bool ShouldRun(IBuildContext context)
        {
            return context.IsRunningInCI && (context.Git.IsMasterBranch || context.Git.IsReleaseBranch);
        }

        public override async Task RunAsync(IBuildContext context)
        {
            context.Log.Information("Creating GitHub Releases");

            var versionInfo = context.GitVersioningGetVersion(context.RootDirectory.FullPath);

            var changeLog = context.FileSystem.GetFile(context.Output.ChangeLogFile).ReadAllText();

            if (context.GitHub.TryGetAccessToken() is not string accessToken)
            {
                throw new Exception("No GitHub access token specified. Cannot create a GitHub Release");
            }

            //
            // For builds on master, create a *draft* release
            //
            if (context.Git.IsMasterBranch)
            {
                await CreateDraftRelease(context, accessToken, versionInfo.NuGetPackageVersion, changeLog);
            }

            //
            // For builds on release branches, create a non-draft release
            //
            if (context.Git.IsReleaseBranch)
            {
                await CreateRelease(context, accessToken, versionInfo.NuGetPackageVersion, changeLog);
            }
        }


        private async Task CreateDraftRelease(IBuildContext context, string accessToken, string version, string changeLog)
        {
            var releaseSettings = new GitHubCreateReleaseSettings()
            {
                Draft = true,
                Prerelease = true,
                Name = $"v{version}",
                Body = changeLog,
                Overwrite = true,
                TargetCommitish = context.Git.CommitId
            };

            await context.GitHubCreateReleaseAsync(null, accessToken, context.GitHub.RepositoryOwner, context.GitHub.RepositoryName, "vNext", releaseSettings);
        }

        private async Task CreateRelease(IBuildContext context, string accessToken, string version, string changeLog)
        {
            var releaseSettings = new GitHubCreateReleaseSettings()
            {
                Name = $"v{version}",
                Body = changeLog,
                TargetCommitish = context.Git.CommitId,
                Assets = context.Output.PackageFiles.ToList()
            };

            await context.GitHubCreateReleaseAsync(null, accessToken, context.GitHub.RepositoryOwner, context.GitHub.RepositoryName, $"v{version}", releaseSettings);
        }
    }
}
