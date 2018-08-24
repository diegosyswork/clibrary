using System;
using System.Data;

namespace SysWork.Data.DaoModel.Exceptions
{
    /// <summary>
    /// 
    /// Wrapper de excepciones, permite capturar la excepcion original, en caso que haya un DbCommand,
    /// el contenido del mismo y el StackTrace original.
    /// 
    /// </summary>
    public class DaoModelException : Exception
    {
        public Exception OriginalException { get; private set; }
        public IDbCommand DbCommand { get; private set; }
        public string OriginalStackTrace { get; private set; }

        public DaoModelException(Exception originalException, IDbCommand dbCommand) : base(originalException.Message)
        {
            this.OriginalException = originalException;
            this.DbCommand = dbCommand;
            this.OriginalStackTrace = originalException.StackTrace;
        }
        public DaoModelException(Exception originalException) : base(originalException.Message)
        {
            this.OriginalException = originalException;
            this.DbCommand = null;
            this.OriginalStackTrace = originalException.StackTrace;
        }
    }
}
