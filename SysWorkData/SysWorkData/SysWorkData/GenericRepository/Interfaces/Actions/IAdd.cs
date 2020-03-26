using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IAdd<TEntity>
    {
        long Add(TEntity entity);
        long Add(TEntity entity, int commandTimeOut);
        long Add(TEntity entity, IDbConnection paramDbConnection);
        long Add(TEntity entity, IDbConnection paramDbConnection, int commandTimeOut);
        long Add(TEntity entity, IDbTransaction paramDbTransaction);
        long Add(TEntity entity, IDbTransaction paramDbTransaction, int commandTimeOut);
        long Add(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        long Add(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut);
        long Add(TEntity entity, out string errMessage);
        long Add(TEntity entity, out string errMessage, int commandTimeOut);

    }
}
