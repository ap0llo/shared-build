using System;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild;

public class DefaultAzurePipelinesArtifactNames : IAzurePipelinesArtifactNames
{
    /// <summary>
    /// The name of the main artifact
    /// </summary>
    public virtual string Binaries => "Binaries";

    /// <summary>
    /// The artifact name under which to save test result files
    /// </summary>
    public virtual string TestResults => "TestResults";

    /// <summary>
    /// The artifact name for the auto-generated change log.
    /// </summary>
    public virtual string ChangeLog => "ChangeLog";


    /// <inheritdoc />
    public virtual void PrintToLog(ICakeLog log)
    {
        log.Information($"{nameof(Binaries)}: {Binaries}");
        log.Information($"{nameof(TestResults)}: {TestResults}");
        log.Information($"{nameof(ChangeLog)}: {ChangeLog}");
    }
}
