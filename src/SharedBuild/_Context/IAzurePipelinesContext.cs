namespace Grynwald.SharedBuild
{
    public interface IAzurePipelinesContext
    {
        /// <summary>
        /// Gets the names to use for publishing pipeline artifacts
        /// </summary>
        IAzurePipelinesArtifactNames ArtifactNames { get; }

        /// <summary>
        /// Gets whether the build is currently running on Azure Pipelines
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Prints the context's data to the log
        /// </summary>
        void PrintToLog(int indentWidth = 0);
    }
}
