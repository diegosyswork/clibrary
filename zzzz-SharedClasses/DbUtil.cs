using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using SysWork.Data.Common;
using SysWork.Data.Common.ObjectResolver;

namespace SysWork.Data.DbUtil
{
    public static class DbUtil
    {
        public static bool VerificaConexionYSolicitaDatos(string nombreConexion,string defDataSource = null,string defUserID=null, string defPassWord=null, string defInitialCatalog = null)
        {
            SqlConnectionStringBuilder connectionSb = new SqlConnectionStringBuilder();
            bool elUsuarioProporcionoParametros = false;

            if (!ExistsConnectionString(nombreConexion))
            {
                //ASIGNO DATOS DEFAULT
                connectionSb.DataSource = (defDataSource == null ? "LOCALHOST" : defDataSource);
                connectionSb.UserID = (defUserID == null ? "SA" : defUserID);
                connectionSb.Password = (defPassWord == null ? "57125712" : defPassWord);
                connectionSb.InitialCatalog = (defInitialCatalog == null ? "master" : defInitialCatalog);
            }
            else
            {
                connectionSb.ConnectionString = ConfigurationManager.ConnectionStrings[nombreConexion].ConnectionString;
            }

            string mensajeError;

            bool huboConexionExitosa = ConnectionSuccess(connectionSb.ConnectionString.ToString(), out mensajeError);

            bool solicitarParametrosConexion = (!huboConexionExitosa);

            while (solicitarParametrosConexion)
            {
                elUsuarioProporcionoParametros = true;

                FrmDatosConexion frmDatosConexion;
                frmDatosConexion = new FrmDatosConexion();

                frmDatosConexion.Server = connectionSb.DataSource;
                frmDatosConexion.InicioDeSesion = connectionSb.UserID;
                frmDatosConexion.Password = connectionSb.Password;
                frmDatosConexion.BaseDeDatos = connectionSb.InitialCatalog;
                frmDatosConexion.MensajeError = "Ha ocurrido el siguiente error: \r\r" + mensajeError;

                frmDatosConexion.ShowDialog();

                connectionSb.DataSource = frmDatosConexion.Server;
                connectionSb.UserID = frmDatosConexion.InicioDeSesion;
                connectionSb.Password = frmDatosConexion.Password;

                if (!string.IsNullOrEmpty(frmDatosConexion.BaseDeDatos.Trim()))
                    connectionSb.InitialCatalog = frmDatosConexion.BaseDeDatos;

                huboConexionExitosa = ConnectionSuccess(connectionSb.ConnectionString.ToString(), out mensajeError);

                solicitarParametrosConexion = (!huboConexionExitosa) && (frmDatosConexion.DialogResult == DialogResult.OK);
            }

            if (!huboConexionExitosa)
            {
                return false;
            }
            else
            {
                if (!ExistsConnectionString(nombreConexion))
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(nombreConexion, connectionSb.ToString());
                    config.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);

                    config.Save(ConfigurationSaveMode.Modified, true);
                    ConfigurationManager.RefreshSection("connectionStrings");
                }
                else
                {
                    if (elUsuarioProporcionoParametros)
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                        config.ConnectionStrings.ConnectionStrings[nombreConexion].ConnectionString = connectionSb.ToString();
                        config.Save(ConfigurationSaveMode.Modified, true);
                        ConfigurationManager.RefreshSection("connectionStrings");
                    }
                }
            }

            return true;
        }
        public static bool SqlConnectionSuccess(string dataSource,string initialCatalog,string userID, string password, out string mensajeError)
        {
            mensajeError = "";
            SqlConnectionStringBuilder s = new SqlConnectionStringBuilder();
            s.DataSource = dataSource;
            s.UserID = userID;
            s.Password = password;
            s.InitialCatalog = initialCatalog;

            return ConnectionSuccess(s.ConnectionString, out mensajeError);
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
            return ConnectionSuccess(EDataBaseEngine.SqlServer, connectionString, out mensajeError);
        }

        /// <summary>
        /// Verifica si una Connection se puede abrir correctamente,  
        /// Debe especificarse el motor de base de datos.
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="mensajeError"></param>
        /// <returns></returns>
        public static bool ConnectionSuccess(EDataBaseEngine resolverType, string connectionString, out string mensajeError)
        {
            mensajeError = "";
            try
            {
                using (IDbConnection dbConnection = DataObjectResolver.GetIDbConnection(resolverType,connectionString))
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

        public static bool ExistsTable(EDataBaseEngine resolverType, string connectionString, string tableName)
        {
            bool exists =false;

            using (DbConnection dbConnection = DataObjectResolver.GetDbConnection(resolverType, connectionString))
            {
                dbConnection.Open();
                exists = dbConnection.GetSchema("Tables", new string[4] { null, null, tableName, "TABLE" }).Rows.Count > 0;
                dbConnection.Close();
            }

            return exists;
        }


        public static bool ExistsTable(string connectionString, string tableName)
        {
            return ExistsTable(EDataBaseEngine.SqlServer, connectionString, tableName);
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


        public static List<string> GetListTables(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable schema = connection.GetSchema("Tables", new string[] { null, null, null, "BASE TABLE" });
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    TableNames.Add(row[2].ToString());
                }
                connection.Close();
                return TableNames;
            }
        }
        public static bool ExistsColumn(EDataBaseEngine resolverType, string connectionString, string tableName, string columnName)
        {
            bool exists = false;
            using (DbConnection dbConnection = DataObjectResolver.GetDbConnection(resolverType, connectionString))
            {
                dbConnection.Open();
                DataTable dtCols = dbConnection.GetSchema("Columns", new[] { dbConnection.DataSource, null, tableName });
                exists = dtCols.Columns.Contains(columnName);
                dbConnection.Close();
            }
            return exists;
        }
        public static bool ExistsColumn(string connectionString, string tableName, string columnName)
        {
            return ExistsColumn(EDataBaseEngine.SqlServer,connectionString, tableName, columnName);
        }

        public static bool ExecuteBatchNonQuery(string query, string ConnectionString)
        {
            return ExecuteBatchNonQuery(EDataBaseEngine.SqlServer, query, ConnectionString);
        }
        public static bool ExecuteBatchNonQuery(EDataBaseEngine resolverType, string query, string ConnectionString)
        {
            bool result = false;

            result = ExecuteBatchNonQuery(resolverType, query, DataObjectResolver.GetIDbConnection(resolverType,ConnectionString));

            return result;
        }

        public static bool ExecuteBatchNonQuery(EDataBaseEngine resolverType,string query, IDbConnection connection)
        {
            string sqlBatch = string.Empty;

            using (IDbCommand dbCommand = DataObjectResolver.GetIDbCommand(resolverType,string.Empty,connection))
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
                            if (! string.IsNullOrEmpty(sqlBatch.Trim()) )
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

        public static string AddPrefixTableNameToFieldList(string fieldList, string tableName)
        {
            string[] splitList = fieldList.Split(',');

            for (int pos = 0; pos < splitList.Length; pos++)
            {
                splitList[pos] = tableName + "." + splitList[pos];
            }

            return string.Join(",", splitList); ;
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
