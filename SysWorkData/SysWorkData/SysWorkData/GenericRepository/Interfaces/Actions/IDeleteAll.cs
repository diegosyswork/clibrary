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
        long DeleteAll(IDbConnection paramDbConnection);
        long DeleteAll(IDbConnection paramDbConnection, int commandTimeOut);
        long DeleteAll(IDbTransaction paramDbTransaction);
        long DeleteAll(IDbTransaction paramDbTransaction, int commandTimeOut);
        bool DeleteAll(out string errMessage);
        bool DeleteAll(out string errMessage, int commandTimeOut);
        long DeleteAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        long DeleteAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut );
    }
}
