using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Accions
{
    public interface IAddRange<TEntity>
    {
        bool AddRange(IList<TEntity> entities);
        bool AddRange(IList<TEntity> entities, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction);
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out string errMessage);
        bool AddRange(IList<TEntity> entities, out string errMessage, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, out string errMessage, out long recordsAffected);
        bool AddRange(IList<TEntity> entities, out string errMessage, out long recordsAffected, int commandTimeOut);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, out IEnumerable<object> addedIds);
        bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, int? commandTimeOut);
    }
}
