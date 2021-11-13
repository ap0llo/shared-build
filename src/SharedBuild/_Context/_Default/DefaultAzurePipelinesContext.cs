using Cake.Common.Build;
using Cake.Common.Build.AzurePipelines;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild
{
    public class DefaultAzurePipelinesContext : IAzurePipelinesContext
    {
        private readonly IAzurePipelinesProvider m_AzurePipelinesProvider;


        /// <inheritdoc />
        public virtual IAzurePipelinesArtifactNames ArtifactNames { get; }

        /// <inheritdoc />
        public virtual bool IsActive => IsRunningOnAzurePipelines || IsRunningOnAzurePipelinesHosted;

        /// <inheritdoc />
        public virtual bool IsRunningOnAzurePipelines => m_AzurePipelinesProvider.IsRunningOnAzurePipelines;

        /// <inheritdoc />
        public virtual bool IsRunningOnAzurePipelinesHosted => m_AzurePipelinesProvider.IsRunningOnAzurePipelinesHosted;

        /// <inheritdoc />
        public virtual AzurePipelinesEnvironmentInfo Environment => m_AzurePipelinesProvider.Environment;

        /// <inheritdoc />
        public virtual IAzurePipelinesCommands Commands => m_AzurePipelinesProvider.Commands;


        public DefaultAzurePipelinesContext(DefaultBuildContext context)
        {
            ArtifactNames = new DefaultAzurePipelinesArtifactNames();
            m_AzurePipelinesProvider = context.AzurePipelines();
        }


        /// <inheritdoc />
        public virtual void PrintToLog(ICakeLog log)
        {
            log.Information($"{nameof(IsActive)}: {IsActive}");

            log.Information($"{nameof(ArtifactNames)}:");
            ArtifactNames.PrintToLog(new IndentedCakeLog(log));
        }
    }
}
