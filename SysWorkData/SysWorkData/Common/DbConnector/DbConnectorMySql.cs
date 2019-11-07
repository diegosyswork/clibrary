using System;
using System.Windows.Forms;
using SysWork.Data.Common.FormsGetParam;
using SysWork.Data.Common.Utilities;
using MySql.Data.MySqlClient;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    /// Implementation of AbstractDbConnector Class for MySql
    /// <seealso cref="SysWork.Data.Common.DbConnector.AbstractDbConnector" />
    /// </summary>

    public class DbConnectorMySql : AbstractDbConnector
    {
        /// <summary>
        /// try to connect to the specified parameters.
        /// </summary>
        /// <exception cref="ArgumentException">The ConnectionStringName is not set.</exception>
        public override void Connect()
        {
            if (TryGetConnectionStringFromConfig)
            {
                if (string.IsNullOrEmpty(ConnectionStringName))
                    throw new ArgumentException("The ConnectionStringName is not set.");

                if (DbUtil.ExistsConnectionStringInConfig(ConnectionStringName))
                    ConnectionString = DbUtil.GetConnectionStringFromConfig(ConnectionStringName);

            }

            MySqlConnectionStringBuilder connectionSb = new MySqlConnectionStringBuilder();
            UserGotParameters = false;

            if (string.IsNullOrEmpty(ConnectionString))
            {
                connectionSb.Server = DefaultDataSource ?? "";
                connectionSb.UserID = DefaultUser ?? "";
                connectionSb.Password = DefaultPassword ?? "";
                connectionSb.Database= DefaultDatabase ?? "";
            }
            else
            {
                connectionSb.ConnectionString = ConnectionString;

                if (IsEncryptedData)
                {
                    connectionSb.UserID = DbUtil.Decrypt(connectionSb.UserID);
                    connectionSb.Password = DbUtil.Decrypt(connectionSb.Password);
                    connectionSb.Server = DbUtil.Decrypt(connectionSb.Server);
                    connectionSb.Database = DbUtil.Decrypt(connectionSb.Database );
                }
            }

            bool hasConnectionSuccess = DbUtil.ConnectionSuccess(EDataBaseEngine.MySql, connectionSb.ConnectionString.ToString(), out string mensajeError);
            ConnectionError = mensajeError;

            bool needConnectionParameters = (!hasConnectionSuccess);

            while (needConnectionParameters && PromptUser)
            {
                UserGotParameters = true;

                FrmGetParamMySQL frmGetParamMySQL;
                frmGetParamMySQL = new FrmGetParamMySQL();

                frmGetParamMySQL.Server = connectionSb.Server;
                frmGetParamMySQL.InicioDeSesion = connectionSb.UserID;
                frmGetParamMySQL.Password = connectionSb.Password;
                frmGetParamMySQL.BaseDeDatos = connectionSb.Database;
                frmGetParamMySQL.ConnectionString = ConnectionString;

                frmGetParamMySQL.MensajeError = "Ha ocurrido el siguiente error: \r\r" + mensajeError;

                frmGetParamMySQL.ShowDialog();

                if (frmGetParamMySQL.DialogResult == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(frmGetParamMySQL.ConnectionString))
                    {
                        ConnectionString = frmGetParamMySQL.ConnectionString;
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

                    hasConnectionSuccess = DbUtil.ConnectionSuccess(EDataBaseEngine.MySql, connectionSb.ConnectionString.ToString(), out mensajeError);
                    ConnectionError = mensajeError;
                }
                else
                {
                    hasConnectionSuccess = false;
                }

                needConnectionParameters = (!hasConnectionSuccess) && (frmGetParamMySQL.DialogResult == DialogResult.OK);
            }

            ConnectionString = connectionSb.ConnectionString;
            IsConnectionSuccess = hasConnectionSuccess;

            if (IsConnectionSuccess && WriteInConfigFile)
            {
                if (IsEncryptedData)
                {
                    connectionSb.UserID = DbUtil.Encrypt(connectionSb.UserID);
                    connectionSb.Password = DbUtil.Encrypt(connectionSb.Password);
                    connectionSb.Server = DbUtil.Encrypt(connectionSb.Server);
                    connectionSb.Database = DbUtil.Encrypt(connectionSb.Database);
                }

                if (!DbUtil.ExistsConnectionStringInConfig(ConnectionStringName))
                    DbUtil.SaveConnectionStringInConfig(ConnectionStringName, connectionSb.ToString());
                else
                    if (UserGotParameters)
                    DbUtil.EditConnectionStringInConfig(ConnectionStringName, connectionSb.ToString());

            }
        }
    }
}
