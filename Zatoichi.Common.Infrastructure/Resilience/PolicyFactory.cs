namespace Zatoichi.Common.Infrastructure.Resilience
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using Microsoft.Data.SqlClient;

    public abstract class PolicyFactory
    {
        public Func<SqlException, bool> IsTransientSqlException => ex =>
            TransientErrors.TransientSqlErrorNumbers.Contains(ex.Number);

        public Func<HttpStatusCodeException, bool> IsTransientRestRequestException => ex =>
            TransientErrors.TransientHttpStatusCodes.FirstOrDefault(a => a == (int) ex.StatusCode) > 0;

        public virtual bool IsTransientException(Exception ex)
        {
            if (ex == null)
            {
                return false;
            }

            var ex2 = ex as HttpStatusCodeException;
            if (ex2 == null || !this.IsTransientRestRequestException(ex2))
            {
                return ex is HttpRequestException;
            }

            return true;
        }

        public virtual bool IsTransientIoException(IOException ioException)
        {
            if (ioException == null)
            {
                return false;
            }

            if (!(ioException is DirectoryNotFoundException) && !(ioException is EndOfStreamException) &&
                !(ioException is FileNotFoundException) && !(ioException is FileLoadException))
            {
                return ioException is PathTooLongException;
            }

            return true;
        }
    }
}