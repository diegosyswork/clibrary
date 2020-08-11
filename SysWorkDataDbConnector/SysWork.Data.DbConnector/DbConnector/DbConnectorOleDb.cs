using System;
using System.Data.OleDb;
using System.Windows.Forms;
using SysWork.Data.Common.Forms.GetParameters;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.DbConnector.Utilities;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    /// Implementation of AbstractDbConnector Class for OleDb
    /// </summary>
    /// <seealso cref="SysWork.Data.Common.DbConnector.AbstractDBConnector" />
    public class DbConnectorOleDb : AbstractDBConnector
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

            OleDbConnectionStringBuilder connectionSb = new OleDbConnectionStringBuilder();
            UserGotParameters = false;

            if (!string.IsNullOrEmpty(ConnectionString))
            {
                connectionSb.ConnectionString = ConnectionString;
                if (IsEncryptedData)
                {
                    foreach (var key in connectionSb.Keys)
                    {
                        if (connectionSb.ContainsKey(key.ToString()))
                            if (connectionSb[key.ToString()].GetType() == typeof(string))
                                connectionSb[key.ToString()] = ConnectorUtilities.Decrypt (connectionSb[key.ToString()].ToString());
                    }
                }
            }

            bool hasConnectionSuccess = false;
            string errMessage = "";

            if (!BeforeConnectShowDefaultsParameters)
            {
                hasConnectionSuccess = DbUtil.ConnectionSuccess(EDataBaseEngine.OleDb, connectionSb.ConnectionString.ToString(), out errMessage);
                ConnectionError = errMessage;
            }

            bool needConnectionParameters = (!hasConnectionSuccess);

            while (needConnectionParameters && PromptUser)
            {
                UserGotParameters = true;

                FrmGetParamOleDb frmGetParamOleDb;
                frmGetParamOleDb = new FrmGetParamOleDb();

                frmGetParamOleDb.ConnectionString = ConnectionString;

                frmGetParamOleDb.ErrMessage = string.IsNullOrEmpty(errMessage) ? "" : "Ha ocurrido el siguiente error: \r\r" + errMessage;

                frmGetParamOleDb.ShowDialog();

                if (frmGetParamOleDb.DialogResult == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(frmGetParamOleDb.ConnectionString))
                    {
                        ConnectionString = frmGetParamOleDb.ConnectionString;
                        connectionSb.ConnectionString = frmGetParamOleDb.ConnectionString;
                    }

                    hasConnectionSuccess = DbUtil.ConnectionSuccess(EDataBaseEngine.OleDb , connectionSb.ConnectionString.ToString(), out errMessage);
                    ConnectionError = errMessage;
                }
                else
                {
                    hasConnectionSuccess = false;
                }

                needConnectionParameters = (!hasConnectionSuccess) && (frmGetParamOleDb.DialogResult == DialogResult.OK);
            }

            ConnectionString = connectionSb.ConnectionString;
            IsConnectionSuccess = hasConnectionSuccess;

            if (IsConnectionSuccess && WriteInConfigFile)
            {
                if (IsEncryptedData)
                {
                    foreach (var key in connectionSb.Keys)
                    {
                        if (connectionSb.ContainsKey(key.ToString()))
                            if (connectionSb[key.ToString()].GetType()==typeof(string))
                                connectionSb[key.ToString()] = ConnectorUtilities.Encrypt(connectionSb[key.ToString()].ToString());
                    }
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
