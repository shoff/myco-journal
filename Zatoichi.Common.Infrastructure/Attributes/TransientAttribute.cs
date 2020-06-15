namespace Zatoichi.Common.Infrastructure.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class TransientAttribute : Attribute
    {
        public Type ServiceType { get; set; }
    }
}