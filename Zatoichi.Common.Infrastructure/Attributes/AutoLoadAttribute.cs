namespace Zatoichi.Common.Infrastructure.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoLoadAttribute : Attribute
    {
        public Type ServiceType { get; set; }
        public LifestyleType LifeStyle { get; set; } = LifestyleType.Transient;
    }

    public enum LifestyleType
    {
        Singleton, Transient, Scoped
    }
}