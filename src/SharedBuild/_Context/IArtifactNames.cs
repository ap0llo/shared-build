namespace Grynwald.SharedBuild;

public interface IArtifactNames : IPrintableObject
{
    string Binaries { get; }

    string ChangeLog { get; }

    string TestResults { get; }
}
