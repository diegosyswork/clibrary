using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepository.Interfaces.Accions
{
    public interface IGetAll<TEntity>
    {
        IList<TEntity> GetAll();
        IList<TEntity> GetAll(int commandTimeOut);
        IList<TEntity> GetAll(IDbConnection paramDbConnection);
        IList<TEntity> GetAll(IDbConnection paramDbConnection, int commandTimeOut);
        IList<TEntity> GetAll(IDbTransaction paramDbTransaction);
        IList<TEntity> GetAll(IDbTransaction paramDbTransaction, int commandTimeOut);
        IList<TEntity> GetAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction);
        IList<TEntity> GetAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut);
    }
}
