using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IGetDataTableByGenericWhereFilter<TEntity>
    {
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeout);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeout);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeout);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, int? CommandTimeout);
    }
}
