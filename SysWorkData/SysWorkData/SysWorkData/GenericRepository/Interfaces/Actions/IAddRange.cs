using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IAddRange<TEntity>
    {
        bool AddRange(IList<TEntity> entities);
        bool AddRange(IList<TEntity> entities, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction);
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out string errMessage);
        bool AddRange(IList<TEntity> entities, out string errMessage, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out string errMessage, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, out string errMessage, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, int? commandTimeOut);
    }
}
