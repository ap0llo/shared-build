using Cake.Common;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild;

public class DefaultTestSettings(DefaultBuildContext context) : ITestSettings
{
    public virtual bool CollectCodeCoverage { get; } = context.Argument("collect-code-coverage", true);

    public void PrintToLog(ICakeLog log)
    {
        log.Information($"{nameof(CollectCodeCoverage)}: {CollectCodeCoverage}");
    }
}
