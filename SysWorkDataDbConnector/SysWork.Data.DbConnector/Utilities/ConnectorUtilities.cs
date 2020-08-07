using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.DbConnector.Utilities
{
    public static class ConnectorUtilities
    {
        /// <summary>
        /// Gets the connection string from app configuration file.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns></returns>
        public static string GetConnectionStringFromConfig(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        /// <summary>
        /// Edits the connectionString in app configuration file.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <param name="connectionString">The connection string.</param>
        public static void EditConnectionStringInConfig(string connectionStringName, string connectionString)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString = connectionString;
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        /// <summary>
        /// Saves the new connection string in app configuration file.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <param name="connectionString">The connection string.</param>
        public static void SaveConnectionStringInConfig(string connectionStringName, string connectionString)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(connectionStringName, connectionString);
            config.ConnectionStrings.ConnectionStrings.Add(connectionStringSettings);
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        /// <summary>
        /// Decrypts the specified input.
        /// </summary>
        /// <remarks>
        /// This method is used to dencrypt values of ConnectionsString.
        /// </remarks>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string Decrypt(string input)
        {
            string result = string.Empty;
            string temp = string.Empty;
            byte[] decryted = null;

            // In case of the input are non "Encrypted" (Base64), return the same input parameter.
            try
            {
                decryted = Convert.FromBase64String(input);
            }
            catch (FormatException)
            {

                decryted = Encoding.Unicode.GetBytes(input);
            }

            result = Encoding.Unicode.GetString(decryted);
            return result;
        }

        /// <summary>
        /// Encrypts the specified input.
        /// </summary>
        /// <remarks>
        /// This method is used to encrypt values of ConnectionsString.
        /// </remarks>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string Encrypt(string input)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(input);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// <summary>
        /// Decrypteds the an MSSqlServer connection string.
        /// </summary>
        /// <param name="encryptedConnectionString">The encrypted connection string.</param>
        /// <returns></returns>
        public static string DecryptedConnectionString(string encryptedConnectionString)
        {
            return DecryptedConnectionString(EDataBaseEngine.MSSqlServer, encryptedConnectionString);
        }


        /// <summary>
        /// Decrypteds an connection string.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="encryptedConnectionString">The encrypted connection string.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method</exception>
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

                case EDataBaseEngine.SqLite:
                    var connectionSbSqlite = new SQLiteConnectionStringBuilder();
                    connectionSbSqlite.ConnectionString = encryptedConnectionString;

                    connectionSbSqlite.Password = Decrypt(connectionSbSqlite.Password);
                    response = connectionSbSqlite.ConnectionString;

                    break;

                case EDataBaseEngine.OleDb:
                    var connectionSbOleDb = new OleDbConnectionStringBuilder();
                    connectionSbOleDb.ConnectionString = encryptedConnectionString;

                    foreach (var key in connectionSbOleDb.Keys)
                    {
                        if (connectionSbOleDb.ContainsKey(key.ToString()))
                            if (connectionSbOleDb[key.ToString()].GetType() == typeof(string))
                                connectionSbOleDb[key.ToString()] = ConnectorUtilities.Decrypt(connectionSbOleDb[key.ToString()].ToString());
                    }

                    response = connectionSbOleDb.ConnectionString;

                    break;

                default:
                    throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method");
            }

            return response;
        }


    }
}
