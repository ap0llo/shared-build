﻿using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace Grynwald.SharedBuild.Tasks
{
    [TaskName(TaskNames.Build)]
    public class BuildTask : FrostingTask<IBuildContext>
    {
        public override void Run(IBuildContext context)
        {
            //
            // Restore NuGet Packages
            //
            context.Log.Information("Restoring NuGet Packages");
            context.DotNetCoreRestore(context.SolutionPath.FullPath, new DotNetCoreRestoreSettings()
            {
                MSBuildSettings = context.BuildSettings.GetDefaultMSBuildSettings()
            });

            //
            // Build
            //
            context.Log.Information($"Building {context.SolutionPath}");
            var buildSettings = new DotNetCoreBuildSettings()
            {
                Configuration = context.BuildSettings.Configuration,
                NoRestore = true,
                MSBuildSettings = context.BuildSettings.GetDefaultMSBuildSettings()
            };

            if (context.BuildSettings.Deterministic)
            {
                context.Log.Information("Using deterministic build settings");
                buildSettings.MSBuildSettings.WithProperty("ContinuousIntegrationBuild", "true");
                buildSettings.MSBuildSettings.WithProperty("Deterministic", "true");
            }

            context.DotNetCoreBuild(context.SolutionPath.FullPath, buildSettings);
        }
    }
}