using DaoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDaoModel.Entities;

namespace TestDaoModel
{
    public class DaoProducto : BaseDao<Producto>
    {
        private string connectionString;

        public DaoProducto(string connectionString) : base(connectionString)
        {
            this.connectionString = connectionString;
        }
        public DaoProducto(string connectionString, string tabla) : base(connectionString,tabla)
        {
            this.connectionString = connectionString;
        }

        public IList<Producto> GetByEan(long ean)
        {
            return GetListByCriteria(producto => (producto.ean == ean) );
        }
    }
}
