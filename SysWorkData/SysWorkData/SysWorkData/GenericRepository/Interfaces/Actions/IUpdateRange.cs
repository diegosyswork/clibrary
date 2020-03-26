using System.Collections.Generic;
using System.Data;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IUpdateRange<TEntity>
    {
        bool UpdateRange(IList<TEntity> entities);
        bool UpdateRange(IList<TEntity> entities, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection);
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, IDbTransaction paramDbTransaction);
        bool UpdateRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, out string errMessage);
        bool UpdateRange(IList<TEntity> entities, out string errMessage, int commandTimeOut);
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);
        bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
