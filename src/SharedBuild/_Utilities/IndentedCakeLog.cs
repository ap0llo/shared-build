using System;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild;

public class IndentedCakeLog(ICakeLog innerLog) : ICakeLog
{
    public Verbosity Verbosity
    {
        get => innerLog.Verbosity;
        set => innerLog.Verbosity = value;
    }

    public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args) =>
        innerLog.Write(verbosity, level, String.Format($"  {format}", args));
}
