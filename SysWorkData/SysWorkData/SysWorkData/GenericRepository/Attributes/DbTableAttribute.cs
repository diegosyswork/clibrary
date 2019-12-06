using System;

namespace SysWork.Data.GenericRepostory.Attributes
{
    /// <summary>
    /// Configures the name of the db table related to this entity. 
    /// If the attribute is not specified, the class name is used instead.
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
