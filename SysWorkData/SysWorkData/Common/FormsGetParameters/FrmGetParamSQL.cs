using System;
using System.Windows.Forms;

namespace SysWork.Data.Common.FormsGetParam
{
    internal partial class FrmGetParamSQL : Form
    {
        public string Server { get; set; }
        public string InicioDeSesion { get; set; }
        public string Password { get; set; }
        public string BaseDeDatos { get; set; }
        public string ConnectionString { get; set; }
        public string MensajeError { get; set; }
        public FrmGetParamSQL()
        {
            InitializeComponent();
        }

        private void FrmDatosConexion_Load(object sender, EventArgs e)
        {
            txtServidor.Text = Server;
            txtInicioDeSesion.Text = InicioDeSesion;
            txtPassword.Text = Password;
            txtBaseDeDatos.Text = BaseDeDatos;

            txtConnectionString.Text = ConnectionString;

            rbtnConnectionString.Checked = !string.IsNullOrEmpty(ConnectionString);

            txtMensajeError.Text = MensajeError;
            txtMensajeError.Refresh();

            if (!string.IsNullOrEmpty(MensajeError))
            {
                this.Height = this.Height + txtMensajeError.Height;
            }
            else
            {
                this.Height = this.Height - txtMensajeError.Height;
            }

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Server = txtServidor.Text;
            InicioDeSesion = txtInicioDeSesion.Text;
            Password = txtPassword.Text;
            BaseDeDatos = txtBaseDeDatos.Text;

            ConnectionString = txtConnectionString.Text;

            if (rbtnConnectionString.Checked && string.IsNullOrEmpty(ConnectionString.Trim()))
            {
                MessageBox.Show("Se debe informar la cadena de conexion!!!","Aviso al operador",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void rbtnParametrosManuales_CheckedChanged(object sender, EventArgs e)
        {
            grpParametrosManuales.Enabled = rbtnParametrosManuales.Checked;
            grpConnectionString.Enabled = rbtnConnectionString.Checked;
        }

        private void rbtnConnectionString_CheckedChanged(object sender, EventArgs e)
        {
            grpParametrosManuales.Enabled = rbtnParametrosManuales.Checked;
            grpConnectionString.Enabled = rbtnConnectionString.Checked;
        }
    }
}
