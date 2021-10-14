using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild
{
    /// <summary>
    /// Interface for an object which's state can be written to the log
    /// </summary>
    public interface IPrintableObject
    {
        /// <summary>
        /// Prints the context's data to the log
        /// </summary>
        void PrintToLog(ICakeLog log);
    }
}
