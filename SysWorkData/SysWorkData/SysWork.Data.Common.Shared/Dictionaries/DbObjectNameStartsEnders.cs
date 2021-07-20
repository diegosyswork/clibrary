using System.Collections.Generic;
using SysWork.Data.Common.ValueObjects;

/// <summary>
/// 
/// </summary>
namespace SysWork.Data.Common.Dictionaries
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
        public static readonly Dictionary<EDatabaseEngine, Dictionary<string, string>> StartEndCharacters = new Dictionary<EDatabaseEngine, Dictionary<string, string>>()
        {
                { EDatabaseEngine.MSSqlServer, new Dictionary<string, string>() { { "starts", "[" }, { "ends", "]" } } },
                { EDatabaseEngine.OleDb, new Dictionary<string, string>() { { "starts", "[" }, { "ends", "]" } } } ,
                { EDatabaseEngine.SqLite, new Dictionary<string, string>() { { "starts", "[" }, { "ends", "]" } }},
                { EDatabaseEngine.MySql, new Dictionary<string, string>() { { "starts", "`" }, { "ends", "`" } } }
        };
    }
}
