using System;
using System.Data;

namespace SysWork.Data.Common.DbInfo
{
    /// <summary>
    /// Clase Interna
    /// </summary>
    internal class ColumnDbInfo
    {
        internal DbType DbType { get; set; }
        internal Int32? MaxLenght { get; set; }
    }
}
