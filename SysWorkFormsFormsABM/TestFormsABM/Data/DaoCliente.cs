using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.DaoModel;

namespace TestFormsABM.Data
{
    public class DaoCliente : BaseDao<Cliente>
    {
        public DaoCliente(string connectionString) : base(connectionString)
        {

        }

        public Cliente GetByCodCliente(string codCliente)
        {
            Cliente cliente = null;

            var resultado = GetListByLambdaExpressionFilter(c => c.codCliente == codCliente);
            if (resultado != null && resultado.Count > 0)
                cliente = resultado[0];

            return cliente;
        }

        public string GetProximoCodigo()
        {
            return GetNextCode("codCliente", 6);
        }

        public IList<Cliente> GetListActivos()
        {
            return GetListByLambdaExpressionFilter(c => c.activo == true);
        }

    }
}
