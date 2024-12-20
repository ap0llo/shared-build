namespace Grynwald.SharedBuild;

public interface IAzurePipelinesArtifactNames : IPrintableObject
{
    string Binaries { get; }

    string ChangeLog { get; }

    string TestResults { get; }
}
