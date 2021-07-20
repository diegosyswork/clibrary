using System;
using SysWork.Data.Common.Attributes;
using SysWork.Data.Mapping;

namespace GerdannaDataManager.Entities
{
    [Table (Name = "personas")]
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

		[Column(IsIdentity = true, IsPrimaryKey = true)]
		public long IdPersona { get; set; }
		[Column()]
		public string Dni { get; set; }
		[Column()]
		public string Apellido { get; set; }
		[Column()]
		public string Nombre { get; set; }
		[Column()]
		public DateTime FechaNacimiento { get; set; }
		[Column()]
		public string Telefono { get; set; }

        public Persona()
        {

        }
        public Persona(long idPersona, string apellido, string nombre, string dni, DateTime fechaNacimiento, string telefono)
        {
            this.IdPersona = idPersona;
            this.Apellido = apellido;
            this.Nombre = nombre;
            this.Dni = dni;
            this.FechaNacimiento = fechaNacimiento;
            this.Telefono = telefono;
        }
        public Persona(string apellido, string nombre, string dni, DateTime fechaNacimiento, string telefono)
        {
            this.Apellido = apellido;
            this.Nombre = nombre;
            this.Dni = dni;
            this.FechaNacimiento = fechaNacimiento;
            this.Telefono = telefono;
        }
    }
}
