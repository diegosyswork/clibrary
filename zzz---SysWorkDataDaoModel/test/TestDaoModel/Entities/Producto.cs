using DaoModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDaoModel.Entities
{
    public class Producto
    {
        [DbColumn (IsIdentity = true,IsPrimary =true)]
        public long idProducto { get; set; }

        [DbColumn]
        public long ean { get; set; }

        [DbColumn]
        public string descripcion { get; set; }

        [DbColumn]
        public long? idMarca { get; set; }

        [DbColumn]
        public long? idUnidadDeMedida { get; set; }

        [DbColumn]
        public decimal cantidadPorUnidad { get; set; }

        [DbColumn]
        public decimal multiplicadorPorUnidad { get; set; }

        [DbColumn]
        public string urlImagen { get; set; }

        [DbColumn]
        public DateTime? fechaInsercion { get; set; }

        [DbColumn]
        public bool bajaLogica { get; set; }

        [DbColumn]
        public string tmpMarca { get; set; }

        [DbColumn]
        public string tmpUnidadDeMedida { get; set; }

        [DbColumn]
        public string tmpCategoria { get; set; }
    }
}
