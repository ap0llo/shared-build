using System;
using Cake.Core.Diagnostics;

namespace Grynwald.SharedBuild
{
    public class PushTarget
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


        public void PrintToLog(int indentWidth = 0)
        {
            string prefix = new String(' ', indentWidth);

            m_Context.Log.Information($"{prefix}{nameof(Type)}: {Type}");
            m_Context.Log.Information($"{prefix}{nameof(FeedUrl)}: {FeedUrl}");
            m_Context.Log.Information($"{prefix}{nameof(IsActive)}: {IsActive(m_Context)}");
        }
    }
}
