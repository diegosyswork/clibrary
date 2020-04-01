using System;

namespace SysWork.Data.Common.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
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
