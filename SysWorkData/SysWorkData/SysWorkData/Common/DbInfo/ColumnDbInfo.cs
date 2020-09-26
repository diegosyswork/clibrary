using System;
using System.Data;

namespace SysWork.Data.Common.DbInfo
{
    /// <summary>
    /// Internal Class for manage DbColumnInfo
    /// </summary>
    internal class DbColumnInfo
    {
        internal DbType DbType { get; set; }
        internal Int32? MaxLenght { get; set; }
    }
}
