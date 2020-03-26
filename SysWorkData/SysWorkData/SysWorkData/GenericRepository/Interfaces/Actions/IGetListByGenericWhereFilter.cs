using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IGetListByGenericWhereFilter<TEntity>
    {
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeOut);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, int commandTimeOut);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, int commandTimeOut);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction,int? commandTimeOut);
    }
}
