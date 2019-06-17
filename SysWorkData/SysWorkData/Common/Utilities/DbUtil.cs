using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.FormsGetParam;

namespace SysWork.Data.Common.Utilities
{
    public static class DbUtil
    {
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

                dbConnection.Close();
            }

            return exists;
        }

        public static bool ExistsTable(string connectionString, string tableName)
        {
            return ExistsTable(EDataBaseEngine.MSSqlServer, connectionString, tableName);
        }

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

                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    TableNames.Add(row[2].ToString());
                }
                connection.Close();

                return TableNames;
            }
        }
        public static List<string> GetListTables(string connectionString)
        {
            return GetListTables(EDataBaseEngine.MSSqlServer, connectionString);
        }

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
                    dtColumns = dbConnection.GetSchema("Columns", new[] { null, tableName, null });

                DataView dv = new DataView(dtColumns);

                dv.RowFilter = String.Format(" COLUMN_NAME = '{1}'",tableName,columnName); 

                exists = dv.Count>0;

                dbConnection.Close();
            }
            return exists;
        }
        public static bool ExistsColumn(string connectionString, string tableName, string columnName)
        {
            return ExistsColumn(EDataBaseEngine.MSSqlServer, connectionString, tableName, columnName);
        }

        public static bool ExecuteBatchNonQuery(string query, string ConnectionString)
        {
            return ExecuteBatchNonQuery(EDataBaseEngine.MSSqlServer, query, ConnectionString);
        }
        public static bool ExecuteBatchNonQuery(EDataBaseEngine dataBaseEngine, string query, string ConnectionString)
        {
            bool result = false;

            result = ExecuteBatchNonQuery(dataBaseEngine, query, StaticDbObjectProvider.GetIDbConnection(dataBaseEngine, ConnectionString));

            return result;
        }

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
        /// Verifica si una Connection se puede abrir correctamente, por default 
        /// el motor de base de datos es SqlServer
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="mensajeError"></param>
        /// <returns></returns>
        public static bool ConnectionSuccess(string connectionString, out string mensajeError)
        {
            return ConnectionSuccess(EDataBaseEngine.MSSqlServer, connectionString, out mensajeError);
        }

        /// <summary>
        /// Verifica si una Connection se puede abrir correctamente,  
        /// Debe especificarse el motor de base de datos.
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="mensajeError"></param>
        /// <returns></returns>
        public static bool ConnectionSuccess(EDataBaseEngine dataBaseEngine, string connectionString, out string mensajeError)
        {
            mensajeError = "";
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
                mensajeError = e.Message;
                return false;
            }
        }

        public static bool IsValidConnectionString(EDataBaseEngine dataBaseEngine, string connectionString, out string mensajeError)
        {
            bool valid = true;
            mensajeError = "";
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
                }
            }
            catch (Exception e)
            {
                valid = false;
                mensajeError = e.Message;
            }

            return valid;
        }

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
                            query = query.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
                            break;
                        case DbType.StringFixedLength:
                            query = query.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
                            break;
                        case DbType.DateTime:
                            query = query.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
                            break;
                        default:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                query = "ERROR EN convertCommandParamatersToLiteralValues : " + e.Message;
            }

            return query;
        }


        public static bool ExistsConnectionString(string connectionStringName)
        {
            String conectionString = "";
            try
            {
                conectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                return false;
            }
            return true;
        }

        public static string GetConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        public static string AddPrefixTableNameToFieldList(string fieldList, string tableName)
        {
            string[] splitList = fieldList.Split(',');

            for (int pos = 0; pos < splitList.Length; pos++)
            {
                splitList[pos] = tableName + "." + splitList[pos];
            }

            return string.Join(",", splitList); ;
        }


        public static bool VerifyMSSQLConnectionStringOrGetParams(string connectionStringName, string defaultDataSource = null, string defaultUserId = null, string defaultPassWord = null, string defaultInitialCatalog = null, string defaultConnectionString = null,bool encryptData = false)
        {
            SqlConnectionStringBuilder connectionSb = new SqlConnectionStringBuilder();
            bool userGotParameters = false;

            if (!ExistsConnectionString(connectionStringName))
            {
                connectionSb.DataSource = defaultDataSource ?? "LOCALHOST";
                connectionSb.UserID = defaultUserId ?? "SA";
                connectionSb.Password = defaultPassWord ?? "";
                connectionSb.InitialCatalog = defaultInitialCatalog ?? "master";
            }
            else
            {
                connectionSb.ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                if (encryptData)
                {
                    connectionSb.UserID = Decrypt(connectionSb.UserID);
                    connectionSb.Password = Decrypt(connectionSb.Password);
                    connectionSb.DataSource = Decrypt(connectionSb.DataSource);
                    connectionSb.InitialCatalog = Decrypt(connectionSb.InitialCatalog);
                }
            }

            bool hasConnectionSuccess = ConnectionSuccess(connectionSb.ConnectionString.ToString(), out string mensajeError);

            bool needConnectionParameters = (!hasConnectionSuccess);

            while (needConnectionParameters)
            {
                userGotParameters = true;

                FrmGetParamSQL frmGetParamSQL;
                frmGetParamSQL = new FrmGetParamSQL();

                frmGetParamSQL.Server = connectionSb.DataSource;
                frmGetParamSQL.InicioDeSesion = connectionSb.UserID;
                frmGetParamSQL.Password = connectionSb.Password;
                frmGetParamSQL.BaseDeDatos = connectionSb.InitialCatalog;
                frmGetParamSQL.ConnectionString = defaultConnectionString;

                frmGetParamSQL.MensajeError = "Ha ocurrido el siguiente error: \r\r" + mensajeError;

                frmGetParamSQL.ShowDialog();

                if (frmGetParamSQL.DialogResult == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(frmGetParamSQL.ConnectionString))
                    {
                        defaultConnectionString = frmGetParamSQL.ConnectionString;
                        connectionSb.ConnectionString = frmGetParamSQL.ConnectionString;

                    }
                    else
                    {
                        connectionSb.DataSource = frmGetParamSQL.Server;
                        connectionSb.UserID = frmGetParamSQL.InicioDeSesion;
                        connectionSb.Password = frmGetParamSQL.Password;
                        if (!string.IsNullOrEmpty(frmGetParamSQL.BaseDeDatos.Trim()))
                            connectionSb.InitialCatalog = frmGetParamSQL.BaseDeDatos;
                    }

                    hasConnectionSuccess = ConnectionSuccess(connectionSb.ConnectionString.ToString(), out mensajeError);
                }
                else
                {
                    hasConnectionSuccess = false;
                }

                needConnectionParameters = (!hasConnectionSuccess) && (frmGetParamSQL.DialogResult == DialogResult.OK);
            }


            if (!hasConnectionSuccess)
            {
                return false;
            }
            else
            {
                if (encryptData)
                {
                    connectionSb.UserID = Encrypt(connectionSb.UserID);
                    connectionSb.Password = Encrypt(connectionSb.Password);
                    connectionSb.DataSource = Encrypt(connectionSb.DataSource);
                    connectionSb.InitialCatalog = Encrypt(connectionSb.InitialCatalog);
                }

                if (!ExistsConnectionString(connectionStringName))
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(connectionStringName, connectionSb.ToString());
                    config.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);

                    config.Save(ConfigurationSaveMode.Modified, true);
                    ConfigurationManager.RefreshSection("connectionStrings");
                }
                else
                {
                    if (userGotParameters)
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                        config.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString = connectionSb.ToString();
                        config.Save(ConfigurationSaveMode.Modified, true);
                        ConfigurationManager.RefreshSection("connectionStrings");
                    }
                }
            }

            return true;
        }
        public static bool VerifyMySQLConnectionStringOrGetParams(string connectionStringName, string defaultServer = null, string defaultUserId = null, string defaultPassWord = null, string defaultDataBase = null, string defaultConnectionString = null,bool encryptData = false)
        {
            MySqlConnectionStringBuilder connectionSb = new MySqlConnectionStringBuilder();
            bool userGotParameters = false;

            if (!ExistsConnectionString(connectionStringName))
            {
                //ASIGNO DATOS DEFAULT
                connectionSb.Server = defaultServer ?? "localhost";
                connectionSb.UserID = defaultUserId ?? "root";
                connectionSb.Password = defaultPassWord ?? "root";
                connectionSb.Database = defaultDataBase ?? "";
            }
            else
            {
                connectionSb.ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                if (encryptData)
                {
                    connectionSb.UserID = Decrypt(connectionSb.UserID);
                    connectionSb.Password = Decrypt(connectionSb.Password);
                    connectionSb.Server = Decrypt(connectionSb.Server);

                    if (!string.IsNullOrEmpty(connectionSb.Database.Trim()))
                        connectionSb.Database = Decrypt(connectionSb.Database);
                }
            }

            bool hasConnectionSuccess = ConnectionSuccess(EDataBaseEngine.MySql, connectionSb.ConnectionString.ToString(), out string mensajeError);

            bool needConnectionParameters = (!hasConnectionSuccess);

            while (needConnectionParameters)
            {
                userGotParameters = true;

                FrmGetParamMySQL frmGetParamMySQL;
                frmGetParamMySQL = new FrmGetParamMySQL();

                frmGetParamMySQL.Server = connectionSb.Server;
                frmGetParamMySQL.InicioDeSesion = connectionSb.UserID;
                frmGetParamMySQL.Password = connectionSb.Password;
                frmGetParamMySQL.BaseDeDatos = connectionSb.Database;
                frmGetParamMySQL.ConnectionString = defaultConnectionString;

                frmGetParamMySQL.MensajeError = "Ha ocurrido el siguiente error: \r\r" + mensajeError;

                frmGetParamMySQL.ShowDialog();
                if (frmGetParamMySQL.DialogResult == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(frmGetParamMySQL.ConnectionString))
                    {
                        defaultConnectionString = frmGetParamMySQL.ConnectionString;
                        connectionSb.ConnectionString = frmGetParamMySQL.ConnectionString;
                    }
                    else
                    {
                        connectionSb.Server = frmGetParamMySQL.Server;
                        connectionSb.UserID = frmGetParamMySQL.InicioDeSesion;
                        connectionSb.Password = frmGetParamMySQL.Password;

                        if (!string.IsNullOrEmpty(frmGetParamMySQL.BaseDeDatos.Trim()))
                            connectionSb.Database = frmGetParamMySQL.BaseDeDatos;
                    }

                    hasConnectionSuccess = ConnectionSuccess(EDataBaseEngine.MySql, connectionSb.ConnectionString.ToString(), out mensajeError);
                }
                else
                {
                    hasConnectionSuccess = false;
                }

                needConnectionParameters = (!hasConnectionSuccess) && (frmGetParamMySQL.DialogResult == DialogResult.OK);
            }

            if (!hasConnectionSuccess)
            {
                return false;
            }
            else
            {

                if (encryptData)
                {
                    connectionSb.UserID = Encrypt(connectionSb.UserID);
                    connectionSb.Password = Encrypt(connectionSb.Password);
                    connectionSb.Server = Encrypt(connectionSb.Server);

                    if (!string.IsNullOrEmpty(connectionSb.Database.Trim()))
                        connectionSb.Database = Encrypt(connectionSb.Database);
                }

                if (!ExistsConnectionString(connectionStringName))
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(connectionStringName, connectionSb.ToString());
                    config.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);

                    config.Save(ConfigurationSaveMode.Modified, true);
                    ConfigurationManager.RefreshSection("connectionStrings");
                }
                else
                {
                    if (userGotParameters)
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                        config.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString = connectionSb.ToString();
                        config.Save(ConfigurationSaveMode.Modified, true);
                        ConfigurationManager.RefreshSection("connectionStrings");
                    }
                }
            }

            return true;
        }
        public static bool VerifySQLiteConnectionStringOrGetParams(string connectionStringName, string defaultConnectionString)
        {
            SQLiteConnectionStringBuilder connectionSb = new SQLiteConnectionStringBuilder();
            bool userGotParameters = false;

            if (!ExistsConnectionString(connectionStringName))
            {
                connectionSb.ConnectionString = defaultConnectionString ?? "";
            }
            else
            {
                connectionSb.ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            }

            string mensajeError = "";
            bool hasConnectionSuccess = File.Exists(connectionSb.DataSource);
            hasConnectionSuccess = hasConnectionSuccess && ConnectionSuccess(EDataBaseEngine.SqLite,connectionSb.ConnectionString.ToString(), out mensajeError);

            bool needConnectionParameters = (!hasConnectionSuccess);

            while (needConnectionParameters)
            {
                userGotParameters = true;

                FrmGetParamSQLite frmGetParamSQLite;
                frmGetParamSQLite = new FrmGetParamSQLite();
                frmGetParamSQLite.ConnectionString = defaultConnectionString;

                frmGetParamSQLite.MensajeError = "Ha ocurrido el siguiente error: \r\r" + mensajeError;

                frmGetParamSQLite.ShowDialog();

                if (frmGetParamSQLite.DialogResult == DialogResult.OK)
                {
                    defaultConnectionString = frmGetParamSQLite.ConnectionString;
                    connectionSb.ConnectionString = frmGetParamSQLite.ConnectionString;

                    hasConnectionSuccess = File.Exists(connectionSb.DataSource);
                    if (!hasConnectionSuccess) mensajeError = "El archivo no existe";

                    hasConnectionSuccess = hasConnectionSuccess && ConnectionSuccess(EDataBaseEngine.SqLite, connectionSb.ConnectionString.ToString(), out mensajeError);
                }
                else
                {
                    hasConnectionSuccess = false;
                }

                needConnectionParameters = (!hasConnectionSuccess) && (frmGetParamSQLite.DialogResult == DialogResult.OK);
            }

            if (!hasConnectionSuccess)
            {
                return false;
            }
            else
            {
                if (!ExistsConnectionString(connectionStringName))
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(connectionStringName, connectionSb.ToString());
                    config.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);

                    config.Save(ConfigurationSaveMode.Modified, true);
                    ConfigurationManager.RefreshSection("connectionStrings");
                }
                else
                {
                    if (userGotParameters)
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                        config.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString = connectionSb.ToString();
                        config.Save(ConfigurationSaveMode.Modified, true);
                        ConfigurationManager.RefreshSection("connectionStrings");
                    }
                }
            }

            return true;
        }
        public static bool VerifyOleDbConnectionStringOrGetParams(string connectionStringName, string defaultConnectionString)
        {
            OleDbConnectionStringBuilder connectionSb = new OleDbConnectionStringBuilder();
            bool userGotParameters = false;

            if (!ExistsConnectionString(connectionStringName))
            {
                connectionSb.ConnectionString = defaultConnectionString ?? "";
            }
            else
            {
                connectionSb.ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            }

            bool hasConnectionSuccess = ConnectionSuccess(EDataBaseEngine.OleDb, connectionSb.ConnectionString.ToString(), out string mensajeError);

            bool needConnectionParameters = (!hasConnectionSuccess);

            while (needConnectionParameters)
            {
                userGotParameters = true;

                FrmGetParamOleDb frmGetParamOleDb;
                frmGetParamOleDb = new FrmGetParamOleDb();
                frmGetParamOleDb.ConnectionString = defaultConnectionString;

                frmGetParamOleDb.MensajeError = "Ha ocurrido el siguiente error: \r\r" + mensajeError;

                frmGetParamOleDb.ShowDialog();

                if (frmGetParamOleDb.DialogResult == DialogResult.OK)
                {
                    defaultConnectionString = frmGetParamOleDb.ConnectionString;
                    connectionSb.ConnectionString = frmGetParamOleDb.ConnectionString;
                    hasConnectionSuccess = ConnectionSuccess(EDataBaseEngine.OleDb, connectionSb.ConnectionString.ToString(), out mensajeError);
                }
                else
                {
                    hasConnectionSuccess = false;
                }

                needConnectionParameters = (!hasConnectionSuccess) && (frmGetParamOleDb.DialogResult == DialogResult.OK);
            }

            if (!hasConnectionSuccess)
            {
                return false;
            }
            else
            {
                if (!ExistsConnectionString(connectionStringName))
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(connectionStringName, connectionSb.ToString());
                    config.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);

                    config.Save(ConfigurationSaveMode.Modified, true);
                    ConfigurationManager.RefreshSection("connectionStrings");
                }
                else
                {
                    if (userGotParameters)
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                        config.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString = connectionSb.ToString();
                        config.Save(ConfigurationSaveMode.Modified, true);
                        ConfigurationManager.RefreshSection("connectionStrings");
                    }
                }
            }
            return true;
        }
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
        private static string Decrypt(string input)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(input);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
        private static string Encrypt(string input)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(input);
            result = Convert.ToBase64String(encryted);
            return result;
        }
        public static string DecryptedConnectionString(string encryptedConnectionString)
        {
            return DecryptedConnectionString(EDataBaseEngine.MSSqlServer, encryptedConnectionString);
        }
        public static string DecryptedConnectionString(EDataBaseEngine dataBaseEngine, string encryptedConnectionString)
        {
            string response = "";

            switch (dataBaseEngine)
            {
                case EDataBaseEngine.MSSqlServer:
                    var connectionSb = new SqlConnectionStringBuilder();
                    connectionSb.ConnectionString = encryptedConnectionString;

                    connectionSb.UserID = Decrypt(connectionSb.UserID);
                    connectionSb.Password = Decrypt(connectionSb.Password);
                    connectionSb.DataSource = Decrypt(connectionSb.DataSource);
                    connectionSb.InitialCatalog = Decrypt(connectionSb.InitialCatalog);

                    response = connectionSb.ConnectionString;
                    break;

                case EDataBaseEngine.MySql:
                    var connectionSbMySql = new MySqlConnectionStringBuilder();
                    connectionSbMySql.ConnectionString = encryptedConnectionString;

                    connectionSbMySql.UserID = Decrypt(connectionSbMySql.UserID);
                    connectionSbMySql.Password = Decrypt(connectionSbMySql.Password);
                    connectionSbMySql.Server = Decrypt(connectionSbMySql.Server);

                    if (!string.IsNullOrEmpty(connectionSbMySql.Database.Trim()))
                        connectionSbMySql.Database = Decrypt(connectionSbMySql.Database);

                    response = connectionSbMySql.ConnectionString;
                    break;

                default:
                    response = encryptedConnectionString;
                    break;
            }

            return response;
        }
        public static  long ParseToLong(object result)
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
