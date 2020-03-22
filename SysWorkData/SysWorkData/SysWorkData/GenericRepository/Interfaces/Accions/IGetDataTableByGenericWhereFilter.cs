using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.GenericRepository.Interfaces.Accions
{
    public interface IGetDataTableByGenericWhereFilter<TEntity>
    {
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeout);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, int commandTimeout);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, int commandTimeout);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        DataTable GetDataTableByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? CommandTimeout);
    }
}
