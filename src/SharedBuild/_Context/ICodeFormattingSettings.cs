using Grynwald.SharedBuild.Tasks;

namespace Grynwald.SharedBuild
{
    public interface ICodeFormattingSettings : IPrintableObject
    {
        /// <summary>
        /// Enables/disables automatic formatting (see <see cref="FormatCodeTask"/>) and validation of code formatting (see <see cref="ValidateCodeFormattingTask"/>).
        /// </summary>
        bool EnableAutomaticFormatting { get; }
    }
}
