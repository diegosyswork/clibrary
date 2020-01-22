using System.Collections.Generic;
using SysWork.Data.GenericRepository;

namespace TestFormsABM.Data
{
    public class ClienteRepository : BaseRepository<Cliente>
    {
        public ClienteRepository(string connectionString) : base(connectionString)
        {

        }

        public Cliente GetByCodCliente(string codCliente)
        {
            return GetByLambdaExpressionFilter(c => c.codCliente == codCliente);
        }

        public string GetProximoCodigo()
        {
            return "";
        }

        public IList<Cliente> GetListActivos()
        {
            return GetListByLambdaExpressionFilter(c => c.activo == true);
        }

    }
}
