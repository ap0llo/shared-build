using System.Collections.Generic;
using Cake.Core.IO;
using Grynwald.SharedBuild.Tasks;

namespace Grynwald.SharedBuild;

public interface ICodeFormattingSettings : IPrintableObject
{
    /// <summary>
    /// Enables/disables automatic formatting (see <see cref="FormatCodeTask"/>) and validation of code formatting (see <see cref="ValidateCodeFormattingTask"/>).
    /// </summary>
    bool EnableAutomaticFormatting { get; }

    /// <summary>
    /// Optional list of directories to exclude from code formatting.
    /// </summary>
    ICollection<DirectoryPath> ExcludedDirectories { get; set; }
}
