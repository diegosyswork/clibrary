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
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeOut);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeOut);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction);
        IList<TEntity> GetListByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction,int? commandTimeOut);
    }
}
