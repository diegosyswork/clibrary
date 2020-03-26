using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Actions
{
    public interface IGetAll<TEntity>
    {
        IList<TEntity> GetAll();
        IList<TEntity> GetAll(int commandTimeOut);
        IList<TEntity> GetAll(IDbConnection dbConnection);
        IList<TEntity> GetAll(IDbConnection dbConnection, int commandTimeOut);
        IList<TEntity> GetAll(IDbTransaction dbTransaction);
        IList<TEntity> GetAll(IDbTransaction dbTransaction, int commandTimeOut);
        IList<TEntity> GetAll(IDbConnection dbConnection, IDbTransaction dbTransaction);
        IList<TEntity> GetAll(IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut);
    }
}
