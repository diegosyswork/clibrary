using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.DaoModel.Attributes;

namespace TestFormsABM.Data
{
    [DbTable(Name = "Clientes")]
    public class Cliente
    {
        [DbColumn(IsIdentity = true, IsPrimary = true)]
        public long idCliente { get; set; }
        [DbColumn]
        public string codCliente { get; set; }
        [DbColumn]
        public string razonSocial { get; set; }
        [DbColumn]
        public bool activo { get; set; }

    }
}
