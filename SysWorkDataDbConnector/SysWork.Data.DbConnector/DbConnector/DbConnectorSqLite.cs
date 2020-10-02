using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using SysWork.Data.Common.Forms.GetParameters;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.DbConnector.Utilities;

namespace SysWork.Data.Common.DbConnector
{

    /// <summary>
    /// Implementation of AbstractDbConnector Class for SQLite
    /// </summary>
    /// <seealso cref="SysWork.Data.Common.DbConnector.AbstractDBConnector" />
    class DbConnectorSqLite : AbstractDBConnector
    {
        /// <summary>
        /// try to connect to the specified parameters.
        /// </summary>
        /// <exception cref="ArgumentException">The ConnectionStringName is not set.</exception>
        public override void Connect()
        {
            ConnectorParameterTypeUsed = EConnectorParameterTypeUsed.ConnectionString;

            if (TryGetConnectionStringFromConfig)
            {
                if (string.IsNullOrEmpty(ConnectionStringName))
                    throw new ArgumentException("The ConnectionStringName is not set.");

                if (DbUtil.ExistsConnectionStringInConfig(ConnectionStringName))
                    ConnectionString = ConnectorUtilities.GetConnectionStringFromConfig(ConnectionStringName);

            }

            SQLiteConnectionStringBuilder connectionSb = new SQLiteConnectionStringBuilder();
            UserGotParameters = false;

            if (string.IsNullOrEmpty(ConnectionString))
            {
                connectionSb.Password = DefaultPassword ?? "";
            }
            else
            {
                connectionSb.ConnectionString = ConnectionString;

                if (IsEncryptedData)
                {
                    connectionSb.Password = ConnectorUtilities.Decrypt(connectionSb.Password);
                }
            }


            bool hasConnectionSuccess = false;

            if (!BeforeConnectShowDefaultsParameters)
            {
                hasConnectionSuccess = File.Exists(connectionSb.DataSource);
                if (!hasConnectionSuccess) ConnectionError = "El archivo no existe";
            }

            bool needConnectionParameters = (!hasConnectionSuccess);
            while (needConnectionParameters && PromptUser)
            {
                UserGotParameters = true;

                FrmGetParamSQLite frmGetParamSQLite;
                frmGetParamSQLite = new FrmGetParamSQLite();

                frmGetParamSQLite.ConnectionString = ConnectionString;

                frmGetParamSQLite.ErrMessage = string.IsNullOrEmpty(ConnectionError) ? "" : "Ha ocurrido el siguiente error: \r\r" + ConnectionError;

                frmGetParamSQLite.ShowDialog();

                if (frmGetParamSQLite.DialogResult == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(frmGetParamSQLite.ConnectionString))
                    {
                        ConnectionString = frmGetParamSQLite.ConnectionString;
                        connectionSb.ConnectionString = frmGetParamSQLite.ConnectionString;
                    }

                    hasConnectionSuccess = DbUtil.ConnectionSuccess(EDatabaseEngine.SqLite, connectionSb.ConnectionString.ToString(), out string mensajeError);
                    ConnectionError = mensajeError;
                }
                else
                {
                    hasConnectionSuccess = false;
                }

                needConnectionParameters = (!hasConnectionSuccess) && (frmGetParamSQLite.DialogResult == DialogResult.OK);
            }

            ConnectionString = connectionSb.ConnectionString;
            IsConnectionSuccess = hasConnectionSuccess;

            if (IsConnectionSuccess && WriteInConfigFile)
            {
                if (IsEncryptedData)
                {
                    connectionSb.Password = ConnectorUtilities.Encrypt(connectionSb.Password);
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
