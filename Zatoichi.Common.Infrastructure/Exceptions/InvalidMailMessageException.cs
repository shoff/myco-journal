namespace Zatoichi.Common.Infrastructure.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class InvalidMailMessageException : ApplicationException
    {
        public InvalidMailMessageException(string message)
            : base(message)
        {
        }
    }
}