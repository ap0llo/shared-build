namespace Grynwald.SharedBuild
{
    public interface IAzurePipelinesArtifactNames
    {
        string Binaries { get; }

        string ChangeLog { get; }

        string TestResults { get; }

        /// <summary>
        /// Prints the context's data to the log
        /// </summary>
        void PrintToLog(int indentWidth = 0);
    }
}
