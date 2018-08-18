using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using SysWork.Data.Common.FormsGetParam;
using SysWork.Data.Common.ObjectResolver;

namespace SysWork.Data.Common.Utilities
{
    public static class DbUtil
    {
        public static bool ExistsTable(EDataBaseEngine dataBaseEngine, string connectionString, string tableName)
        {
            bool exists = false;

            using (DbConnection dbConnection = DataObjectResolver.GetDbConnection(dataBaseEngine, connectionString))
            {
                dbConnection.Open();
                if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
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
            using (DbConnection connection = DataObjectResolver.GetDbConnection(dataBaseEngine,connectionString))
            {
                connection.Open();
                DataTable schema = null;

                if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                    schema = connection.GetSchema("Tables", new string[] { null, null, null, "BASE TABLE" });
                else if (dataBaseEngine == EDataBaseEngine.OleDb)
                    schema=connection.GetSchema("Tables", new string[] { null, null, null, "TABLE" });
                else if (dataBaseEngine == EDataBaseEngine.SqLite)
                    schema=connection.GetSchema("Tables", new string[] { null, null, null, "TABLE" });

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
            using (DbConnection dbConnection = DataObjectResolver.GetDbConnection(dataBaseEngine, connectionString))
            {
                dbConnection.Open();
                DataTable dtColumns = null;

                if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                    dtColumns = dbConnection.GetSchema("Columns", new[] { null, null, tableName, null });
                else if (dataBaseEngine == EDataBaseEngine.OleDb)
                    dtColumns = dbConnection.GetSchema("Columns", new[] { null, null, tableName, null });
                else if (dataBaseEngine == EDataBaseEngine.SqLite)
                    dtColumns = dbConnection.GetSchema("Columns", new[] { null, tableName,null});

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

            result = ExecuteBatchNonQuery(dataBaseEngine, query, DataObjectResolver.GetIDbConnection(dataBaseEngine, ConnectionString));

            return result;
        }

        public static bool ExecuteBatchNonQuery(EDataBaseEngine dataBaseEngine, string query, IDbConnection connection)
        {
            string sqlBatch = string.Empty;

            using (IDbCommand dbCommand = DataObjectResolver.GetIDbCommand(dataBaseEngine, string.Empty, connection))
            {
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
                using (IDbConnection dbConnection = DataObjectResolver.GetIDbConnection(dataBaseEngine, connectionString))
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


        public static bool VerfifySQLConnectionStringOrGetParams(string connectionStringName, string defaultDataSource = null, string defaultUserId = null, string defaultPassWord = null, string defaultInitialCatalog = null)
        {
            SqlConnectionStringBuilder connectionSb = new SqlConnectionStringBuilder();
            bool userGotParameters = false;

            if (!ExistsConnectionString(connectionStringName))
            {
                //ASIGNO DATOS DEFAULT
                connectionSb.DataSource = (defaultDataSource == null ? "LOCALHOST" : defaultDataSource);
                connectionSb.UserID = (defaultUserId == null ? "SA" : defaultUserId);
                connectionSb.Password = (defaultPassWord == null ? "" : defaultPassWord);
                connectionSb.InitialCatalog = (defaultInitialCatalog == null ? "master" : defaultInitialCatalog);
            }
            else
            {
                connectionSb.ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            }

            string mensajeError;

            bool hasConnectionSuccess = ConnectionSuccess(connectionSb.ConnectionString.ToString(), out mensajeError);

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
                frmGetParamSQL.MensajeError = "Ha ocurrido el siguiente error: \r\r" + mensajeError;

                frmGetParamSQL.ShowDialog();

                connectionSb.DataSource = frmGetParamSQL.Server;
                connectionSb.UserID = frmGetParamSQL.InicioDeSesion;
                connectionSb.Password = frmGetParamSQL.Password;

                if (!string.IsNullOrEmpty(frmGetParamSQL.BaseDeDatos.Trim()))
                    connectionSb.InitialCatalog = frmGetParamSQL.BaseDeDatos;

                hasConnectionSuccess = ConnectionSuccess(connectionSb.ConnectionString.ToString(), out mensajeError);

                needConnectionParameters = (!hasConnectionSuccess) && (frmGetParamSQL.DialogResult == DialogResult.OK);
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
        
        private static DataTable ConvertToDatatable<T>(List<T> data)
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
    }
}
