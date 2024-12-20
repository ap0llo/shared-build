using System;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild;

public class PushTarget : IPushTarget
{
    private readonly Func<IBuildContext, bool> m_IsActive;

    /// <inheritdoc />
    public virtual PushTargetType Type { get; }

    /// <inheritdoc />
    public virtual string FeedUrl { get; }


    public PushTarget(PushTargetType type, string feedUrl, Func<IBuildContext, bool> isActive)
    {
        if (String.IsNullOrWhiteSpace(feedUrl))
            throw new ArgumentException("Value must not be null or whitespace", nameof(feedUrl));

        if (!Enum.IsDefined(type))
            throw new ArgumentException($"Undefined enum value '{type}'", nameof(type));

        Type = type;
        FeedUrl = feedUrl;
        m_IsActive = isActive ?? throw new ArgumentNullException(nameof(isActive));
    }


    /// <inheritdoc />
    public virtual bool IsActive(IBuildContext context) => m_IsActive(context);


    public void PrintToLog(ICakeLog log)
    {
        log.Information($"{nameof(Type)}: {Type}");
        log.Information($"{nameof(FeedUrl)}: {FeedUrl}");
    }
}
