using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IUpdate<TEntity>
    {
        bool Update(TEntity entity);
        bool Update(TEntity entity, int commandTimeOut);
        bool Update(TEntity entity, IDbConnection dbConnection);
        bool Update(TEntity entity, IDbConnection dbConnection, int commandTimeOut);
        bool Update(TEntity entity, IDbTransaction dbTransaction);
        bool Update(TEntity entity, IDbTransaction dbTransaction, int commandTimeOut);
        bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction);
        bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut);
        bool Update(TEntity entity, out string errMessage);
        bool Update(TEntity entity, out string errMessage, int commandTimeOut);
        bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);
        bool Update(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
