using System;
using System.Windows.Forms;
using SysWork.Data.Common.Utilities;

namespace SysWork.Data.Common.FormsGetParam
{
    /// <summary>
    /// Form to Get Data Parameters for MSSQLServer
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    internal partial class FrmGetParamSQL : Form
    {
        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the dataBase.
        /// </summary>
        /// <value>
        /// The dataBase.
        /// </value>
        public string DataBase { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrMessage { get; set; }

        public FrmGetParamSQL()
        {
            InitializeComponent();
        }

        private void FrmGetParamMSSQL_Load(object sender, EventArgs e)
        {
            txtServer.Text = Server;
            txtLogin.Text = Login;
            txtPassword.Text = Password;
            txtDataBase.Text = DataBase;

            txtConnectionString.Text = ConnectionString;

            rbtnConnectionString.Checked = !string.IsNullOrEmpty(ConnectionString);

            lblErrMessage.Text = ErrMessage;
            lblErrMessage.Refresh();

            if (!string.IsNullOrEmpty(ErrMessage))
            {
                this.Height = this.Height + lblErrMessage.Height;
            }
            else
            {
                this.Height = this.Height - lblErrMessage.Height;
            }

            this.CenterToScreen();
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            Server = txtServer.Text;
            Login = txtLogin.Text;
            Password = txtPassword.Text;
            DataBase = txtDataBase.Text;

            ConnectionString = txtConnectionString.Text;

            if (rbtnConnectionString.Checked)
            {
                if (string.IsNullOrEmpty(ConnectionString.Trim()))
                {
                    MessageBox.Show("Se debe informar la cadena de conexion!!!", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DbUtil.IsValidConnectionString(EDataBaseEngine.MSSqlServer, ConnectionString.Trim(), out string errMessage))
                {
                    MessageBox.Show($"Error en el formato de la cadena de conexion \r\n \r\n detalle del error: \r\n \r\n {errMessage} ", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                ConnectionString = "";
            }


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void RbtnParametrosManuales_CheckedChanged(object sender, EventArgs e)
        {
            grpManualParameters.Enabled = rbtnParametrosManuales.Checked;
            grpConnectionString.Enabled = rbtnConnectionString.Checked;
        }

        private void RbtnConnectionString_CheckedChanged(object sender, EventArgs e)
        {
            grpManualParameters.Enabled = rbtnParametrosManuales.Checked;
            grpConnectionString.Enabled = rbtnConnectionString.Checked;
        }
    }
}
