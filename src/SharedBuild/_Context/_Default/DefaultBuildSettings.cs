using System;
using Cake.Common;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild;

public class DefaultBuildSettings : IBuildSettings
{
    private readonly DefaultBuildContext m_Context;


    /// <inheritdoc />
    public virtual string Configuration => m_Context.Argument("configuration", "Release");

    /// <summary>
    /// Determines whether to use deterministic build settings
    /// </summary>
    /// <remarks>
    /// Determinisitc build can be enabled/disabled using the --deterministic command line switch.
    /// Wehn running in the CI environment, it is enabled by default.
    /// In other environments it is disabled by default.
    /// </remarks>
    public virtual bool Deterministic => m_Context.Argument("deterministic", m_Context.IsRunningInCI);



    public DefaultBuildSettings(DefaultBuildContext context)
    {
        m_Context = context ?? throw new ArgumentNullException(nameof(context));
    }


    /// <inheritdoc />
    public virtual DotNetMSBuildSettings GetDefaultMSBuildSettings() => new()
    {
        TreatAllWarningsAs = MSBuildTreatAllWarningsAs.Error
    };

    /// <inheritdoc />
    public virtual void PrintToLog(ICakeLog log)
    {
        log.Information($"{nameof(Configuration)}: {Configuration}");
        log.Information($"{nameof(Deterministic)}: {Deterministic}");
    }
}
