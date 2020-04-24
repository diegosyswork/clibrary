﻿using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using SysWork.Data.Common.FormsGetParam;
using SysWork.Data.Common.Utilities;

namespace SysWork.Data.Common.DbConnector
{
    /// <summary>
    /// Implementation of AbstractDbConnector Class for SQLite
    /// <seealso cref="SysWork.Data.Common.DbConnector.AbstractDbConnector" />
    /// </summary>
    class DbConnectorSqLite : AbstractDbConnector
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
                    connectionSb.Password = DbUtil.Decrypt(connectionSb.Password);
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

                    hasConnectionSuccess = DbUtil.ConnectionSuccess(EDataBaseEngine.SqLite, connectionSb.ConnectionString.ToString(), out string mensajeError);
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
                    connectionSb.Password = DbUtil.Encrypt(connectionSb.Password);
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
