namespace Zatoichi.Common.Infrastructure.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class AuthenticationException : ApplicationException
    {
        public AuthenticationException(string message)
            : base(message)
        {
        }
    }
}