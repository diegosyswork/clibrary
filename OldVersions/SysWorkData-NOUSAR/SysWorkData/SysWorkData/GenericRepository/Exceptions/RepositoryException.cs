using System;
using System.Data;

namespace SysWork.Data.GenericRepository.Exceptions
{
    /// <summary>
    /// Exceptions Wrapper, allows to capture the original exception, in case there is a DbCommand,
    /// content of the same and the original StackTrace.
    /// </summary>
    public class RepositoryException : Exception
    {
        /// <summary>
        /// Gets the original exception.
        /// </summary>
        /// <value>
        /// The original exception.
        /// </value>
        public Exception OriginalException { get; private set; }

        /// <summary>
        /// Gets the database command.
        /// </summary>
        /// <value>
        /// The database command.
        /// </value>
        public IDbCommand DbCommand { get; private set; }

        /// <summary>
        /// Gets the original stack trace.
        /// </summary>
        /// <value>
        /// The original stack trace.
        /// </value>
        public string OriginalStackTrace { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        /// <param name="originalException">The original exception.</param>
        /// <param name="dbCommand">The database command.</param>
        public RepositoryException(Exception originalException, IDbCommand dbCommand) : base(originalException.Message)
        {
            this.OriginalException = originalException;
            this.DbCommand = dbCommand;
            this.OriginalStackTrace = originalException.StackTrace;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        /// <param name="originalException">The original exception.</param>
        public RepositoryException(Exception originalException) : base(originalException.Message)
        {
            this.OriginalException = originalException;
            this.DbCommand = null;
            this.OriginalStackTrace = originalException.StackTrace;
        }
    }
}
