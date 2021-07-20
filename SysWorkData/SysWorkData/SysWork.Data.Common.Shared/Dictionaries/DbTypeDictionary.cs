using System;
using System.Collections.Generic;
using System.Data;

namespace SysWork.Data.Common.Dictionaries
{
    /// <summary>
    /// Dictionary of equivalences between database data types and DbType
    /// </summary>
    public static class DbTypeDictionary
    {
        /// <summary>
        /// Dictionary of equivalences between database data types and DbType
        /// </summary>
        public static readonly Dictionary<string, DbType> DbColumnTypeToDbTypeEnum = new Dictionary<string, DbType>(StringComparer.OrdinalIgnoreCase)
        {
            { "smallint", DbType.Int16 },
            { "int", DbType.Int32 },
            { "bigint", DbType.Int64 },
            { "integer", DbType.Int64 },
            { "long", DbType.Int64 },
            { "bit", DbType.Boolean },
            { "bool", DbType.Boolean },
            { "boolean", DbType.Boolean },
            { "blob", DbType.Binary },
            { "binary", DbType.Binary },
            { "image", DbType.Binary },
            { "datetime", DbType.DateTime },
            { "smalldatetime", DbType.DateTime },
            { "date", DbType.Date },
            { "double", DbType.Double },
            { "float", DbType.Double },
            { "real", DbType.Double },
            { "uniqueidentifier", DbType.Guid },
            { "guid", DbType.Guid },
            { "money", DbType.Decimal},
            { "currency", DbType.Decimal},
            { "smallmoney", DbType.Decimal},
            { "decimal", DbType.Decimal},
            { "numeric", DbType.Decimal},
            { "unsignedtinyint", DbType.UInt16},
            { "single", DbType.Single},
            { "string", DbType.String },
            { "varchar", DbType.String },
            { "nvarchar", DbType.String },
            { "longtext", DbType.String },
            { "nchar", DbType.String },
            { "char", DbType.AnsiStringFixedLength},
            { "ntext", DbType.String },
            { "mediumtext", DbType.String },
            { "text", DbType.String },
            { "WChar", DbType.String },
            { "varwchar", DbType.String },
            { "longvarwchar", DbType.String },
            { "timestamp", DbType.DateTime},
            { "tinyint", DbType.Int32 }
        };
    }
}
