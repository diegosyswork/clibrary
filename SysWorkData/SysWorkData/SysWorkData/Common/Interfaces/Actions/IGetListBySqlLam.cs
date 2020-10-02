using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using SysWork.Data.Common.LambdaSqlBuilder;

namespace SysWork.Data.Common.Interfaces.Actions
{
    /// <summary>
    /// Gets an IList<TEntity> with the records that match with the SqlLam Object query.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGetListBySqlLam<TEntity>
    {
        /// <summary>
        /// Gets an IList<TEntity> with the records that match with the SqlLam Object query.
        /// </summary>
        /// <param name="sqlLam">The SqlLam Object.</param>
        /// <returns></returns>
        IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam);

        /// <summary>
        /// Gets an IList<TEntity> with the records that match with the SqlLam Object query using a custom dbCommand timeout.
        /// </summary>
        /// <param name="sqlLam">The SqlLam Object.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, int commandTimeOut);

        /// <summary>
        /// Gets an IList<TEntity> with the records that match with the SqlLam Object query using an DbConnection.
        /// </summary>
        /// <param name="sqlLam">The SqlLam Object.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbConnection dbConnection);

        /// <summary>
        /// Gets an IList<TEntity> with the records that match with the SqlLam Object query using an DbConnection and a custom dbCommand timeout.
        /// </summary>
        /// <param name="sqlLam">The SqlLam Object.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbConnection dbConnection, int commandTimeOut);

        /// <summary>
        /// Gets an IList<TEntity> with the records that match with the SqlLam Object query using an DbTransaction.
        /// </summary>
        /// <param name="sqlLam">The SqlLam Object.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets an IList<TEntity> with the records that match with the SqlLam Object query using an DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="sqlLam">The SqlLam Object.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbTransaction dbTransaction, int commandTimeOut);

        /// <summary>
        /// Gets an IList<TEntity> with the records that match with the SqlLam Object query using an DbConnection and DbTransaction.
        /// </summary>
        /// <param name="sqlLam">The SqlLam Object.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbConnection dbConnection, IDbTransaction dbTransaction);

        /// <summary>
        /// Gets an IList<TEntity> with the records that match with the SqlLam Object query using an DbConnection, DbTransaction and a custom dbCommand timeout.
        /// </summary>
        /// <param name="sqlLam">The SqlLam Object.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        IList<TEntity> GetListBySqlLam(SqlLamBase sqlLam, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);
    }
}
