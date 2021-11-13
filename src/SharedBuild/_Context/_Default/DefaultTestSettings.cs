using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Common;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild
{
    public class DefaultTestSettings : ITestSettings
    {
        public virtual bool CollectCodeCoverage { get; }


        public DefaultTestSettings(DefaultBuildContext context)
        {
            CollectCodeCoverage = context.Argument("collect-code-coverage", true);
        }


        public void PrintToLog(ICakeLog log)
        {
            log.Information($"{nameof(CollectCodeCoverage)}: {CollectCodeCoverage}");
        }
    }
}
