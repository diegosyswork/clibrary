using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    interface IGetById<TEntity>
    {
        TEntity GetById(object id);
        TEntity GetById(object id, int commandTimeOut);
        TEntity GetById(object id, IDbConnection dbConnection);
        TEntity GetById(object id, IDbConnection dbConnection, int commandTimeOut);
        TEntity GetById(object id, IDbTransaction dbTransaction);
        TEntity GetById(object id, IDbTransaction dbTransaction, int commandTimeOut);
        TEntity GetById(object id, IDbConnection dbConnection, IDbTransaction dbTransaction);
        TEntity GetById(object id, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);
    }

}
