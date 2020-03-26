using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IDeleteById<TEntity>
    {
        bool DeleteById(long Id);
        bool DeleteById(long Id, int commandTimeOut);
        bool DeleteById(long Id, IDbConnection paramDbConnection);
        bool DeleteById(long Id, IDbConnection paramDbConnection, int commandTimeOut);
        bool DeleteById(long Id, IDbTransaction paramDbTransaction);
        bool DeleteById(long Id, IDbTransaction paramDbTransaction, int commandTimeOut);
        bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int commandTimeOut);
        bool DeleteById(long Id, out string errMessage);
        bool DeleteById(long Id, out string errMessage, int commandTimeOut);
        bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);
        bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
