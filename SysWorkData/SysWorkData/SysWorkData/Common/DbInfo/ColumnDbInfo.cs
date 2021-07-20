using System;
using System.Data;

namespace SysWork.Data.Common.DbInfo
{
    /// <summary>
    /// Internal Class for manage DbColumnInfo
    /// </summary>
    public class DbColumnInfo
    {
        public DbType DbType { get; set; }
        public Int32? MaxLenght { get; set; }
    }
}
