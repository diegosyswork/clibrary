using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Syntax;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.GenericRepository.CodeWriter
{
    /// <summary>
    /// Write a Repository Class
    /// </summary>
    public class RepositoryClassFromTable
    {
        private string _connectionString;
        private EDataBaseEngine _databaseEngine;

        private string _nameSpace;
        private string _entityName;
        private string _className;

        private string _dbTableName;
        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryClassFromTable"/> class. 
        /// The databaseEngine is MSSqlServer
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="EntityName">Name of the entity.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="DbTableName">Name of the database table.</param>
        public RepositoryClassFromTable(string ConnectionString, string EntityName, string NameSpace, string DbTableName)
        {
            RepositoryClassFromDbConstructorResolver(EDataBaseEngine.MSSqlServer, ConnectionString, EntityName, NameSpace, DbTableName);

        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryClassFromTable"/> class.
        /// </summary>
        /// <param name="databaseEngine">The database engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="EntityName">Name of the entity.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="DbTableName">Name of the database table.</param>
        public RepositoryClassFromTable(EDataBaseEngine databaseEngine, string ConnectionString, string EntityName, string NameSpace, string DbTableName)
        {
            RepositoryClassFromDbConstructorResolver(databaseEngine, ConnectionString, EntityName, NameSpace, DbTableName);
        }

        private void RepositoryClassFromDbConstructorResolver(EDataBaseEngine databaseEngine, string ConnectionString, string EntityName, string NameSpace, string DbTableName)
        {
            _connectionString = ConnectionString;
            _databaseEngine = databaseEngine;
            _nameSpace = NameSpace;
            _entityName = EntityName;
            _className = EntityName + "Repository";
            _dbTableName = DbTableName;
            _syntaxProvider = new SyntaxProvider(_databaseEngine);
        }

        private string GetTextClass()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(CodeWriterHelper.AddUsing("System"));
            builder.AppendLine(CodeWriterHelper.AddUsing("System.Collections.Generic"));
            builder.AppendLine(CodeWriterHelper.AddUsing("System.Linq"));
            builder.AppendLine(CodeWriterHelper.AddUsing("System.Text"));
            //builder.AppendLine(CodeWriterHelper.AddUsing("System.Threading.Tasks"));
            builder.AppendLine(CodeWriterHelper.AddUsing("SysWork.Data.Common"));
            builder.AppendLine(CodeWriterHelper.AddUsing("SysWork.Data.GenericRepository"));
            builder.AppendLine(CodeWriterHelper.AddUsing("SysWork.Data.Common.Attributes"));
            builder.AppendLine(CodeWriterHelper.AddUsing(_nameSpace + ".Entities"));
            builder.AppendLine(CodeWriterHelper.StartNamespace(_nameSpace + ".Repositories"));
            builder.AppendLine(AddSummary());
            builder.AppendLine(CodeWriterHelper.StartClass(_className, string.Format("BaseRepository<{0}>", _entityName)));
            builder.AppendLine(AddConstructor());
            builder.AppendLine(AddMethodsGetByUniquesKeys());
            builder.AppendLine(CodeWriterHelper.EndClass());
            builder.AppendLine(CodeWriterHelper.EndNamespace());

            return builder.ToString();
        }
        /// <summary>
        /// Verify all UNIQUE KEYS in the table, and create a method to return an Entity.
        /// </summary>
        /// <returns>
        /// </returns>
        private string AddMethodsGetByUniquesKeys()
        {
            string ret = "";
            foreach (var UK in GetListUniqueKeys())
            {
                using (IDbConnection conn = StaticDbObjectProvider.GetIDbConnection(_databaseEngine, _connectionString))
                {
                    conn.Open();
                    string columnList = GetListColumnsUnique(UK);
                    string cmdText = string.Format("SELECT {0} FROM {1}", GetListColumnsUnique(UK), _syntaxProvider.GetSecureTableName(_dbTableName));
                    var dbCommand = conn.CreateCommand();
                    dbCommand.CommandText = cmdText;
                    dbCommand.Connection = conn;

                    DataTable schema = dbCommand.ExecuteReader(CommandBehavior.KeyInfo).GetSchemaTable();
                    ret += CreateMethodCodeGetByUnique(schema, columnList) + Environment.NewLine;
                }
            }
            return ret;
        }

        private List<string> GetListUniqueKeys()
        {
            switch (_databaseEngine)
            {
                case EDataBaseEngine.MSSqlServer:
                    return GetListUniqueKeyMSSqlServer();
                case EDataBaseEngine.SqLite:
                    return GetListUniqueKeysSqlite();
                case EDataBaseEngine.OleDb:
                    return GetListUniqueKeysOleDb();
                case EDataBaseEngine.MySql:
                    return GetListUniqueKeysMySql();
                default:
                    throw new ArgumentOutOfRangeException("The databaseEngine is not supported by this method GetListUniques()");
            }
        }
        private string GetListColumnsUnique(string uniqueKey)
        {
            switch (_databaseEngine)
            {
                case EDataBaseEngine.MSSqlServer:
                    return GetListColumnsUniqueMSSqlServer(uniqueKey);
                case EDataBaseEngine.SqLite:
                    return GetListColumnsUniqueSqlite(uniqueKey);
                case EDataBaseEngine.OleDb:
                    return GetListColumnsUniqueOleDb(uniqueKey);
                case EDataBaseEngine.MySql:
                    return GetListColumnsUniqueMySql(uniqueKey);
                default:
                    throw new ArgumentOutOfRangeException("The databaseEngine is not supported by this method GetListUniques()");
            }
        }

        private List<string> GetListUniqueKeyMSSqlServer()
        {
            var listUnique = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string sql = "";
                SqlCommand sqlCommand;

                sql = string.Format("SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'UNIQUE' AND TABLE_NAME = '{0}'", _dbTableName);
                sqlCommand = new SqlCommand(sql, sqlConnection);

                SqlDataReader readerConstraint = sqlCommand.ExecuteReader();

                while (readerConstraint.Read())
                    listUnique.Add(readerConstraint.GetString(0));

                readerConstraint.Close();
            }
            return listUnique;
        }

        private string GetListColumnsUniqueMSSqlServer(string uniqueKey)
        {
            string columns = "";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME = '{0}' AND TABLE_NAME = '{1}' ORDER BY ORDINAL_POSITION", uniqueKey, _dbTableName);
                var cmd = new SqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    columns += _syntaxProvider.GetSecureColumnName(reader.GetString(0)) + ",";
                }
                reader.Close();

                columns = columns.Substring(0, columns.Length - 1);

            }
            return columns;
        }
        private List<string> GetListUniqueKeysOleDb()
        {
            var listUnique = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                DataTable Uks = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes, new Object[] { null, null, null, null, _dbTableName });
                foreach (DataRow row in Uks.Rows)
                {
                    if ((bool)row["UNIQUE"] && !((bool)row["PRIMARY_KEY"]))
                        if (!listUnique.Contains(row["INDEX_NAME"].ToString()))
                            listUnique.Add(row["INDEX_NAME"].ToString());
                }
            }
            return listUnique;
        }
        private string GetListColumnsUniqueOleDb(string uniqueKey)
        {
            string columns = "";
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                DataTable Uks = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes, new Object[] { null, null, null, null, _dbTableName });
                foreach (DataRow row in Uks.Rows)
                {
                    if (row["INDEX_NAME"].ToString().Equals(uniqueKey))
                        columns += _syntaxProvider.GetSecureColumnName(row["COLUMN_NAME"].ToString()) + ",";
                }
                columns = columns.Substring(0, columns.Length - 1);
            }
            return columns;
        }

        private List<string> GetListUniqueKeysMySql()
        {
            var listUnique = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                DataTable Uks = conn.GetSchema("INDEXES", new string[] { null, null, _dbTableName });
                foreach (DataRow row in Uks.Rows)
                {
                    if ((bool)row["UNIQUE"] && !((bool)row["PRIMARY"]))
                        if (!listUnique.Contains(row["INDEX_NAME"].ToString()))
                            listUnique.Add(row["INDEX_NAME"].ToString());
                }
            }
            return listUnique;
        }
        private string GetListColumnsUniqueMySql(string uniqueKey)
        {
            string columns = "";

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                DataTable schema = conn.GetSchema("INDEXCOLUMNS", new string[] { null, null, _dbTableName, uniqueKey, null });
                foreach (DataRow row in schema.Rows)
                {
                    columns += _syntaxProvider.GetSecureColumnName(row["COLUMN_NAME"].ToString()) + ",";
                }
                columns = columns.Substring(0, columns.Length - 1);
            }

            return columns;
        }


        private List<string> GetListUniqueKeysSqlite()
        {
            var listUnique = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var dbCommand = conn.CreateCommand();
                dbCommand.Connection = conn;
                dbCommand.CommandText = _syntaxProvider.GetQuerySelectTop0(_dbTableName);

                DataTable schema = dbCommand.ExecuteReader(CommandBehavior.KeyInfo).GetSchemaTable();
                foreach (DataRow row in schema.Rows)
                {
                    if (((bool)row["IsUnique"]) && !((bool)row["IsKey"]))
                        listUnique.Add("UQ_" + row["ColumnName"].ToString());
                }

            }
            return listUnique;
        }

        private string GetListColumnsUniqueSqlite(string uniqueKey)
        {
            string columns = "";
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var dbCommand = conn.CreateCommand();
                dbCommand.Connection = conn;
                dbCommand.CommandText = _syntaxProvider.GetQuerySelectTop0(_dbTableName);

                DataTable schema = dbCommand.ExecuteReader(CommandBehavior.KeyInfo).GetSchemaTable();
                foreach (DataRow row in schema.Rows)
                {
                    if (uniqueKey.Equals("UQ_" + row["ColumnName"].ToString()))
                        columns += _syntaxProvider.GetSecureColumnName(row["ColumnName"].ToString()) + ",";
                }
                columns = columns.Substring(0, columns.Length - 1);
            }
            return columns;
        }

        private string CreateMethodCodeGetByUnique(DataTable schema,string columns)
        {
            string ret = "";

            ret += string.Format("\t\tpublic {0} {1} ({2})" + Environment.NewLine, _entityName, GetMethodNameGetByUnique(schema,columns), GetMethodParametersGetByUnique(schema,columns));
            ret += "\t\t{" + Environment.NewLine;
            ret += "\t\t\t return GetByLambdaExpressionFilter(entity => " ;

            string lambdaBody = "";
            List<string> listaColumnas = columns.Split(',').ToList();
            for (int i = 0; i < listaColumnas.Count; i++)
                listaColumnas[i] = _syntaxProvider.RemoveStartersAndEndersColumnName(listaColumnas[i]);

            foreach (DataRow dataRow in schema.Rows)
            {
                string columnName = dataRow["ColumnName"].ToString();

                if (listaColumnas.Contains(columnName))
                    lambdaBody += string.Format(" && (entity.{0} == {1})", columnName, columnName);
            }

            lambdaBody = lambdaBody.Substring(3);
            ret += lambdaBody;

            ret += ");" + Environment.NewLine;

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
            for (int i = 0; i < listaColumnas.Count; i++)
                listaColumnas[i] = _syntaxProvider.RemoveStartersAndEndersColumnName(listaColumnas[i]);

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
            for (int i = 0; i < listaColumnas.Count; i++)
                listaColumnas[i] = _syntaxProvider.RemoveStartersAndEndersColumnName(listaColumnas[i]);

            foreach (DataRow dataRow in schema.Rows)
            {
                bool allowDbNull = (bool)dataRow["AllowDbNull"];
                string columnName = dataRow["ColumnName"].ToString();
                string columnDataType = dataRow["DataType"].ToString();

                string dataType = CodeWriterHelper.GetDataType(columnDataType, allowDbNull);

                if (listaColumnas.Contains(columnName))
                    ret += dataType + " " + columnName + ",";
            }

            ret = ret.Substring(0, ret.Length - 1);

            return ret;
        }

        private string AddConstructor()
        {
            string ret = "";

            ret += "\t\t public " + _entityName + "Repository (string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString,dataBaseEngine)" + Environment.NewLine;
            ret += "\t\t {" + Environment.NewLine;
            ret += "\t\t " + Environment.NewLine;
            ret += "\t\t }" + Environment.NewLine;

            return ret;
        }

        private string AddSummary()
        {

            string ret = "\t/// <summary>\r\n";
            ret += "\t/// This class was created automatically with the RepositoryClassFromDb class.\r\n";
            ret += "\t/// Inherited from GenericRepository which allows you to perform the following actions: \r\n";
            ret += "\t/// Add, \r\n";
            ret += "\t/// AddRange, \r\n";
            ret += "\t/// Update, \r\n";
            ret += "\t/// UpdateRange, \r\n";
            ret += "\t/// DeleteById, \r\n";
            ret += "\t/// DeleteAll, \r\n";
            ret += "\t/// DeleteByLambdaExpressionFilter, \r\n";
            ret += "\t/// DeleteByGenericWhereFilter, \r\n";
            ret += "\t/// GetById, \r\n";
            ret += "\t/// GetByLambdaExpressionFilter, \r\n";
            ret += "\t/// GetByGenericWhereFilter, \r\n";
            ret += "\t/// GetAll, \r\n";
            ret += "\t/// GetListByLambdaExpressionFilter, \r\n";
            ret += "\t/// GetDataTableByLambdaExpressionFilter, \r\n";
            ret += "\t/// GetListByGenericWhereFilter, \r\n";
            ret += "\t/// GetDataTableByGenericWhereFilter, \r\n";
            ret += "\t/// Find \r\n";
            ret += "\t/// Find \r\n";
            ret += "\t/// </summary>\r\n";

            return ret;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetTextClass();
        }
    }
}
