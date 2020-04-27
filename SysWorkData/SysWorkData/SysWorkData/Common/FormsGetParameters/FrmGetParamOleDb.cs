using System;
using System.Windows.Forms;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.FormsGetParam
{
    /// <summary>
    /// Form to Get Data Parameters for OleDb.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class FrmGetParamOleDb : Form
    {
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
        /// Initializes a new instance of the <see cref="FrmGetParamOleDb"/> class.
        /// </summary>
        public FrmGetParamOleDb()
        {
            InitializeComponent();
        }

        private void FrmGetParamOleDb_Load(object sender, EventArgs e)
        {
            txtConnectionString.Text = ConnectionString;

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

            if (string.IsNullOrEmpty( txtConnectionString.Text))
            {
                MessageBox.Show("Se debe informar la cadena de conexion!!!", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (!DbUtil.IsValidConnectionString(EDataBaseEngine.OleDb, txtConnectionString.Text,out string errMessage))
            {
                MessageBox.Show($"Error en el formato de la cadena de conexion \r\n \r\n detalle del error: \r\n \r\n {errMessage} ", "Aviso al operador", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ConnectionString = txtConnectionString.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
