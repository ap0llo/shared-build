﻿namespace Grynwald.SharedBuild
{
    public interface IGitHubContext
    {
        /// <summary>
        /// Gets the host name of the GitHub server
        /// </summary>
        string HostName { get; }

        /// <summary>
        /// Gets the name of the repository owner (user or group) on GitHub
        /// </summary>
        string RepositoryOwner { get; }

        /// <summary>
        /// Gets the name of the repository on Github
        /// </summary>
        string RepositoryName { get; }

        /// <summary>
        /// Tries to get the GitHub Access token for the current builds
        /// </summary>
        string? TryGetAccessToken();

        /// <summary>
        /// Prints the context's data to the log
        /// </summary>
        void PrintToLog(int indentWidth = 0);
    }
}
