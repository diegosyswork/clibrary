using System;

namespace SysWork.Data.NetCore.Common.Attributes
{
    #region DOCUMENTATION Class
    /// <summary>
    /// Use this class to decorate a class that represents a database view. 
    /// Supports view names with special characters.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// [DbView(Name = "PersonsView")]
    /// public class Person
    /// {
    ///     [DbColumn]
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
    public class DbViewAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of view.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
