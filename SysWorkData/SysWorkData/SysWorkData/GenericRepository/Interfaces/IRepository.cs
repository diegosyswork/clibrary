﻿using SysWork.Data.Common.Interfaces.Actions;

namespace SysWork.Data.GenericRepository.Interfaces
{
    public interface IRepository<TEntity> :
        IAdd<TEntity>,
        IAddAsync<TEntity>,
        IAddRange<TEntity>,
        IAddRangeAsync<TEntity>,
        IDeleteAll<TEntity>,
        IDeleteAllAsync<TEntity>,
        IDeleteByGenericWhereFilter<TEntity>,
        IDeleteByGenericWhereFilterAsync<TEntity>,
        IDeleteById<TEntity>,
        IDeleteByIdAsync<TEntity>,
        IDeleteByLambdaExpressionFilter<TEntity>,
        IDeleteByLambdaExpressionFilterAsync<TEntity>,
        IExists<TEntity>,
        IExistsAsync<TEntity>,
        IFind<TEntity>,
        IFindAsync<TEntity>,
        IGetAll<TEntity>,
        IGetAllAsync<TEntity>,
        IGetByGenericWhereFilter<TEntity>,
        IGetByGenericWhereFilterAsync<TEntity>,
        IGetById<TEntity>,
        IGetByIdAsync<TEntity>,
        IGetByLambdaExpressionFilter<TEntity>,
        IGetByLambdaExpressionFilterAsync<TEntity>,
        IGetDataTableByGenericWhereFilter<TEntity>,
        IGetDataTableByGenericWhereFilterAsync<TEntity>,
        IGetDataTableByLambdaExpressionFilter<TEntity>,
        IGetDataTableByLambdaExpressionFilterAsync<TEntity>,
        IGetListByGenericWhereFilter<TEntity>,
        IGetListByGenericWhereFilterAsync<TEntity>,
        IGetListByLambdaExpressionFilter<TEntity>,
        IGetListByLambdaExpressionFilterAsync<TEntity>,
        IGetListBySqlLam<TEntity>,
        IGetListBySqlLamAsync<TEntity>,
        IRecordCount<TEntity>,
        IRecordCountAsync<TEntity>,
        IUpdate<TEntity>,
        IUpdateAsync<TEntity>,
        IUpdateRange<TEntity>,
        IUpdateRangeAsync<TEntity>
    { }
}
