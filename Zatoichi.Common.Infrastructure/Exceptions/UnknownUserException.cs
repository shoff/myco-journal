namespace Zatoichi.Common.Infrastructure.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UnknownUserException : AuthenticationException
    {
        public UnknownUserException(string message)
            : base(message)
        {
        }
    }
}