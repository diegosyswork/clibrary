using System.Collections.Generic;
using SysWork.Data.NetCore.Common.ValueObjects;

/// <summary>
/// 
/// </summary>
namespace SysWork.Data.NetCore.Common.Dictionaries
{
    /// <summary>
    /// Character dictionary with which the names of database objects 
    /// should start and end, according to the DatabaseEngine. 
    /// For example "[" "]", for MSSqlserver
    /// </summary>
    public static class DbObjectNameStartsEnders
    {
        /// <summary>
        /// Character dictionary with which the names of database objects 
        /// should start and end, according to the DatabaseEngine. 
        /// For example "[" "]", for MSSqlserver
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
