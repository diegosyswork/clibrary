using System;

namespace SysWork.Data.NetCore.Common.Attributes
{
    #region DOCUMENTATION Class
    /// <summary>
    /// Use this class to decorate a class that represents a database table. 
    /// Supports table names with special characters.
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
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the Table. By default the Table Name is the class Name.
        /// In case the DbTable contains special charactes like " " (Spaces) "-" minus, etc, use this attribute to specify 
        /// the real Table name thats no represented by the class name by restrictions of languague.(C#)
        /// </summary>
        /// <value>
        /// The name of the Table.
        /// </value>
        public string Name { get; set; }
    }
}
