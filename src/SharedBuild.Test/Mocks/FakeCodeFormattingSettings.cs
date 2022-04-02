using System;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild.Test.Mocks
{
    /// <summary>
    /// Mock of <see cref="ICodeFormattingSettings"/>
    /// </summary>
    internal class FakeCodeFormattingSettings : ICodeFormattingSettings
    {
        public bool EnableAutomaticFormatting { get; set; }


        public void PrintToLog(ICakeLog log)
        {
            throw new NotImplementedException();
        }
    }
}
