namespace Zatoichi.Common.Infrastructure.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class ScopedAttribute : Attribute
    {
        public Type ServiceType { get; set; }
    }
}