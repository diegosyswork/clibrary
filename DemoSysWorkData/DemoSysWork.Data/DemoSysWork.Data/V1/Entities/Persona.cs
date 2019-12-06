using System;
using SysWork.Data.GenericRepostory.Attributes;

namespace GerdannaDataManager.Entities
{
    [DbTable (Name = "personas")]
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
