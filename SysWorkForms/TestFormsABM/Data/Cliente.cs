using SysWork.Data.GenericRepository.Attributes;

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
