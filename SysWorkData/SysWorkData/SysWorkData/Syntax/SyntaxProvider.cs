using System;
using System.Collections.Generic;
using SysWork.Data.Common;
using SysWork.Data.Common.Dictionarys;

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


        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <returns></returns>
        public string GetSecureTableName(string TableName)
        {
            string secureTableName = TableName;
            DbObjectNameStartsEnders.StartEndCharacters.TryGetValue(_dataBaseEngine, out Dictionary<string, string> chars);
            chars.TryGetValue("starts", out string startsWith);
            chars.TryGetValue("ends", out string endsWith);

            secureTableName = startsWith + secureTableName.Replace(startsWith, "").Replace(endsWith, "") + endsWith;

            return secureTableName;
        }
        /// <summary>
        /// Gets an secure name of view, if this contains special characters adds brackets 
        /// or equivalent in the databaseEngine.
        /// </summary>
        /// <param name="ViewName">Name of the view.</param>
        /// <returns></returns>
        public string GetSecureViewName(string ViewName)
        {
            string secureTableName = ViewName;
            DbObjectNameStartsEnders.StartEndCharacters.TryGetValue(_dataBaseEngine, out Dictionary<string, string> chars);
            chars.TryGetValue("starts", out string startsWith);
            chars.TryGetValue("ends", out string endsWith);

            secureTableName = startsWith + secureTableName.Replace(startsWith, "").Replace(endsWith, "") + endsWith;

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

            DbObjectNameStartsEnders.StartEndCharacters.TryGetValue(_dataBaseEngine, out Dictionary<string, string> chars);
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

            DbObjectNameStartsEnders.StartEndCharacters.TryGetValue(_dataBaseEngine, out Dictionary<string, string> chars);
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
        /// Secures the name for parameter.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>
        /// </returns>
        public string SecureNameForParameter(string fieldName)
        {
            string newName = fieldName;
            foreach (var item in DbObjectNameCharReplacerDictionary.CharacterEquivalence)
            {
                newName = newName.Replace(item.Key, item.Value);
            }
            return newName;
        }
        /// <summary>
        /// Secures the parameter name to field.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// </returns>
        public string SecureParameterNameToField(string parameterName)
        {
            string newName = parameterName;
            foreach (var item in DbObjectNameCharReplacerDictionary.CharacterEquivalence)
            {
                newName = newName.Replace(item.Value, item.Key);
            }
            return newName;
        }
    }
}
