using System;
using System.Windows.Forms;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.Forms.GetParameters
{
    /// <summary>
    /// Form to Get Data Parameters for MySQL
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    internal partial class FrmGetParamMySQL : Form
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
        /// Gets or sets the data base.
        /// </summary>
        /// <value>
        /// The data base.
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

        /// <summary>
        /// Gets the parameters used.
        /// </summary>
        /// <value>
        /// The parameters used.
        /// </value>
        public EConnectorParameterTypeUsed ParameterTypeUsed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmGetParamMySQL"/> class.
        /// </summary>
        public FrmGetParamMySQL()
        {
            InitializeComponent();
        }

        private void FrmGetParamMySQL_Load(object sender, EventArgs e)
        {
            txtServer.Text = Server;
            txtLogin.Text = Login;
            txtPassword.Text = Password;
            txtDataBase.Text = DataBase;

            txtConnectionString.Text = ConnectionString;

            rbtnConnectionString.Checked = (ParameterTypeUsed == EConnectorParameterTypeUsed.ConnectionString);

            txtErrMessage.Text = ErrMessage;
            txtErrMessage.Refresh();

            if (!string.IsNullOrEmpty(ErrMessage))
                this.Height = this.Height + txtErrMessage.Height;
            else
                this.Height = this.Height - txtErrMessage.Height;

            this.CenterToScreen();
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            Server = txtServer.Text;
            Login = txtLogin.Text;
            Password = txtPassword.Text;
            DataBase = txtDataBase.Text;

            ConnectionString = txtConnectionString.Text;

            ParameterTypeUsed = rbtnConnectionString.Checked ? EConnectorParameterTypeUsed.ConnectionString : EConnectorParameterTypeUsed.ManualParameters;

            if (rbtnConnectionString.Checked)
            {
                if (string.IsNullOrEmpty(ConnectionString.Trim()))
                {
                    MessageBox.Show("Se debe informar la cadena de conexion!!!", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DbUtil.IsValidConnectionString(EDatabaseEngine.MySql, ConnectionString.Trim(), out string errMessage))
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
