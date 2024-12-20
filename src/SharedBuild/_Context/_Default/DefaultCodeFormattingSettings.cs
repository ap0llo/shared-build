using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Grynwald.SharedBuild;

public class DefaultCodeFormattingSettings : ICodeFormattingSettings
{
    /// <inheritdoc />
    public virtual bool EnableAutomaticFormatting => true;

    /// <inheritdoc />
    public ICollection<DirectoryPath> ExcludedDirectories { get; set; } = new List<DirectoryPath>();


    public void PrintToLog(ICakeLog log)
    {
        var indentedLog = new IndentedCakeLog(log);

        log.Information($"{nameof(EnableAutomaticFormatting)}: {EnableAutomaticFormatting}");

        log.Information($"{nameof(ExcludedDirectories)}:");
        var index = 0;
        foreach (var path in ExcludedDirectories ?? Array.Empty<DirectoryPath>())
        {
            indentedLog.Information($"{nameof(ExcludedDirectories)}[{index}]: {path}");
            index++;
        }
    }
}
