using System.Collections.Generic;

namespace SysWork.Data.Common.Dictionarys
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbObjectNameStartsEnders
    {
        /// <summary>
        /// The start end characters/
        /// </summary>
        public static readonly Dictionary<EDataBaseEngine, Dictionary<string, string>> StartEndCharacters = new Dictionary<EDataBaseEngine, Dictionary<string, string>>()
        {
                { EDataBaseEngine.MSSqlServer, new Dictionary<string, string>() { { "starts", "[" }, { "ends", "]" } } },
                { EDataBaseEngine.OleDb, new Dictionary<string, string>() { { "starts", "[" }, { "ends", "]" } } } ,
                { EDataBaseEngine.SqLite, new Dictionary<string, string>() { { "starts", "[" }, { "ends", "]" } }},
                { EDataBaseEngine.MySql, new Dictionary<string, string>() { { "starts", "`" }, { "ends", "`" } } }
        };

    }
}
