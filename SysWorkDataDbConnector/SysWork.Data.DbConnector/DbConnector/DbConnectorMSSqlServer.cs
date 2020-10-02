using System;
using System.Windows.Forms;
using SysWork.Data.Common.Forms.GetParameters;
using SysWork.Data.Common.Utilities;
using System.Data.SqlClient;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.DbConnector.Utilities;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    /// Implementation of AbstractDbConnector Class for MSSqlServer
    /// </summary>
    /// <seealso cref="SysWork.Data.Common.DbConnector.AbstractDBConnector" />
    
    public class DbConnectorMSSqlServer : AbstractDBConnector
    {
        /// <summary>
        /// Try to connect with the specified parameters.
        /// </summary>
        /// <exception cref="ArgumentException">The ConnectionStringName is not set.</exception>
        public override void Connect()
        {
            if (TryGetConnectionStringFromConfig)
            {
                if (string.IsNullOrEmpty(ConnectionStringName))
                    throw new ArgumentException("The ConnectionStringName is not set.");

                if (DbUtil.ExistsConnectionStringInConfig(ConnectionStringName))
                    ConnectionString = ConnectorUtilities.GetConnectionStringFromConfig(ConnectionStringName);

            }

            SqlConnectionStringBuilder connectionSb = new SqlConnectionStringBuilder();
            UserGotParameters = false;

            if (string.IsNullOrEmpty(ConnectionString))
            {
                connectionSb.DataSource = DefaultDataSource ?? "";
                connectionSb.UserID = DefaultUser ?? "";
                connectionSb.Password = DefaultPassword ?? "";
                connectionSb.InitialCatalog = DefaultDatabase ?? "";
                ConnectorParameterTypeUsed = EConnectorParameterTypeUsed.ManualParameter;
            }
            else
            {
                connectionSb.ConnectionString = ConnectionString;

                if (IsEncryptedData)
                {
                    connectionSb.UserID = ConnectorUtilities.Decrypt(connectionSb.UserID);
                    connectionSb.Password = ConnectorUtilities.Decrypt(connectionSb.Password);
                    connectionSb.DataSource = ConnectorUtilities.Decrypt(connectionSb.DataSource);
                    connectionSb.InitialCatalog = ConnectorUtilities.Decrypt(connectionSb.InitialCatalog);
                }
                ConnectorParameterTypeUsed = EConnectorParameterTypeUsed.ConnectionString;
            }

            bool hasConnectionSuccess = false;
            string errMessage = "";

            if (!BeforeConnectShowDefaultsParameters)
            {
                hasConnectionSuccess = DbUtil.ConnectionSuccess(EDatabaseEngine.MSSqlServer, connectionSb.ConnectionString.ToString(), out errMessage);
                ConnectionError = errMessage;
            }

            bool needConnectionParameters = (!hasConnectionSuccess);

            while (needConnectionParameters && PromptUser)
            {
                UserGotParameters = true;

                FrmGetParamSQL frmGetParamSQL = new FrmGetParamSQL();

                frmGetParamSQL.Server = connectionSb.DataSource;
                frmGetParamSQL.Login = connectionSb.UserID;
                frmGetParamSQL.Password = connectionSb.Password;
                frmGetParamSQL.DataBase = connectionSb.InitialCatalog;
                frmGetParamSQL.ConnectionString = ConnectionString;

                frmGetParamSQL.ErrMessage = string.IsNullOrEmpty(errMessage) ? "" : "Ha ocurrido el siguiente error: \r\r" + errMessage;

                frmGetParamSQL.ParameterTypeUsed = ConnectorParameterTypeUsed;

                frmGetParamSQL.ShowDialog();

                if (frmGetParamSQL.DialogResult == DialogResult.OK)
                {
                    ConnectorParameterTypeUsed = frmGetParamSQL.ParameterTypeUsed;

                    if (!string.IsNullOrEmpty(frmGetParamSQL.ConnectionString))
                    {
                        ConnectionString  = frmGetParamSQL.ConnectionString;
                        connectionSb.ConnectionString = frmGetParamSQL.ConnectionString;
                    }
                    else
                    {
                        connectionSb.DataSource = frmGetParamSQL.Server;
                        connectionSb.UserID = frmGetParamSQL.Login;
                        connectionSb.Password = frmGetParamSQL.Password;

                        if (!string.IsNullOrEmpty(frmGetParamSQL.DataBase.Trim()))
                            connectionSb.InitialCatalog = frmGetParamSQL.DataBase;
                    }

                    hasConnectionSuccess = DbUtil.ConnectionSuccess(EDatabaseEngine.MSSqlServer,connectionSb.ConnectionString.ToString(), out errMessage);
                    ConnectionError = errMessage;
                }
                else
                {
                    hasConnectionSuccess = false;
                }

                needConnectionParameters = (!hasConnectionSuccess) && (frmGetParamSQL.DialogResult == DialogResult.OK);
            }

            ConnectionString = connectionSb.ConnectionString;
            IsConnectionSuccess = hasConnectionSuccess;

            if (IsConnectionSuccess && WriteInConfigFile)
            {
                if (IsEncryptedData)
                {
                    connectionSb.UserID = ConnectorUtilities.Encrypt(connectionSb.UserID);
                    connectionSb.Password = ConnectorUtilities.Encrypt (connectionSb.Password);
                    connectionSb.DataSource = ConnectorUtilities.Encrypt (connectionSb.DataSource);
                    connectionSb.InitialCatalog = ConnectorUtilities.Encrypt(connectionSb.InitialCatalog);
                }

                if (!DbUtil.ExistsConnectionStringInConfig(ConnectionStringName))
                    ConnectorUtilities.SaveConnectionStringInConfig(ConnectionStringName, connectionSb.ToString());
                else
                    if (UserGotParameters)
                    ConnectorUtilities.EditConnectionStringInConfig(ConnectionStringName, connectionSb.ToString());
            }
        }
    }
}
