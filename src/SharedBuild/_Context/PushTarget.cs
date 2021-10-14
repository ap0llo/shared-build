using System;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild
{
    public class PushTarget : IPrintableObject
    {
        private readonly DefaultBuildContext m_Context;

        public PushTargetType Type { get; }

        public string FeedUrl { get; }

        public Func<IBuildContext, bool> IsActive { get; }


        public PushTarget(DefaultBuildContext context, PushTargetType type, string feedUrl, Func<IBuildContext, bool> isActive)
        {
            if (String.IsNullOrWhiteSpace(feedUrl))
                throw new ArgumentException("Value must not be null or whitespace", nameof(feedUrl));

            if (!Enum.IsDefined(type))
                throw new ArgumentException($"Undefined enum value '{type}'", nameof(type));

            m_Context = context ?? throw new ArgumentNullException(nameof(context));
            Type = type;
            FeedUrl = feedUrl;
            IsActive = isActive ?? throw new ArgumentNullException(nameof(isActive));
        }


        public void PrintToLog(ICakeLog log)
        {
            log.Information($"{nameof(Type)}: {Type}");
            log.Information($"{nameof(FeedUrl)}: {FeedUrl}");
            log.Information($"{nameof(IsActive)}: {IsActive(m_Context)}");
        }
    }
}
