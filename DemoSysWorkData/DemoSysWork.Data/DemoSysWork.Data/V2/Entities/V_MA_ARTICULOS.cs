using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SysWork.Data.Mapping;
namespace InterfaceB2B.Data.Entities
{
	[Table(Name = "V_MA_ARTICULOS")]
	public class V_MA_ARTICULOS
	{
		/// <summary>
		/// This class was created automatically with the class EntityClassFromDb.
		/// Please check the DbTypes and the field names.
		/// </summary>

		[Column(IsIdentity = true)]
		public long ID { get; set; }
		[Column(IsPrimaryKey = true)]
		public string IDARTICULO { get; set; }
		[Column()]
		public string CODIGOBARRA { get; set; }
		[Column()]
		public string DESCRIPCION { get; set; }
		[Column()]
		public string CUENTAPROVEEDOR { get; set; }
		[Column()]
		public string IDUNIDAD { get; set; }
		[Column()]
		public string IDRUBRO { get; set; }
		[Column()]
		public string IDTIPO { get; set; }
		[Column()]
		public bool USASERIE { get; set; }
		[Column()]
		public bool USALOTE { get; set; }
		[Column()]
		public bool EXENTO { get; set; }
		[Column()]
		public string NOTAS { get; set; }
		[Column()]
		public long? PUNTOPEDIDO { get; set; }
		[Column()]
		public decimal? COSTO { get; set; }
		[Column()]
		public decimal? IMPUESTOS { get; set; }
		[Column()]
		public decimal? PRECIO1 { get; set; }
		[Column()]
		public decimal? PRECIO2 { get; set; }
		[Column()]
		public decimal? PRECIO3 { get; set; }
		[Column()]
		public decimal? PRECIO4 { get; set; }
		[Column()]
		public decimal? PRECIO5 { get; set; }
		[Column()]
		public bool SUSPENDIDO { get; set; }
		[Column()]
		public long? PUNTOMAXIMO { get; set; }
	
