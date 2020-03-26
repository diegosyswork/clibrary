using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IDeleteAll<TEntity>
    {
        long DeleteAll();
        long DeleteAll(int commandTimeOut);
        long DeleteAll(IDbConnection dbConnection);
        long DeleteAll(IDbConnection dbConnection, int commandTimeOut);
        long DeleteAll(IDbTransaction dbTransaction);
        long DeleteAll(IDbTransaction dbTransaction, int commandTimeOut);
        bool DeleteAll(out string errMessage);
        bool DeleteAll(out string errMessage, int commandTimeOut);
        long DeleteAll(IDbConnection dbConnection, IDbTransaction dbTransaction);
        long DeleteAll(IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut );
    }
}
