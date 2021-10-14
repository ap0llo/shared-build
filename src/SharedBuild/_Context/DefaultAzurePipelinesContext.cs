using System;
using Cake.Common.Build;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild
{
    public class DefaultAzurePipelinesContext : IAzurePipelinesContext
    {
        private readonly DefaultBuildContext m_Context;


        /// <inheritdoc />
        public IAzurePipelinesArtifactNames ArtifactNames { get; }

        /// <inheritdoc />
        public bool IsActive =>
            m_Context.AzurePipelines().IsRunningOnAzurePipelines ||
            m_Context.AzurePipelines().IsRunningOnAzurePipelinesHosted;


        public DefaultAzurePipelinesContext(DefaultBuildContext context)
        {
            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            ArtifactNames = new DefaultAzurePipelinesArtifactNames(context);
        }


        /// <inheritdoc />
        public void PrintToLog(int indentWidth = 0)
        {
            string prefix = new String(' ', indentWidth);

            m_Context.Log.Information($"{prefix}{nameof(IsActive)}: {IsActive}");

            m_Context.Log.Information($"{prefix}{nameof(ArtifactNames)}:");
            ArtifactNames.PrintToLog(indentWidth + 2);
        }
    }
}
