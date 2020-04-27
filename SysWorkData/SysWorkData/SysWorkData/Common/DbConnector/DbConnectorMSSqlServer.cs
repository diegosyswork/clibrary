using System;
using System.Windows.Forms;
using SysWork.Data.Common.FormsGetParam;
using SysWork.Data.Common.Utilities;
using System.Data.SqlClient;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    /// Implementation of AbstractDbConnector Class for MSSqlServer
    /// <seealso cref="SysWork.Data.Common.DbConnector.AbstractDbConnector" />
    /// </summary>
    public class DbConnectorMSSqlServer : AbstractDbConnector
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
                    ConnectionString = DbUtil.GetConnectionStringFromConfig(ConnectionStringName);

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
                    connectionSb.UserID = DbUtil.Decrypt(connectionSb.UserID);
                    connectionSb.Password = DbUtil.Decrypt(connectionSb.Password);
                    connectionSb.DataSource = DbUtil.Decrypt(connectionSb.DataSource);
                    connectionSb.InitialCatalog = DbUtil.Decrypt(connectionSb.InitialCatalog);
                }
                ConnectorParameterTypeUsed = EConnectorParameterTypeUsed.ConnectionString;
            }

            bool hasConnectionSuccess = false;
            string errMessage = "";

            if (!BeforeConnectShowDefaultsParameters)
            {
                hasConnectionSuccess = DbUtil.ConnectionSuccess(EDataBaseEngine.MSSqlServer, connectionSb.ConnectionString.ToString(), out errMessage);
                ConnectionError = errMessage;
            }

            bool needConnectionParameters = (!hasConnectionSuccess);

            while (needConnectionParameters && PromptUser)
            {
                UserGotParameters = true;

                FrmGetParamSQL frmGetParamSQL;
                frmGetParamSQL = new FrmGetParamSQL();

                frmGetParamSQL.Server = connectionSb.DataSource;
                frmGetParamSQL.Login = connectionSb.UserID;
                frmGetParamSQL.Password = connectionSb.Password;
                frmGetParamSQL.DataBase = connectionSb.InitialCatalog;
                frmGetParamSQL.ConnectionString = ConnectionString;

                frmGetParamSQL.ErrMessage = string.IsNullOrEmpty(errMessage) ? "" : "Ha ocurrido el siguiente error: \r\r" + errMessage;
                frmGetParamSQL.ParameterTypeUsed = ConnectorParameterTypeUsed;

                frmGetParamSQL.ShowDialog();

                ConnectorParameterTypeUsed = frmGetParamSQL.ParameterTypeUsed;

                if (frmGetParamSQL.DialogResult == DialogResult.OK)
                {

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

                    hasConnectionSuccess = DbUtil.ConnectionSuccess(EDataBaseEngine.MSSqlServer,connectionSb.ConnectionString.ToString(), out errMessage);
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
                    connectionSb.UserID = DbUtil.Encrypt(connectionSb.UserID);
                    connectionSb.Password = DbUtil.Encrypt (connectionSb.Password);
                    connectionSb.DataSource = DbUtil.Encrypt (connectionSb.DataSource);
                    connectionSb.InitialCatalog = DbUtil.Encrypt(connectionSb.InitialCatalog);
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
