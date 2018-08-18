using System;

namespace SysWork.Data.DaoModel.Attributes
{
    /// <summary>
    /// Usar este atributo para decorar las propiedades en las clases modelos (Entities).
    /// Estas propiedades tienen que tener exactamente el mismo nombre que las columnas de la Tabla de Base de Datos.
    /// </summary>
    public class DbColumnAttribute : Attribute
    {
        /// <summary>
        /// Setear en TRUE si se requiere de una conversion implicita.
        /// </summary>
        public bool Convert { get; set; }
        /// <summary>
        /// Setear en TRUE si el atributo representa la primary key en la tabla. (pueden ser mas de una)
        /// y en las rutinas de actualizacion, se utiliza este campo y no otro para actualizar.
        /// </summary>
        public bool IsPrimary { get; set; }
        /// <summary>
        /// Especifica si el atributo representa a una columna de tipo identity. (Deberia haber una sola)
        /// </summary>
        public bool IsIdentity { get; set; }
    }
}
