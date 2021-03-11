using SysWork.Data.Common.Interfaces.Actions;

namespace SysWork.Data.GenericRepository.Interfaces
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IAdd{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IAddAsync{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IAddRange{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IAddRangeAsync{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IDeleteAll{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IDeleteAllAsync{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IDeleteByGenericWhereFilter{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IDeleteById{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IDeleteByLambdaExpressionFilter{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IExists{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IFind{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetAll{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetAllAsync{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetByGenericWhereFilter{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetById{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetByLambdaExpressionFilter{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetDataTableByGenericWhereFilter{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetDataTableByLambdaExpressionFilter{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetListByGenericWhereFilter{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetListByLambdaExpressionFilter{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IGetListBySqlLam{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IRecordCount{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IUpdate{TEntity}" />
    /// <seealso cref="SysWork.Data.Common.Interfaces.Actions.IUpdateRange{TEntity}" />
    
    public interface IRepository<TEntity> :
        IAdd<TEntity>,
        IAddAsync<TEntity>,
        IAddRange<TEntity>,
        IAddRangeAsync<TEntity>,
        IDeleteAll<TEntity>,
        IDeleteAllAsync<TEntity>,
        IDeleteByGenericWhereFilter<TEntity>,
        IDeleteById<TEntity>,
        IDeleteByLambdaExpressionFilter<TEntity>,
        IExists<TEntity>,
        IFind<TEntity>,
        IGetAll<TEntity>,
        IGetAllAsync<TEntity>,
        IGetByGenericWhereFilter<TEntity>,
        IGetById<TEntity>,
        IGetByLambdaExpressionFilter<TEntity>,
        IGetDataTableByGenericWhereFilter<TEntity>,
        IGetDataTableByLambdaExpressionFilter<TEntity>,
        IGetListByGenericWhereFilter<TEntity>,
        IGetListByLambdaExpressionFilter<TEntity>,
        IGetListBySqlLam<TEntity>,
        IRecordCount<TEntity>,
        IUpdate<TEntity>,
        IUpdateRange<TEntity> { }
}
