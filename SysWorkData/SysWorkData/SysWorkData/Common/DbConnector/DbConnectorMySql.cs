using System;
using System.Windows.Forms;
using SysWork.Data.Common.FormsGetParam;
using SysWork.Data.Common.Utilities;
using MySql.Data.MySqlClient;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    /// Implementation of AbstractDbConnector Class for MySql
    /// </summary>
    /// <seealso cref="SysWork.Data.Common.DbConnector.AbstractDBConnector" />
    public class DbConnectorMySql : AbstractDBConnector
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
                ConnectorParameterTypeUsed = EConnectorParameterTypeUsed.ManualParameter;
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
                ConnectorParameterTypeUsed = EConnectorParameterTypeUsed.ConnectionString;
            }

            bool hasConnectionSuccess = false;
            string errMessage = "";

            if (!BeforeConnectShowDefaultsParameters)
            {
                hasConnectionSuccess = DbUtil.ConnectionSuccess(EDataBaseEngine.MySql, connectionSb.ConnectionString.ToString(), out errMessage);
                ConnectionError = errMessage;
            }

            bool needConnectionParameters = (!hasConnectionSuccess);

            while (needConnectionParameters && PromptUser)
            {
                UserGotParameters = true;

                FrmGetParamMySQL frmGetParamMySQL;
                frmGetParamMySQL = new FrmGetParamMySQL();

                frmGetParamMySQL.Server = connectionSb.Server;
                frmGetParamMySQL.Login = connectionSb.UserID;
                frmGetParamMySQL.Password = connectionSb.Password;
                frmGetParamMySQL.DataBase = connectionSb.Database;
                frmGetParamMySQL.ConnectionString = ConnectionString;

                frmGetParamMySQL.ErrMessage = string.IsNullOrEmpty(errMessage) ? "" : "Ha ocurrido el siguiente error: \r\r" + errMessage;

                frmGetParamMySQL.ParameterTypeUsed = ConnectorParameterTypeUsed;

                frmGetParamMySQL.ShowDialog();

                if (frmGetParamMySQL.DialogResult == DialogResult.OK)
                {
                    ConnectorParameterTypeUsed = frmGetParamMySQL.ParameterTypeUsed;

                    if (!string.IsNullOrEmpty(frmGetParamMySQL.ConnectionString))
                    {
                        ConnectionString = frmGetParamMySQL.ConnectionString;
                        connectionSb.ConnectionString = frmGetParamMySQL.ConnectionString;
                    }
                    else
                    {
                        connectionSb.Server = frmGetParamMySQL.Server;
                        connectionSb.UserID = frmGetParamMySQL.Login;
                        connectionSb.Password = frmGetParamMySQL.Password;

                        if (!string.IsNullOrEmpty(frmGetParamMySQL.DataBase.Trim()))
                            connectionSb.Database = frmGetParamMySQL.DataBase;
                    }

                    hasConnectionSuccess = DbUtil.ConnectionSuccess(EDataBaseEngine.MySql, connectionSb.ConnectionString.ToString(), out errMessage);
                    ConnectionError = errMessage;
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
