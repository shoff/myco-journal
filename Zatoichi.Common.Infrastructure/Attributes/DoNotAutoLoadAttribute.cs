namespace Zatoichi.Common.Infrastructure.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DoNotAutoLoadAttribute : Attribute
    {
    }
}