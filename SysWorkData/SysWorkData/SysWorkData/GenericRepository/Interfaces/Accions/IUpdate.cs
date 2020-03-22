using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Accions
{
    public interface IUpdate<TEntity>
    {
        bool Update(TEntity entity);
        bool Update(TEntity entity, int commandTimeOut);
        bool Update(TEntity entity, IDbConnection paramDbConnection);
        bool Update(TEntity entity, IDbConnection paramDbConnection, int commandTimeOut);
        bool Update(TEntity entity, IDbTransaction paramDbTransaction);
        bool Update(TEntity entity, IDbTransaction paramDbTransaction, int commandTimeOut);
        bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int commandTimeOut);
        bool Update(TEntity entity, out string errMessage);
        bool Update(TEntity entity, out string errMessage, int commandTimeOut);
        bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);
        bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
