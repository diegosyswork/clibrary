using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.DaoModel.CodeWriter
{
    /// <summary>
    /// Fecha: 30/07/2017
    /// Author: Diego Martinez
    /// 
    /// Se encarga de generar una clase tipo DAO, ademas genera tantos metodos 
    /// como UNIQUE KEY tenga la Tabla.
    /// 
    /// </summary>
    
    public class DaoClassFromDb
    {
        private string _connectionString;

        private string _nameSpace;
        private string _entityName;
        private string _className;

        private string _dbTableName;

        private string _dataSource;
        private string _initialCatalog;


        public DaoClassFromDb(string ConnectionString,string EntityName,string NameSpace,string DbTableName)
        {
            _connectionString = ConnectionString;
            _nameSpace = NameSpace;
            _entityName = EntityName;
            _className = "Dao" + EntityName;
            _dbTableName = DbTableName;

            SqlConnectionStringBuilder s = new SqlConnectionStringBuilder(_connectionString);
            _dataSource = s.DataSource;
            _initialCatalog = s.InitialCatalog;
        }
        private string GetTextClass()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(CodeWriterUtil.AddUsing("System"));
            builder.AppendLine(CodeWriterUtil.AddUsing("System.Collections.Generic"));
            builder.AppendLine(CodeWriterUtil.AddUsing("System.Linq"));
            builder.AppendLine(CodeWriterUtil.AddUsing("System.Text"));
            builder.AppendLine(CodeWriterUtil.AddUsing("System.Threading.Tasks"));
            builder.AppendLine(CodeWriterUtil.AddUsing("SysWork.Data.DaoModel.Attributes"));
            builder.AppendLine(CodeWriterUtil.AddUsing(_nameSpace + ".Entities"));
            builder.AppendLine(CodeWriterUtil.StartNamespace(_nameSpace + ".Daos"));
            builder.AppendLine(CodeWriterUtil.StartClass(_className, string.Format("BaseDao<{0}>", _entityName)));
            builder.AppendLine(AddSummary());
            builder.AppendLine(AddConstructor());
            builder.AppendLine(AddMethodsGetByUniquesKeys());
            builder.AppendLine(CodeWriterUtil.EndClass());
            builder.AppendLine(CodeWriterUtil.EndNamespace());

            return builder.ToString();
        }
        /// <summary>
        ///  Verifica todas las claves UNIQUE que contenga la tabla y en base a eso genera un
        ///  metodo que devuelve una entidad
        /// </summary>
        /// <returns></returns>
        private string AddMethodsGetByUniquesKeys()
        {
            string ret = "";

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string sql = "";
                SqlCommand sqlCommand;

                sql = string.Format("SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'UNIQUE' AND TABLE_NAME = '{0}'", _dbTableName);
                sqlCommand = new SqlCommand(sql, sqlConnection);

                SqlDataReader readerConstraint = sqlCommand.ExecuteReader();
                List<String> listaConstraints = new List<string>();

                while (readerConstraint.Read())
                    listaConstraints.Add(readerConstraint.GetString(0));

                readerConstraint.Close();


                foreach (var constraint in listaConstraints)
                {
                    string columnas = "";
                    sql = string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME = '{0}' AND TABLE_NAME = '{1}' ORDER BY ORDINAL_POSITION", constraint,_dbTableName);
                    sqlCommand = new SqlCommand(sql, sqlConnection);
                    SqlDataReader readerColumnas = sqlCommand.ExecuteReader();
                    while (readerColumnas.Read())
                    {
                        columnas += readerColumnas.GetString(0) + ",";
                    }
                    readerColumnas.Close();

                    columnas = columnas.Substring(0, columnas.Length - 1);

                    sql = string.Format("SELECT {0} FROM [{1}]", columnas, _dbTableName);
                    sqlCommand = new SqlCommand(sql, sqlConnection);

                    DataTable schema = sqlCommand.ExecuteReader(CommandBehavior.KeyInfo).GetSchemaTable();
                    ret += CreateMethodCodeGetByUnique(schema, columnas) + Environment.NewLine;
                }

                sqlConnection.Close();

            }

            return ret;
        }

        private string CreateMethodCodeGetByUnique(DataTable schema,string columns)
        {
            string ret = "";

            ret += string.Format("\t\tpublic {0} {1} ({2})" + Environment.NewLine, _entityName, GetMethodNameGetByUnique(schema,columns), GetMethodParametersGetByUnique(schema,columns));
            ret += "\t\t{" + Environment.NewLine;
            ret += string.Format("\t\t\t {0} {1} = null;" + Environment.NewLine, _entityName, _entityName.ToLower());
            ret += "\t\t\t var resultado = GetListByLambdaExpressionFilter(entity => " ;

            string lambdaBody = "";
            List<string> listaColumnas = columns.Split(',').ToList();

            foreach (DataRow dataRow in schema.Rows)
            {
                string columnName = dataRow["ColumnName"].ToString();

                if (listaColumnas.Contains(columnName))
                    lambdaBody += string.Format(" && (entity.{0} == {1})", columnName, columnName);
            }

            lambdaBody = lambdaBody.Substring(3);
            ret += lambdaBody;

            ret += ");" + Environment.NewLine;

            ret += "\t\t\t if (resultado != null && resultado.Count > 0)" + Environment.NewLine;
            ret += string.Format("\t\t\t\t {0} = resultado[0];",_entityName.ToLower());
            ret += Environment.NewLine;
            ret += string.Format("\t\t\t return {0};", _entityName.ToLower()) + Environment.NewLine;

            ret += "\t\t}";

            return ret;
        }

        /// <summary>
        /// En base a las columnas que tenga la restriccion, genera el nombre del metodo
        /// con prefijo GetBy + El nombre de las columnas en estilo CamelCase
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public string GetMethodNameGetByUnique(DataTable schema, string columns)
        {
            string ret = "GetBy";
            List<string> listaColumnas = columns.Split(',').ToList();
            foreach (DataRow dataRow in schema.Rows)
            {
                string columnName = dataRow["ColumnName"].ToString();

                if (listaColumnas.Contains(columnName))
                    ret += columnName.Substring(0, 1).ToUpper() + columnName.Substring(1);
            }
            return ret;
        }
        /// <summary>
        /// Crea la lista de parametros que tiene que recibir el metodo GetBy
        /// con el tipo de Datos Correspondiente
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public string GetMethodParametersGetByUnique(DataTable schema, string columns)
        {
            string ret = "";

            List<string> listaColumnas = columns.Split(',').ToList();

            foreach (DataRow dataRow in schema.Rows)
            {
                bool allowDbNull = (bool)dataRow["AllowDbNull"];
                string columnName = dataRow["ColumnName"].ToString();
                string columnDataType = dataRow["DataType"].ToString();

                string dataType = CodeWriterUtil.GetDataType(columnDataType, allowDbNull);

                if (listaColumnas.Contains(columnName))
                    ret += dataType + " " + columnName + ",";
            }

            ret = ret.Substring(0, ret.Length - 1);

            return ret;
        }

        private string AddConstructor()
        {
            string ret = "";

            ret += "\t\t public Dao" + _entityName + "(string connectionString) : base(connectionString)" + Environment.NewLine;
            ret += "\t\t {" + Environment.NewLine;
            ret += "\t\t " + Environment.NewLine;
            ret += "\t\t }" + Environment.NewLine;

            return ret;
        }

        private string AddSummary()
        {
            string ret = "\t /// <summary>" + Environment.NewLine;
            ret += "\t/// **********************************************************************" + Environment.NewLine;
            ret += "\t/// " + Environment.NewLine;
            ret += "\t/// Esta clase fue generada automaticamente por la clase DaoClassFromDb" + Environment.NewLine;
            ret += "\t/// " + Environment.NewLine;
            ret += string.Format("\t/// Fecha: {0}", string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now)) + Environment.NewLine;
            ret += "\t/// " + Environment.NewLine;
            ret += "\t/// **********************************************************************" + Environment.NewLine;
            ret += "\t/// </summary>" + Environment.NewLine;

            return ret;
        }

        public override string ToString()
        {
            return GetTextClass();
        }
    }
}
