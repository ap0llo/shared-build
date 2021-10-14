using System;
using System.Collections.Generic;
using Cake.Common;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Grynwald.SharedBuild
{
    public class DefaultOutputContext : IOutputContext
    {
        private readonly DefaultBuildContext m_Context;


        /// <inheritdoc />
        public virtual DirectoryPath BinariesDirectory
        {
            get
            {
                var binariesDirectory = m_Context.EnvironmentVariable("BUILD_BINARIESDIRECTORY");
                return String.IsNullOrEmpty(binariesDirectory) ? m_Context.RootDirectory.Combine("Binaries") : binariesDirectory;
            }
        }

        /// <inheritdoc />
        public virtual DirectoryPath PackagesDirectory => BinariesDirectory.Combine(m_Context.BuildSettings.Configuration).Combine("packages");

        /// <inheritdoc />
        public virtual DirectoryPath TestResultsDirectory => BinariesDirectory.Combine(m_Context.BuildSettings.Configuration).Combine("TestResults");

        /// <inheritdoc />
        public virtual FilePath ChangeLogFile => BinariesDirectory.CombineWithFilePath("changelog.md");

        /// <inheritdoc />
        public virtual IEnumerable<FilePath> PackageFiles => m_Context.FileSystem.GetFilePaths(PackagesDirectory, "*.nupkg");


        public DefaultOutputContext(DefaultBuildContext context)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
        }


        /// <inheritdoc />
        public virtual void PrintToLog(int indentWidth = 0)
        {
            string prefix = new String(' ', indentWidth);

            m_Context.Log.Information($"{prefix}{nameof(BinariesDirectory)}: {BinariesDirectory.FullPath}");
            m_Context.Log.Information($"{prefix}{nameof(PackagesDirectory)}: {PackagesDirectory.FullPath}");
            m_Context.Log.Information($"{prefix}{nameof(TestResultsDirectory)}: {TestResultsDirectory.FullPath}");
            m_Context.Log.Information($"{prefix}{nameof(ChangeLogFile)}: {ChangeLogFile.FullPath}");
        }
    }
}
