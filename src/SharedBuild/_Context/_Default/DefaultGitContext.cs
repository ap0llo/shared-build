using System;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Grynwald.SharedBuild
{
    public class DefaultGitContext : IGitContext
    {
        private readonly DefaultBuildContext m_Context;


        /// <inheritdoc />
        public virtual string BranchName
        {
            get
            {
                if (m_Context.AzurePipelines.IsActive)
                {
                    var branchName = m_Context.AzurePipelines.Environment.Repository.SourceBranch;

                    if (branchName.StartsWith("refs/heads/"))
                    {
                        branchName = branchName["refs/heads/".Length..];
                    }

                    return branchName;
                }
                else
                {
                    return StartGit("rev-parse", "--abbrev-ref", "HEAD").Trim();
                }

            }
        }

        /// <inheritdoc />
        public virtual string CommitId => m_Context.AzurePipelines.IsActive
            ? m_Context.AzurePipelines.Environment.Repository.SourceVersion
            : StartGit("rev-parse", "HEAD").Trim();

        /// <inheritdoc />
        public virtual string RemoteUrl => StartGit("remote", "get-url", "origin").Trim();

        /// <inheritdoc />
        public virtual bool IsMainBranch => BranchName.Equals("main", StringComparison.OrdinalIgnoreCase) || BranchName.Equals("master", StringComparison.OrdinalIgnoreCase);

        /// <inheritdoc />
        public virtual bool IsReleaseBranch => BranchName.StartsWith("release/", StringComparison.OrdinalIgnoreCase);


        public DefaultGitContext(DefaultBuildContext context)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));

        }


        /// <inheritdoc />
        public virtual void PrintToLog(ICakeLog log)
        {
            log.Information($"{nameof(BranchName)}: {BranchName}");
            log.Information($"{nameof(CommitId)}: {CommitId}");
            log.Information($"{nameof(RemoteUrl)}: {RemoteUrl}");
            log.Information($"{nameof(IsMainBranch)}: {IsMainBranch}");
            log.Information($"{nameof(IsReleaseBranch)}: {IsReleaseBranch}");
        }


        private string StartGit(params string[] args)
        {
            var process = m_Context.ProcessRunner.Start("git", new ProcessSettings()
            {
                Arguments = ProcessArgumentBuilder.FromStrings(args),
                RedirectStandardOutput = true
            });

            process.WaitForExit();

            var exitCode = process.GetExitCode();
            if (exitCode != 0)
                throw new Exception($"Command 'git {String.Join(" ", args)}' completed with exit code {exitCode}");

            var stdOut = String.Join(Environment.NewLine, process.GetStandardOutput());
            return stdOut;
        }
    }
}
