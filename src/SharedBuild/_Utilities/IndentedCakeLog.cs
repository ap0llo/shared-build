using System;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild
{
    public class IndentedCakeLog : ICakeLog
    {
        private readonly ICakeLog m_InnerLog;


        public Verbosity Verbosity
        {
            get => m_InnerLog.Verbosity;
            set => m_InnerLog.Verbosity = value;
        }


        public IndentedCakeLog(ICakeLog innerLog)
        {
            m_InnerLog = innerLog ?? throw new ArgumentNullException(nameof(innerLog));
        }


        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args) =>
            m_InnerLog.Write(verbosity, level, String.Format($"  {format}", args));
    }
}
