namespace Grynwald.SharedBuild;

public interface ITestSettings : IPrintableObject
{
    /// <summary>
    /// Gets whether to collect coverage data when running tests
    /// </summary>
    bool CollectCodeCoverage { get; }
}
