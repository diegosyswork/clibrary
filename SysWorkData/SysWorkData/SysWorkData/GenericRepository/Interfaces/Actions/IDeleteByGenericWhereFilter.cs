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
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, out long recordsAffected, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbTransaction paramDbTransaction, out long recordsAffected, int commandTimeOut);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected);
        bool DeleteByGenericWhereFilter(GenericWhereFilter whereFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int? commandTimeOut);
    }
}
