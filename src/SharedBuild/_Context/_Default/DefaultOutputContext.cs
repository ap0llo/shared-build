using System;
using System.Collections.Generic;
using Cake.Common;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Grynwald.SharedBuild;

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
    public virtual DirectoryPath CodeCoverageReportDirectory => BinariesDirectory.Combine(m_Context.BuildSettings.Configuration).Combine("CodeCoverage").Combine("Report");

    /// <inheritdoc />
    public virtual DirectoryPath CodeCoverageHistoryDirectory => BinariesDirectory.Combine(m_Context.BuildSettings.Configuration).Combine("CodeCoverage").Combine("History");

    /// <inheritdoc />
    public virtual FilePath ChangeLogFile => BinariesDirectory.CombineWithFilePath("changelog.md");

    /// <inheritdoc />
    public virtual IEnumerable<FilePath> PackageFiles => m_Context.FileSystem.GetFilePaths(PackagesDirectory, "*.nupkg");


    public DefaultOutputContext(DefaultBuildContext context)
    {
        m_Context = context ?? throw new ArgumentNullException(nameof(context));
    }


    /// <inheritdoc />
    public virtual void PrintToLog(ICakeLog log)
    {
        log.Information($"{nameof(BinariesDirectory)}: {BinariesDirectory.FullPath}");
        log.Information($"{nameof(PackagesDirectory)}: {PackagesDirectory.FullPath}");
        log.Information($"{nameof(TestResultsDirectory)}: {TestResultsDirectory.FullPath}");
        log.Information($"{nameof(CodeCoverageReportDirectory)}: {CodeCoverageReportDirectory.FullPath}");
        log.Information($"{nameof(CodeCoverageHistoryDirectory)}: {CodeCoverageHistoryDirectory.FullPath}");
        log.Information($"{nameof(ChangeLogFile)}: {ChangeLogFile.FullPath}");
    }
}
