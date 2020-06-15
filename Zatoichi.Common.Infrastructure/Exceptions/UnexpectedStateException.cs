namespace Zatoichi.Common.Infrastructure.Exceptions
{
    using System;

    [Serializable]
    public class UnexpectedStateException : ApplicationException
    {
        public UnexpectedStateException(string message)
            : this(message, null)
        {
        }

        public UnexpectedStateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}