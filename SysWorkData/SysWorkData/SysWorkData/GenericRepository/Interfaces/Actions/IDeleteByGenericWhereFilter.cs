using System.Data;
using SysWork.Data.Common.Filters;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IDeleteByGenericWhereFilter<TEntity>
    {
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, out long recordsAffected, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, out long recordsAffected, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
