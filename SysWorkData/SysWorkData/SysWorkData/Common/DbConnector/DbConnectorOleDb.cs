using System;
using System.Data.OleDb;
using System.Windows.Forms;
using SysWork.Data.Common.FormsGetParam;
using SysWork.Data.Common.Utilities;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    /// Implementation of AbstractDbConnector Class for OleDb
    /// <seealso cref="SysWork.Data.Common.DbConnector.AbstractDbConnector" />
    /// </summary>
    public class DbConnectorOleDb : AbstractDbConnector
    {
        /// <summary>
        /// try to connect to the specified parameters.
        /// </summary>
        /// <exception cref="ArgumentException">The ConnectionStringName is nos set.</exception>
        public override void Connect()
        {
            if (TryGetConnectionStringFromConfig)
            {
                if (string.IsNullOrEmpty(ConnectionStringName))
                    throw new ArgumentException("The ConnectionStringName is nos set.");

                if (DbUtil.ExistsConnectionStringInConfig(ConnectionStringName))
                    ConnectionString = DbUtil.GetConnectionStringFromConfig(ConnectionStringName);

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
                                connectionSb[key.ToString()] = DbUtil.Decrypt (connectionSb[key.ToString()].ToString());
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
                                connectionSb[key.ToString()] = DbUtil.Encrypt(connectionSb[key.ToString()].ToString());
                    }
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
