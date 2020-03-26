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
        bool DeleteById(long Id, IDbConnection dbConnection);
        bool DeleteById(long Id, IDbConnection dbConnection, int commandTimeOut);
        bool DeleteById(long Id, IDbTransaction dbTransaction);
        bool DeleteById(long Id, IDbTransaction dbTransaction, int commandTimeOut);
        bool DeleteById(long Id, IDbConnection dbConnection, IDbTransaction dbTransaction);
        bool DeleteById(long Id, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut);
        bool DeleteById(long Id, out string errMessage);
        bool DeleteById(long Id, out string errMessage, int commandTimeOut);
        bool DeleteById(long Id, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);
        bool DeleteById(long Id, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
