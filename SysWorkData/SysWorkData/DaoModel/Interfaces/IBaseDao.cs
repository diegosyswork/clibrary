using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.DaoModel.Interfaces
{
    /// <summary>
    /// Interfaz generica de los Daos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseDao<T>
    {
        long Add(T entity);
        bool AddRange(IList<T> entities);
        bool Update(T entity);
        bool UpdateRange(IList<T> entities);
        bool DeleteById(long id);
        T GetById(object id);
        IList<T> GetAll();
        IList<T> Find(IEnumerable<object> ids);
    }
}
