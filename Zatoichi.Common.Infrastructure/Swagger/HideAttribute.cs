namespace Zatoichi.Common.Infrastructure.Swagger
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class HideAttribute : Attribute
    {
    }
}