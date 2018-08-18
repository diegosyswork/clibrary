using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.DaoModel.Attributes;
namespace GerdannaDataManager.Entities
{
	[DbTable (Name = "Personas")]
	public class Persona
	{
		/// <summary>
		/// **********************************************************************
		/// 
		/// Esta clase fue generada automaticamente por la clase EntityClassFromDb
		/// 
		/// Fecha: 14/08/2018 17:31:01
		/// 
		/// DataSource: NT-SYSWORK\SQLEXPRESS
		/// InitialCatalog: TEST
		/// 
		/// **********************************************************************
		/// </summary>

		[DbColumn(IsIdentity = true, IsPrimary = true)]
		public long IdPersona { get; set; }
		[DbColumn()]
		public string Dni { get; set; }
		[DbColumn()]
		public string Apellido { get; set; }
		[DbColumn()]
		public string Nombre { get; set; }
		[DbColumn()]
		public DateTime FechaNacimiento { get; set; }
		[DbColumn()]
		public string Telefono { get; set; }
	}
}
