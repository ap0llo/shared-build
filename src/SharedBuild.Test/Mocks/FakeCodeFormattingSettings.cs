using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Grynwald.SharedBuild.Test.Mocks;

/// <summary>
/// Mock of <see cref="ICodeFormattingSettings"/>
/// </summary>
internal class FakeCodeFormattingSettings : ICodeFormattingSettings
{
    /// <inheritdoc />
    public bool EnableAutomaticFormatting { get; set; }

    /// <inheritdoc />
    public ICollection<DirectoryPath> ExcludedDirectories { get; set; } = [];


    public void PrintToLog(ICakeLog log) => throw new NotImplementedException();
}
