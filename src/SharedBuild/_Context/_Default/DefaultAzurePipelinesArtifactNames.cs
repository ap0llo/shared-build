using System;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild
{
    public class DefaultAzurePipelinesArtifactNames : IAzurePipelinesArtifactNames
    {
        private readonly DefaultBuildContext m_Context;


        /// <summary>
        /// The name of the main artifact
        /// </summary>
        public string Binaries => "Binaries";

        /// <summary>
        /// The artifact name under which to save test result files
        /// </summary>
        public string TestResults => "TestResults";

        /// <summary>
        /// The artifact name for the auto-generated change log.
        /// </summary>
        public string ChangeLog => "ChangeLog";


        public DefaultAzurePipelinesArtifactNames(DefaultBuildContext context)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public void PrintToLog(int indentWidth = 0)
        {
            string prefix = new String(' ', indentWidth);

            m_Context.Log.Information($"{prefix}{nameof(Binaries)}: {Binaries}");
            m_Context.Log.Information($"{prefix}{nameof(TestResults)}: {TestResults}");
            m_Context.Log.Information($"{prefix}{nameof(ChangeLog)}: {ChangeLog}");
        }
    }
}
