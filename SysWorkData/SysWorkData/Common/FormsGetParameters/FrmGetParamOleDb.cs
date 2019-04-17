﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SysWork.Data.Common.Utilities;

namespace SysWork.Data.Common.FormsGetParam
{
    public partial class FrmGetParamOleDb : Form
    {
        public string ConnectionString { get; set; }
        public string MensajeError { get; set; }

        public FrmGetParamOleDb()
        {
            InitializeComponent();
        }

        private void FrmGetParamOleDb_Load(object sender, EventArgs e)
        {
            txtConnectionString.Text = ConnectionString;

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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
