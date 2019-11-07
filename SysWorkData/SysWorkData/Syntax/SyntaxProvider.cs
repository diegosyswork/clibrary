using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common;

namespace SysWork.Data.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public class SyntaxProvider
    {
        private EDataBaseEngine _dataBaseEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxProvider"/> class.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        public SyntaxProvider(EDataBaseEngine dataBaseEngine)
        {
            _dataBaseEngine = dataBaseEngine;
        }

        private readonly Dictionary<EDataBaseEngine, Dictionary<string, string>> StartEndCharacters = new Dictionary<EDataBaseEngine, Dictionary<string, string>>()
        {
                { EDataBaseEngine.MSSqlServer, new Dictionary<string, string>() { { "starts", "[" }, { "ends", "]" } } },
                { EDataBaseEngine.OleDb, new Dictionary<string, string>() { { "starts", "[" }, { "ends", "]" } } } ,
                { EDataBaseEngine.SqLite, new Dictionary<string, string>() { { "starts", "[" }, { "ends", "]" } }},
                { EDataBaseEngine.MySql, new Dictionary<string, string>() { { "starts", "`" }, { "ends", "`" } } }
        };

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <returns></returns>
        public string GetSecureTableName(string TableName)
        {
            string secureTableName = TableName;
            StartEndCharacters.TryGetValue(_dataBaseEngine, out Dictionary<string, string> chars);
            chars.TryGetValue("starts", out string startsWith);
            chars.TryGetValue("ends", out string endsWith);

            secureTableName = startsWith + secureTableName.Replace(startsWith, "").Replace(endsWith, "") + endsWith;

            /*
            if (_dataBaseEngine == EDataBaseEngine.MySql)
                secureTableName = "`" + TableName.Replace("`", "") + "`";
            else
                secureTableName = "[" + TableName.Replace("[", "").Replace("]", "") + "]";
            */

            return secureTableName;
        }
        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="prefixTable">The prefix table.</param>
        /// <returns></returns>
        public string GetSecureColumnName(string ColumnName, string prefixTable = null)
        {
            string secureColumnName = ColumnName;

            StartEndCharacters.TryGetValue(_dataBaseEngine, out Dictionary<string, string> chars);
            chars.TryGetValue("starts", out string startsWith);
            chars.TryGetValue("ends", out string endsWith);
            secureColumnName = startsWith + secureColumnName.Replace(startsWith, "").Replace(endsWith, "") + endsWith;

            if (prefixTable != null)
                secureColumnName+=  GetSecureTableName(prefixTable) + ".";


            return secureColumnName;
        }

        /// <summary>
        /// In the case that the column name contains a start or end character such as "[", "]", "` ", 
        /// it removes them. Use when calling GetSchema for example
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public string RemoveStartersAndEndersColumnName(string columnName)
        {
            string originalColumnName = columnName;

            StartEndCharacters.TryGetValue(_dataBaseEngine, out Dictionary<string, string> chars);
            chars.TryGetValue("starts", out string startsWith);
            chars.TryGetValue("ends", out string endsWith);
            originalColumnName = originalColumnName.Replace(startsWith, "").Replace(endsWith, "") ;

            return originalColumnName;
        }

        /// <summary>
        /// Gets the sub query get identity.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The database engine is not supported by this method (GetSubQueryGetIdentity())</exception>
        public string GetSubQueryGetIdentity()
        {
            if (_dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return " SELECT SCOPE_IDENTITY()";
            else if (_dataBaseEngine == EDataBaseEngine.OleDb)
                // Hay que Ejecutar por separado otra consulta solicitando @@Identity
                return "";
            else if (_dataBaseEngine == EDataBaseEngine.SqLite)
                return " ; SELECT last_insert_rowid()";
            else if (_dataBaseEngine == EDataBaseEngine.MySql)
                return " ; SELECT LAST_INSERT_ID()";

            throw new ArgumentException("The database engine is not supported by this method (GetSubQueryGetIdentity())");
        }

        /// <summary>
        /// The character equivalence
        /// </summary>
        private readonly Dictionary<string, string> CharacterEquivalence = new Dictionary<string, string>()
        {
            { " ",  "____01____"},
            { "-",  "____02____"},
            { ".",  "____03____"},
            { "/",  "____04____"},
            { @"\", "____05____"}
        };

        /// <summary>
        /// Secures the name for parameter.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// </returns>
        public string SecureNameForParameter(string fieldName)
        {
            string newName = fieldName;
            foreach (var item in CharacterEquivalence)
            {
                newName = newName.Replace(item.Key, item.Value);
            }
            return newName;
        }
        /// <summary>
        /// Secures the parameter name to field.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public string SecureParameterNameToField(string parameterName)
        {
            string newName = parameterName;
            foreach (var item in CharacterEquivalence)
            {
                newName = newName.Replace(item.Value, item.Key);
            }
            return newName;
        }
    }
}
