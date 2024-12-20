namespace Grynwald.SharedBuild;

public static class TaskNames
{
    public const string CI = "CI";
    public const string Default = "Default";
    public const string Build = "Build";
    public const string Pack = "Pack";
    public const string Test = "Test";
    public const string Push = "Push";
    public const string CreateGitHubRelease = "CreateGitHubRelease";
    public const string GenerateChangeLog = "GenerateChangeLog";
    public const string PrintBuildContext = "PrintBuildContext";
    public const string SetBuildNumber = "SetBuildNumber";
    public const string FormatCode = "FormatCode";
    public const string ValidateCodeFormatting = "ValidateCodeFormatting";
    public const string Validate = "Validate";
    public const string Generate = "Generate";
    public const string SetGitHubMilestone = "SetGitHubMilestone";
}
