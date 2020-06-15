namespace Zatoichi.Common.Infrastructure.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TodoAttribute : Attribute
    {
        public string Message { get; set; }
        public string DateSet { get; set; }
    }
}