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
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, int commandTimeOut);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, int commandTimeOut);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        TEntity GetByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut);
    }
}
