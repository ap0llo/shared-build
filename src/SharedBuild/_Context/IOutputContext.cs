using System.Collections.Generic;
using Cake.Core.IO;

namespace Grynwald.SharedBuild
{
    public interface IOutputContext : IPrintableObject
    {
        /// <summary>
        /// Gets the root output directory
        /// </summary>
        DirectoryPath BinariesDirectory { get; }

        /// <summary>
        /// Gets the output path for the auto-generated change log
        /// </summary>
        FilePath ChangeLogFile { get; }

        /// <summary>
        /// Gets the output path for NuGet packages
        /// </summary>
        DirectoryPath PackagesDirectory { get; }

        /// <summary>
        /// Gets the output path for test results
        /// </summary>
        DirectoryPath TestResultsDirectory { get; }

        /// <summary>
        /// Gets all NuGet package files in the packages output directory
        /// </summary>
        IEnumerable<FilePath> PackageFiles { get; }
    }
}
