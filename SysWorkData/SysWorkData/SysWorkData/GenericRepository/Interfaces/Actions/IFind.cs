using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IFind<TEntity>
    {
        IList<TEntity> Find(IEnumerable<object> ids);
        IList<TEntity> Find(IEnumerable<object> ids, int commandTimeOut);
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection);
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection, int commandTimeOut);
        IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction dbTransaction);
        IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction dbTransaction, int commandTimeOut);
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        IList<TEntity> Find(IEnumerable<object> ids, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction,int? commandTimeOut);

    }
}
