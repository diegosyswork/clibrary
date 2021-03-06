using System;
using System.Linq;
using System.Reflection;
using System.Text;
using SysWork.Data.Common.Syntax;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.Attributes
{
    #region DOCUMENTATION Class
    /// <summary>
    /// Use this class to decorate the properties of a class that represents a database column. 
    /// supports column names with special characters.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// [DbTable(Name = "Persons")]
    /// public class Person
    /// {
    ///     [DbColumn(IsIdentity = True, IsPrimary = True)]
    ///     public long IdPerson {get; set;}
    ///     
    ///     [DbColumn]
    ///     public string LastName {get; set;}
    ///     
    ///     [DbColumn]
    ///     public string FirstName {get; set;}
    ///     
    ///     [DbColumn]
    ///     public string PassPort {get; set;}
    ///     
    ///     [DbColumn(ColumnName = "Birth Day")]
    ///     public string Birth_Day {get; set;}
    /// }
    /// ]]>
    /// </code>
    /// </example>
    #endregion
    [AttributeUsage(AttributeTargets.Property)]
    public class DbColumnAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the column. By default the Column Name is the property Column Name.
        /// In case the DbColum contains special charactes like " " (Spaces) "-" minus, etc, use this attribute to specify 
        /// the real DbColumn name thats no represented by the property by restrictions of languague.(C#)
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public string ColumnName { get; set; } = null;
        
        /// <summary>
        /// Sets true if requieres conversion.
        /// </summary>
        public bool Convert { get; set; }
        
        /// <summary>
        /// Sets true, if is part of the primary key. In UPDATEs methods, uses this to deteminate what row update.
        /// </summary>
        public bool IsPrimary { get; set; }
        
        /// <summary>
        /// Sets true if is an Identity field.
        /// </summary>
        public bool IsIdentity { get; set; }
    }
}
