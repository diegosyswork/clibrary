﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Data.DbUtil
{
    internal partial class FrmDatosConexion : Form
    {
        public string Server { get; set; }
        public string InicioDeSesion { get; set; }
        public string Password { get; set; }
        public string BaseDeDatos { get; set; }
        public string MensajeError { get; set; }
        public FrmDatosConexion()
        {
            InitializeComponent();
        }

        private void FrmDatosConexion_Load(object sender, EventArgs e)
        {
            txtServidor.Text = Server;
            txtInicioDeSesion.Text = InicioDeSesion;
            txtPassword.Text = Password;
            txtBaseDeDatos.Text = BaseDeDatos;
            if (!string.IsNullOrEmpty(MensajeError))
            {
                txtMensajeError.Text = MensajeError;
                txtMensajeError.Refresh();

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

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}