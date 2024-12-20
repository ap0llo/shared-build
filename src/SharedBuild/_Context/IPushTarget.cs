namespace Grynwald.SharedBuild;

public interface IPushTarget : IPrintableObject
{
    /// <summary>
    /// Gets the type of NuGet feed to push to
    /// </summary>
    PushTargetType Type { get; }

    /// <summary>
    /// Gets the url of the NuGet feed to push to
    /// </summary>
    string FeedUrl { get; }

    /// <summary>
    /// Determines whtther the push target is active.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    bool IsActive(IBuildContext context);

}
