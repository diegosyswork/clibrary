using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWork.Data.Common;

namespace SysWork.Data.Syntax
{
    public class SyntaxProvider
    {
        private EDataBaseEngine _dataBaseEngine;

        public SyntaxProvider(EDataBaseEngine dataBaseEngine)
        {
            _dataBaseEngine = dataBaseEngine;
        }

        public string GetTableName(string TableName)
        {
            string ret = "";

            if (_dataBaseEngine == EDataBaseEngine.MySql)
                ret = "`" + TableName.Replace("`", "") + "`";
            else
                ret = "[" + TableName.Replace("[", "").Replace("]", "") + "]";


            return ret;
        }
        public string GetColumnName(string ColumnName, string prefixTable = null)
        {
            string ret = "";

            if (_dataBaseEngine == EDataBaseEngine.MySql)
                ret = "`" + ColumnName.Replace("`", "") + "`";
            else
                ret = "[" + ColumnName.Replace("[", "").Replace("]", "") + "]";

            if (prefixTable != null)
                ret+=  GetTableName(prefixTable) + ".";


            return ret;
        }

        /// <summary>
        /// 
        /// Dependiendo del motor de base de datos utilizado, devuelve la subconsulta que debe
        /// realizarse para obtener la identidad insertada
        /// 
        /// </summary>
        /// <returns></returns>
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

            throw new ArgumentException("Es necesario revisar el metodo GetSubQueryScalar()");
        }



    }
}
