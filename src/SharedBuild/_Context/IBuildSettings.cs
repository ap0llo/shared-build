using Cake.Common.Tools.DotNetCore.MSBuild;

namespace Grynwald.SharedBuild
{
    public interface IBuildSettings
    {
        /// <summary>
        /// Gets the configuration to build (Debug/Relesae)
        /// </summary>
        string Configuration { get; }

        /// <summary>
        /// Determines whether to use deterministic build settings
        /// </summary>
        bool Deterministic { get; }


        /// <summary>
        /// Gets the default settings to use when calling MSBuild
        /// </summary>
        /// <returns></returns>
        DotNetCoreMSBuildSettings GetDefaultMSBuildSettings();

        /// <summary>
        /// Prints the context's data to the log
        /// </summary>
        void PrintToLog(int indentWidth = 0);
    }
}
