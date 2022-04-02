using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grynwald.SharedBuild.Tasks;
using Grynwald.SharedBuild.Test.Mocks;
using Xunit;

namespace Grynwald.SharedBuild.Test.Tasks
{
    /// <summary>
    /// Tests for <see cref="ValidateCodeFormattingTask"/>
    /// </summary>
    public class ValidateCodeFormattingTaskTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Task_can_be_enabled_and_disabled_through_code_formatting_settings(bool enableAutomaticFormatting)
        {
            // ARRANGE
            var context = new FakeBuildContext();
            context.CodeFormattingSettings.EnableAutomaticFormatting = enableAutomaticFormatting;

            var sut = new ValidateCodeFormattingTask();

            // ACT
            var shouldRun = sut.ShouldRun(context);

            // ASSERT
            Assert.Equal(enableAutomaticFormatting, shouldRun);
        }
    }
}
