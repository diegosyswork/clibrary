using System;

namespace SysWork.Data.Common.Attributes
{
    #region DOCUMENTATION Class
    /// <summary>
    /// Use this class to decorate the properties of a class that represents a database entity. 
    /// Supports Columns columns with special characters.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// [DbTable(Name = "Persons")]
    /// public class Person
    /// {
    ///     [DbColumn(IsIdentity = True)]
    ///     public long IdPerson {get; set;}
    ///     
    ///     [DbColumn(IsPrimary = True)]
    ///     public string LastName {get; set;}
    ///     
    ///     [DbColumn(IsPrimary = True)]
    ///     public string FirstName {get; set;}
    ///     
    ///     [DbColumn(IsPrimary = True)]
    ///     public string PassPort {get; set;}
    ///     
    ///     [DbColumn(IsPrimary = True)]
    ///     public string BirthDay {get; set;}
    ///     
    ///     [DbColumn(ColumnName = Birth Day")]
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
        /// Sets true if requieres implicit conversion.
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
