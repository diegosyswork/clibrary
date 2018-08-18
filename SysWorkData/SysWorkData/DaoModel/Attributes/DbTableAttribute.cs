using System;

namespace SysWork.Data.DaoModel.Attributes
{
    /// <summary>
    /// Configures the name of the db table related to this entity. If the attribute is not specified, the class name is used instead.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
