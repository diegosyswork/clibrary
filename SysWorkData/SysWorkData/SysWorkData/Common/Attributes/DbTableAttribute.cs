using System;

namespace SysWork.Data.Common.Attributes
{
    /// <summary>
    /// Configures the name of the db table related to this entity. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of table.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