		[Column()]
		public string PoliticaPrecios { get; set; }
		[Column()]
		public double? TasaIVA { get; set; }
		[Column()]
		public decimal? PRECIO6 { get; set; }
		[Column()]
		public decimal? PRECIO7 { get; set; }
		[Column()]
		public decimal? PRECIO8 { get; set; }
		[Column()]
		public decimal? PRECIO9 { get; set; }
		[Column()]
		public decimal? PRECIO10 { get; set; }
		[Column()]
		public bool SUSPENDIDOV { get; set; }
		[Column()]
		public bool SUSPENDIDOC { get; set; }
		[Column()]
		public string IMAGEN_ICONO { get; set; }
		[Column()]
		public string IMAGEN_AMPLIADA { get; set; }
		[Column()]
		public string IMAGEN_AMPLIADA2 { get; set; }
		[Column()]
		public string USUARIO { get; set; }
		[Column()]
		public bool SUSPENDIDOGM { get; set; }
		[Column()]
		public string ABM { get; set; }
		[Column()]
		public double? CANT_MINIMA_PEDIDO_PROVEEDOR { get; set; }
		[Column()]
		public string UBICACION_HABITUAL { get; set; }
		[Column()]
		public string PROCEDENCIA { get; set; }
		[Column()]
		public bool BEBIDA_ALCOHOLICA { get; set; }
		[Column()]
		public bool CONSIGNADO { get; set; }
		[Column()]
		public bool SUSPENDIDOWEB { get; set; }
		[Column()]
		public bool NO_CONTROLA_STOCK { get; set; }
		[Column()]
		public string IDUNIDADSUELTOS { get; set; }
		[Column()]
		public bool SolicitaDatosMatarife { get; set; }
		[Column()]
		public string IDMONEDA { get; set; }
		[Column()]
		public string CODIGOARTPROVEEDOR { get; set; }
		[Column()]
		public string CODIGOBARRA2 { get; set; }
		[Column()]
		public string CODIGOBARRA3 { get; set; }
		[Column()]
		public string CODIGOBARRA4 { get; set; }
		[Column()]
		public string CODIGOBARRA5 { get; set; }
		[Column()]
		public string IDFAMILIA { get; set; }
		[Column()]
		public string ULTIMO_NRO_DESPACHO { get; set; }
		[Column()]
		public string IDBONIFICACION { get; set; }
		[Column()]
		public decimal? MK_PRECIO1 { get; set; }
		[Column()]
		public decimal? MK_PRECIO2 { get; set; }
		[Column()]
		public decimal? MK_PRECIO3 { get; set; }
		[Column()]
		public decimal? MK_PRECIO4 { get; set; }
		[Column()]
		public decimal? MK_PRECIO5 { get; set; }
		[Column()]
		public decimal? MK_PRECIO6 { get; set; }
		[Column()]
		public decimal? MK_PRECIO7 { get; set; }
		[Column()]
		public decimal? MK_PRECIO8 { get; set; }
		[Column()]
		public decimal? MK_PRECIO9 { get; set; }
		[Column()]
		public decimal? MK_PRECIO10 { get; set; }
		[Column()]
		public bool IVA_INCLUIDO { get; set; }
		[Column()]
		public bool SOLICITA_PRECIO_REFERENCIA { get; set; }
		[Column()]
		public string INFORMACION_ADICIONAL { get; set; }
		[Column()]
		public string URL1 { get; set; }
		[Column()]
		public string URL2 { get; set; }
		[Column()]
		public string URL3 { get; set; }
		[Column()]
		public string IDGARANTIA { get; set; }
		[Column()]
		public decimal? PRECIO_WEB { get; set; }
		[Column()]
		public string IDMONEDA_COMPRA { get; set; }
		[Column()]
		public decimal? COSTO_MONEDA { get; set; }
		[Column()]
		public double? COTIZACION_MONEDA { get; set; }
		[Column()]
		public bool PideDescripcionAdicional { get; set; }
		[Column()]
		public long? PUNTOS_CANJEABLE { get; set; }
		[Column()]
		public string URL4 { get; set; }
		[Column()]
		public string URL5 { get; set; }
		[Column()]
		public string URL6 { get; set; }
		[Column()]
		public string IDTEMPORADA { get; set; }
		[Column()]
		public string CODIGO_NEXTEL { get; set; }
		[Column()]
		public string INFORMACION_ADICIONAL_TEXTO { get; set; }
		[Column()]
		public bool REQUIERE_CONFIRMACION_COMPONENTES { get; set; }
		[Column()]
		public double? LARGO { get; set; }
		[Column()]
		public double? ALTO { get; set; }
		[Column()]
		public double? PROFUNDO { get; set; }
		[Column()]
		public double? PESO_X_UNIDAD { get; set; }
		[Column()]
		public string UNIDAD_DIMENSIONES { get; set; }
		[Column()]
		public bool PESABLE { get; set; }
		[Column()]
		public string LOTE_AUTOMATICO { get; set; }
		[Column()]
		public string SERIE_AUTOMATICA { get; set; }
		[Column()]
		public bool SOLICITA_TROPA { get; set; }
		[Column()]
		public string ARTICULO_COMPLEMENTARIO { get; set; }
		[Column()]
		public bool PESO_X_CANTIDAD { get; set; }
		[Column()]
		public long? FID_PUNTOS_ACUMULA { get; set; }
		[Column()]
		public bool PUBLICAR_EN_MERCADOLIBRE { get; set; }
		/*
		[Column()]
		public string ID_PUBLICACION_MERCADOLIBRE { get; set; }
		[Column()]
		public string ID_CATEGORIA_MERCADOLIBRE { get; set; }
		[Column()]
		public double? DTO_X_CANTIDAD { get; set; }
		[Column()]
		public double? CANT_DTO_X_CANTIDAD { get; set; }
		[Column()]
		public double? DTO_X_CANTIDAD_2 { get; set; }
		[Column()]
		public double? CANT_DTO_X_CANTIDAD_2 { get; set; }
		[Column()]
		public double? BONIFICACION_COMPRA_MANUAL { get; set; }
		*/
	}
}
