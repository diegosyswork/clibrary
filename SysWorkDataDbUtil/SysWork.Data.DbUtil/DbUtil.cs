using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Data.DbUtil
{
    public class DbUtil
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

            bool huboConexionExitosa = CunnectionSuccess(connectionSb.ConnectionString.ToString(), out mensajeError);

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

                huboConexionExitosa = CunnectionSuccess(connectionSb.ConnectionString.ToString(), out mensajeError);

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
        public static bool CunnectionSuccess(string connectionString, out string mensajeError)
        {
            mensajeError = "";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    return true;
                }
            }
            catch (Exception e)
            {
                mensajeError = e.Message;
                return false;
            }
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

        private string GetGeneratedSql(SqlCommand cmd)
        {
            string result = cmd.CommandText.ToString();
            foreach (SqlParameter p in cmd.Parameters)
            {
                string isQuted = (p.Value is string || p.Value is DateTime) ? "'" : "";
                result = result.Replace('@' + p.ParameterName.ToString(), isQuted + p.Value.ToString() + isQuted);
            }
            return result;
        }

        public static string ConvertCommandParamatersToLiteralValues(SqlCommand cmd)
        {
            string query;
            try
            {
                query = cmd.CommandText;

                foreach (SqlParameter prm in cmd.Parameters)
                {
                    switch (prm.SqlDbType)
                    {
                        case SqlDbType.Bit:
                            int boolToInt = (bool)prm.Value ? 1 : 0;
                            query = query.Replace(prm.ParameterName, string.Format("{0}", (bool)prm.Value ? 1 : 0));
                            break;
                        case SqlDbType.Int:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                        case SqlDbType.BigInt:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                        case SqlDbType.Float:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                        case SqlDbType.Decimal:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                        case SqlDbType.Money:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                        case SqlDbType.Real:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                        case SqlDbType.SmallInt:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                        case SqlDbType.SmallMoney:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                        case SqlDbType.TinyInt:
                            query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                            break;
                        default:
                            query = query.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
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

        public static bool ExistsTable(string connectionString, string nombreTabla)
        {
            string sql =
                " SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES " +
                " WHERE TABLE_TYPE = 'BASE TABLE' " +
                " AND TABLE_NAME = @nombreTabla ";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@nombreTabla", nombreTabla);

                    int n = (int)sqlCommand.ExecuteScalar();

                    sqlConnection.Close();

                    return n != 0;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool ExistsColumn(string connectionString, string tableName, string columnName)
        {

            string sql = " SELECT COUNT(*) " +
                         " FROM INFORMATION_SCHEMA.COLUMNS " +
                         " WHERE COLUMN_NAME = @pNombreColumna AND TABLE_NAME = @pNombreTabla ";

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@pNombreTabla", tableName);
                    sqlCommand.Parameters.AddWithValue("@pNombreColumna", columnName);

                    int n = (Int32)sqlCommand.ExecuteScalar();

                    sqlConnection.Close();

                    return n != 0;
                }
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static bool ExecuteBatchNonQuery(string query, string ConnectionString)
        {
            bool result = false;

            result = ExecuteBatchNonQuery(query, new SqlConnection(ConnectionString));

            return result;
        }

        public static bool ExecuteBatchNonQuery(string query, SqlConnection connection)
        {
            string sqlBatch = string.Empty;

            using (SqlCommand sqlCommand = new SqlCommand(string.Empty, connection))
            {
                try
                {
                    connection.Open();
                    query += "\nGO";   // make sure last batch is executed.

                    foreach (string line in query.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (line.ToUpperInvariant().Trim() == "GO")
                        {
                            if (! string.IsNullOrEmpty(sqlBatch.Trim()) )
                            {
                                sqlCommand.CommandText = sqlBatch;
                                sqlCommand.ExecuteNonQuery();
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
    }
}
