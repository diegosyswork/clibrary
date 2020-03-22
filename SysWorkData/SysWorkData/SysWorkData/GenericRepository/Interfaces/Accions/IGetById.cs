using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Accions
{
    interface IGetById<TEntity>
    {
        TEntity GetById(object id);
        TEntity GetById(object id, int commandTimeOut);
        TEntity GetById(object id, IDbConnection paramDbConnection);
        TEntity GetById(object id, IDbConnection paramDbConnection, int commandTimeOut);
        TEntity GetById(object id, IDbTransaction paramDbTransaction);
        TEntity GetById(object id, IDbTransaction paramDbTransaction, int commandTimeOut);
        TEntity GetById(object id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        TEntity GetById(object id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut);
    }

}
