namespace Zatoichi.Common.Infrastructure.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///     Used to decorate any class, method, field, property event, etc
    ///     that the existence in the code base needs to be validated/justified.
    /// </summary>
    /// <remarks>
    ///     This will allow us to run meta reports against the code to see where we might
    ///     be letting cruft drift in. We REALLY want to avoid the mess we made in AUSKF!
    /// </remarks>
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SuspectTypeAttribute : Attribute
    {
        public string Reason { get; set; }
        public string Developer { get; set; }
    }
}