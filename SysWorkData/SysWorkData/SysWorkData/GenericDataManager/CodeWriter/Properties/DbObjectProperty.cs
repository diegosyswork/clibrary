using System;
using System.Collections.Generic;
using System.Text;

namespace SysWork.Data.GenericDataManager.CodeWriter.Properties
{
    public class DbObjectWriterProperty
    {
        public string ObjectName { get; set; }
        public string PublicPropertyName { get; set; }

        public DbObjectWriterProperty(string ObjectName, string PublicPropertyName)
        {
            this.ObjectName = ObjectName;
            this.PublicPropertyName = PublicPropertyName;
        }

    }
}
