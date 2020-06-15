namespace Zatoichi.Common.Infrastructure.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ExternalAuthenticationException : AuthenticationException
    {
        public ExternalAuthenticationException(string message)
            : base(message)
        {
        }
    }
}