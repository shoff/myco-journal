namespace Zatoichi.Common.Infrastructure.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    {
        public Type ServiceType { get; set; }
    }
}