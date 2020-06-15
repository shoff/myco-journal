namespace Zatoichi.Common.Infrastructure.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class DependentAssemblyAttribute : Attribute
    {
    }
}