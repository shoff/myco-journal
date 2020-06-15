namespace Zatoichi.Common.Infrastructure.Extensions.Core
{
    using System;
    using ChaosMonkey.Guards;

    public static class EventExtensions
    {
        public static void Raise(this EventHandler handler, object sender, EventArgs args)
        {
            Guard.IsNotNull(sender, "sender");
            Guard.IsNotNull(args, "args");
            handler?.Invoke(sender, args);
        }

        public static void Raise<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            Guard.IsNotNull(sender, "sender");
            Guard.IsNotNull(args, "args");
            handler?.Invoke(sender, args);
        }

        public static bool IsNull(this EventHandler handler)
        {
            return handler == null;
        }

        public static bool IsNull<T>(this EventHandler<T> handler) where T : EventArgs
        {
            return handler == null;
        }
    }
}