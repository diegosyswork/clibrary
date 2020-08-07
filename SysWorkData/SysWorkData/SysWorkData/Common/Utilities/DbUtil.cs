using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.Utilities
{
    /// <summary>
    /// Database Utilities
    /// </summary>
    /// <remarks>
    /// Database Utilities. All methods are "multi" Database Engine
    /// </remarks>
    public static class DbUtil
    {
        /// <summary>
        /// Given a connectionString and the name of a table determines if the table exists. The default DatabaseEngine is MSSqlServer
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>
        ///     <c>true</c> if the table exists, or <c>false</c> if not.
        /// </returns>
        public static bool ExistsTable(string connectionString, string tableName)
        {
            return ExistsTable(EDataBaseEngine.MSSqlServer, connectionString, tableName);
        }

        /// <summary>
        /// Given a connectionString, the name of a table and DatabaseEngine, determines if the table exists. 
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static bool ExistsTable(EDataBaseEngine dataBaseEngine, string connectionString, string tableName)
        {
            bool exists = false;

            using (DbConnection dbConnection = StaticDbObjectProvider.GetDbConnection(dataBaseEngine, connectionString))
            {
                dbConnection.Open();
                if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                    exists = dbConnection.GetSchema("Tables", new string[4] { null, null, tableName, "BASE TABLE" }).Rows.Count > 0;
                else if (dataBaseEngine == EDataBaseEngine.MySql)
                    exists = dbConnection.GetSchema("Tables", new string[4] { null, null, tableName, "BASE TABLE" }).Rows.Count > 0;
                else if (dataBaseEngine == EDataBaseEngine.OleDb)
                    exists = dbConnection.GetSchema("Tables", new string[4] { null, null, tableName, "TABLE" }).Rows.Count > 0;
                else if (dataBaseEngine == EDataBaseEngine.SqLite)
                    exists = dbConnection.GetSchema("Tables", new string[4] { null, null, tableName, "TABLE" }).Rows.Count > 0;
                else
                    throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");

                dbConnection.Close();
            }
            return exists;
        }

        /// <summary>
        /// Given a connectionString return a list of tables in the Database. The default DatabaseEngine is MSSqlServer
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>List of tables in string format.</returns>
        public static List<string> GetListTables(string connectionString)
        {
            return GetListTables(EDataBaseEngine.MSSqlServer, connectionString);
        }

        /// <summary>
        /// Given a connectionString and DatabaseEngine, return a list of tables in the Database.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static List<string> GetListTables(EDataBaseEngine dataBaseEngine, string connectionString)
        {
            using (DbConnection connection = StaticDbObjectProvider.GetDbConnection(dataBaseEngine,connectionString))
            {
                connection.Open();
                DataTable schema = null;

                if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                    schema = connection.GetSchema("Tables", new string[] { null, null, null, "BASE TABLE" });
                else if (dataBaseEngine == EDataBaseEngine.MySql)
                    schema = connection.GetSchema("Tables", new string[] { null, null, null, "BASE TABLE" });
                else if (dataBaseEngine == EDataBaseEngine.OleDb)
                    schema=connection.GetSchema("Tables", new string[] { null, null, null, "TABLE" });
                else if (dataBaseEngine == EDataBaseEngine.SqLite)
                    schema = connection.GetSchema("Tables", new string[] { null, null, null, "TABLE" });
                else
                    throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");

                List<string> tables = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    tables.Add(row[2].ToString());
                }
                connection.Close();

                return tables;
            }
        }

        /// <summary>
        /// Given a connectionString return a list of views in the Database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static List<string> GetListViews(string connectionString)
        {
            return GetListViews(EDataBaseEngine.MSSqlServer, connectionString);
        }

        /// <summary>
        /// Given a connectionString and DatabaseEngine, return a list of views in the Database.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static List<string> GetListViews(EDataBaseEngine dataBaseEngine, string connectionString)
        {
            using (DbConnection connection = StaticDbObjectProvider.GetDbConnection(dataBaseEngine, connectionString))
            {
                connection.Open();
                DataTable schema = null;

                if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                    schema = connection.GetSchema("Tables", new string[] { null, null, null, "VIEW" });
                else if (dataBaseEngine == EDataBaseEngine.MySql)
                    schema = connection.GetSchema("Views", new string[] { null, null, null, null });
                else if (dataBaseEngine == EDataBaseEngine.OleDb)
                    schema = connection.GetSchema("Views", new string[] { null, null, null });
                else if (dataBaseEngine == EDataBaseEngine.SqLite)
                    schema = connection.GetSchema("Views", new string[] { null, null, null, null });
                else
                    throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");

                List<string> views = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    views.Add(row[2].ToString());
                }
                connection.Close();

                return views;
            }
        }
        /// <summary>
        /// Given an connectionString, an table name and a column name determines if the column exists. The default DatabaseEngine is MSSqlServer</summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool ExistsColumn(string connectionString, string tableName, string columnName)
        {
            return ExistsColumn(EDataBaseEngine.MSSqlServer, connectionString, tableName, columnName);
        }

        /// <summary>
        /// Given an connectionString, an datase engine, an table name and a column name determines if the column exists.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static bool ExistsColumn(EDataBaseEngine dataBaseEngine, string connectionString, string tableName, string columnName)
        {
            bool exists = false;
            using (DbConnection dbConnection = StaticDbObjectProvider.GetDbConnection(dataBaseEngine, connectionString))
            {
                dbConnection.Open();
                DataTable dtColumns = null;

                if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                    dtColumns = dbConnection.GetSchema("Columns", new[] { null, null, tableName, null });
                else if (dataBaseEngine == EDataBaseEngine.MySql)
                    dtColumns = dbConnection.GetSchema("Columns", new[] { null, null, tableName, null });
                else if (dataBaseEngine == EDataBaseEngine.OleDb)
                    dtColumns = dbConnection.GetSchema("Columns", new[] { null, null, tableName, null });
                else if (dataBaseEngine == EDataBaseEngine.SqLite)
                    dtColumns = dbConnection.GetSchema("Columns", new[] { null, tableName,null});
                else
                    throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");

                // For compatibility with SQLite filter the TABLE_NAME, because the restriction does't work.
                exists = dtColumns.Select(String.Format("COLUMN_NAME = '{0}' AND TABLE_NAME = '{1}'", columnName, tableName)).Length != 0;

                dbConnection.Close();
            }
            return exists;
        }

        /// <summary>
        /// Execute a list of sentences separated by the GO clause.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns></returns>
        public static bool ExecuteBatchNonQuery(string query, string ConnectionString)
        {
            return ExecuteBatchNonQuery(EDataBaseEngine.MSSqlServer, query, ConnectionString);
        }

        /// <summary>
        /// Execute a list of sentences separated by the GO clause.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="query">The query.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns></returns>
        public static bool ExecuteBatchNonQuery(EDataBaseEngine dataBaseEngine, string query, string ConnectionString)
        {
            bool result = false;

            result = ExecuteBatchNonQuery(dataBaseEngine, query, StaticDbObjectProvider.GetIDbConnection(dataBaseEngine, ConnectionString));

            return result;
        }
        /// <summary>
        /// Execute a list of sentences separated by the GO clause.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="query">The query.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public static bool ExecuteBatchNonQuery(EDataBaseEngine dataBaseEngine, string query, IDbConnection connection)
        {
            string sqlBatch = string.Empty;

            using (IDbCommand dbCommand = connection.CreateCommand())
            {
                dbCommand.CommandText = string.Empty;
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    query += "\nGO";

                    foreach (string line in query.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (line.ToUpperInvariant().Trim() == "GO")
                        {
                            if (!string.IsNullOrEmpty(sqlBatch.Trim()))
                            {
                                dbCommand.CommandText = sqlBatch;
                                dbCommand.ExecuteNonQuery();
                            }
                            sqlBatch = string.Empty;
                        }
                        else
                        {
                            sqlBatch += line + "\n";
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            return true;
        }

        /// <summary>
        /// Try to open an DbConnection with the ConnectionString provided. The DatabaseEngine is MSSqlServer.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static bool ConnectionSuccess(string connectionString)
        {
            return ConnectionSuccess(EDataBaseEngine.MSSqlServer, connectionString, out string errMessage);
        }

        /// <summary>
        /// Try to open an DbConnection with the ConnectionString provided. The DatabaseEngine is MSSqlServer.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="errMessage">The mensaje error.</param>
        /// <returns></returns>
        public static bool ConnectionSuccess(string connectionString, out string errMessage)
        {
            return ConnectionSuccess(EDataBaseEngine.MSSqlServer, connectionString, out errMessage);
        }

        /// <summary>
        /// Connections the success.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static bool ConnectionSuccess(EDataBaseEngine dataBaseEngine, string connectionString)
        {
            return ConnectionSuccess(dataBaseEngine, connectionString, out string errMessage);
        }


        /// <summary>
        /// Try opening a DbConnection with the ConnectionString provided. The databaseEngine must be set.  
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="errMessage">The mensaje error.</param>
        /// <returns></returns>
        public static bool ConnectionSuccess(EDataBaseEngine dataBaseEngine, string connectionString, out string errMessage)
        {
            errMessage = "";
            try
            {
                using (IDbConnection dbConnection = StaticDbObjectProvider.GetIDbConnection(dataBaseEngine, connectionString))
                {
                    dbConnection.Open();
                    dbConnection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                errMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// Determines whether is valid connection string  for MSSqlServer.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        ///   <c>true</c> if [is valid connection string] [the specified data base engine]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidConnectionString(string connectionString)
        {
            return IsValidConnectionString(EDataBaseEngine.MSSqlServer, connectionString, out string errMessage);
        }

        /// <summary>
        /// Determines whether is valid connection string  for MSSqlServer.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if [is valid connection string] [the specified data base engine]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidConnectionString(string connectionString, out string errMessage)
        {
            return IsValidConnectionString(EDataBaseEngine.MSSqlServer, connectionString, out errMessage);
        }


        /// <summary>
        /// Determines whether is valid connection string  for the specified data base engine.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="errMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if [is valid connection string] [the specified data base engine]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidConnectionString(EDataBaseEngine dataBaseEngine, string connectionString, out string errMessage)
        {
            bool valid = true;
            errMessage = "";
            try
            {
                switch (dataBaseEngine)
                {
                    case EDataBaseEngine.MSSqlServer:
                        var sbSql = new SqlConnectionStringBuilder(connectionString);
                        break;
                    case EDataBaseEngine.SqLite:
                        var sbSqlite = new SQLiteConnectionStringBuilder(connectionString);
                        break;
                    case EDataBaseEngine.MySql:
                        var sbMySql = new MySqlConnectionStringBuilder(connectionString);
                        break;
                    case EDataBaseEngine.OleDb:
                        var sbOleDb = new OleDbConnectionStringBuilder(connectionString);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
                }
            }
            catch (Exception e)
            {
                valid = false;
                errMessage = e.Message;
            }

            return valid;
        }

        /// <summary>
        /// Converts the command paramaters to literal values.
        /// </summary>
        /// <param name="dbCommand">The database command.</param>
        /// <returns></returns>
        public static string ConvertCommandParamatersToLiteralValues(IDbCommand dbCommand)
        {
            string query;
            try
            {
                query = dbCommand.CommandText;

                foreach (IDataParameter prm in dbCommand.Parameters)
                {
                    switch (prm.DbType)
                    {
                        case DbType.Boolean:
                            int boolToInt = (bool)prm.Value ? 1 : 0;
                            query = query.Replace(prm.ParameterName, string.Format("{0}", (bool)prm.Value ? 1 : 0));
                            break;
                        case DbType.String:
                            if (prm.Value == DBNull.Value)
                                query = query.Replace(prm.ParameterName, string.Format("{0}", "NULL"));
                            else
                                query = query.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
                            break;
                        case DbType.StringFixedLength:
                            if (prm.Value == DBNull.Value)
                                query = query.Replace(prm.ParameterName, string.Format("{0}", "NULL"));
                            else
                                query = query.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
                            break;
                        case DbType.DateTime:
                            if (prm.Value == DBNull.Value)
                                query = query.Replace(prm.ParameterName, string.Format("{0}", "NULL"));
                            else
                                query = query.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
                            break;
                        default:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value == DBNull.Value ? "NULL" : prm.Value));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                query = "ERROR in convertCommandParamatersToLiteralValues : " + e.Message;
            }

            return query;
        }

        /// <summary>
        /// Adds the prefix table name to a field list.
        /// </summary>
        /// <param name="fieldList">The field list.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static string AddPrefixTableNameToFieldList(string fieldList, string tableName)
        {
            string[] splitList = fieldList.Split(',');

            for (int pos = 0; pos < splitList.Length; pos++)
            {
                splitList[pos] = tableName + "." + splitList[pos];
            }

            return string.Join(",", splitList); ;
        }

        /// <summary>
        /// Converts an generic list T to datatable. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns>
        /// </returns>
        public static DataTable ConvertToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }


        /// <summary>
        /// Verify the existence of a ConnectionString in app Configuration File.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns></returns>
        public static bool ExistsConnectionStringInConfig(string connectionStringName)
        {
            String conectionString = "";
            try
            {
                conectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Try to Parse from multiple Types to Long.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static long ParseToLong(object result)
        {
            if (result.GetType() == typeof(System.Int64))
                return (long)result;
            if (result.GetType() == typeof(System.Int32))
                return (long)(Int32)result;
            else if (result.GetType() == typeof(System.Decimal))
                return (long)(Decimal)result;
            else
                return long.Parse(result.ToString());
        }

    }
}
