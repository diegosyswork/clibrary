using System.Collections.Generic;
using System.Data;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IUpdateRange<TEntity>
    {
        bool UpdateRange(IList<TEntity> entities);
        bool UpdateRange(IList<TEntity> entities, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection);
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, IDbTransaction dbTransaction);
        bool UpdateRange(IList<TEntity> entities, IDbTransaction dbTransaction, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction);
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, out string errMessage);
        bool UpdateRange(IList<TEntity> entities, out string errMessage, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);
        bool UpdateRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
