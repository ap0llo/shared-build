using Cake.Core.IO;
using Grynwald.SharedBuild.Tasks;
using Grynwald.SharedBuild.Test.Mocks;
using Xunit;

namespace Grynwald.SharedBuild.Test.Tasks
{
    /// <summary>
    /// Tests for <see cref="FormatCodeTask"/>
    /// </summary>
    public class FormatCodeTaskTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Task_can_be_enabled_and_disabled_through_code_formatting_settings(bool enableAutomaticFormatting)
        {
            // ARRANGE
            var context = new FakeBuildContext();
            context.CodeFormattingSettings.EnableAutomaticFormatting = enableAutomaticFormatting;

            var sut = new FormatCodeTask();

            // ACT
            var shouldRun = sut.ShouldRun(context);

            // ASSERT
            Assert.Equal(enableAutomaticFormatting, shouldRun);
        }

        [Fact]
        public void Task_starts_dotnet_format_with_expected_parameters()
        {
            // ARRANGE
            var rootDirectory = (DirectoryPath)"X:\\repository";
            var workingDirectory = rootDirectory.Combine("directory1");
            var solutionPath = rootDirectory.CombineWithFilePath("Example.sln");

            var context = new FakeBuildContext();
            {
                context.Environment.WorkingDirectory = workingDirectory;
                context.RootDirectory = rootDirectory;
                context.SolutionPath = solutionPath;
                context.AddDotNetCli();
            }

            var sut = new FormatCodeTask();

            // ACT
            sut.Run(context);

            // ASSERT
            var startedProcess = Assert.Single(context.ProcessRunner.ProcessInvocations);
            Assert.Equal("dotnet", startedProcess.FilePath.GetFilenameWithoutExtension());
            Assert.Equal(rootDirectory, startedProcess.Settings.WorkingDirectory);
            Assert.Collection(
                startedProcess.Settings.Arguments,
                arg => Assert.Equal("format", arg.Render()),
                arg => Assert.Equal($@"""{solutionPath}""", arg.Render())
            );
        }

        [Fact]
        public void Task_passes_excluded_directories_to_dotnet_format()
        {
            // ARRANGE
            var rootDirectory = (DirectoryPath)"X:\\repository";
            var workingDirectory = rootDirectory.Combine("directory1");
            var solutionPath = rootDirectory.CombineWithFilePath("Example.sln");

            var context = new FakeBuildContext();
            {
                context.Environment.WorkingDirectory = workingDirectory;
                context.RootDirectory = rootDirectory;
                context.SolutionPath = solutionPath;
                context.AddDotNetCli();
            }

            context.CodeFormattingSettings.ExcludedDirectories = new DirectoryPath[]
            {
                "X:\\repository\\directory2",
                "directory3",
            };

            var sut = new FormatCodeTask();

            // ACT
            sut.Run(context);

            // ASSERT
            var startedProcess = Assert.Single(context.ProcessRunner.ProcessInvocations);
            Assert.Equal("dotnet", startedProcess.FilePath.GetFilenameWithoutExtension());
            Assert.Equal(rootDirectory, startedProcess.Settings.WorkingDirectory);
            Assert.Collection(
                startedProcess.Settings.Arguments,
                arg => Assert.Equal("format", arg.Render()),
                arg => Assert.Equal($@"""{solutionPath}""", arg.Render()),
                arg => Assert.Equal("--exclude", arg.Render()),
                arg => Assert.Equal("\"directory2\"", arg.Render()),
                arg => Assert.Equal("\"directory1/directory3\"", arg.Render())
            );
        }
    }
}
