using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IGetByGenericWhereFilter<TEntity>
    {
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeOut);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeOut);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeOut);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);
    }
}
