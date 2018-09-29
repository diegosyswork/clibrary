using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SysWork.Data.Common.LambdaSqlBuilder.Adapter;

namespace SysWork.Data.Common.LambdaSqlBuilder.ValueObjects
{
    /// <summary>
    /// An enumeration of the available SQL adapters. Can be used to set the backing database for db specific SQL syntax
    /// </summary>
    public enum SqlAdapter
    {
        SqlServer2008,
        SqlServer2012,
        MySql,
        SQLite
    }
}
