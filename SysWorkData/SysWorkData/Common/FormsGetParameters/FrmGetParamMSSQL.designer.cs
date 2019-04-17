namespace SysWork.Data.Common.FormsGetParam
{
    partial class FrmGetParamSQL
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.txtMensajeError = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rbtnParametrosManuales = new System.Windows.Forms.RadioButton();
            this.grpParametrosManuales = new System.Windows.Forms.GroupBox();
            this.txtBaseDeDatos = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInicioDeSesion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServidor = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpConnectionString = new System.Windows.Forms.GroupBox();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.rbtnConnectionString = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpParametrosManuales.SuspendLayout();
            this.grpConnectionString.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(247, 347);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(85, 21);
            this.btnCancelar.TabIndex = 8;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAceptar.Location = new System.Drawing.Point(125, 347);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(85, 21);
            this.btnAceptar.TabIndex = 9;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // txtMensajeError
            // 
            this.txtMensajeError.AutoSize = true;
            this.txtMensajeError.Location = new System.Drawing.Point(23, 347);
            this.txtMensajeError.MaximumSize = new System.Drawing.Size(232, 0);
            this.txtMensajeError.MinimumSize = new System.Drawing.Size(410, 0);
            this.txtMensajeError.Name = "txtMensajeError";
            this.txtMensajeError.Size = new System.Drawing.Size(410, 13);
            this.txtMensajeError.TabIndex = 11;
            this.txtMensajeError.Text = "Ha Ocurrido el siguiente error: ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::SysWork.Data.Properties.Resources.connector_icon_mssqlserver;
            this.pictureBox1.Location = new System.Drawing.Point(169, -8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 135);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // rbtnParametrosManuales
            // 
            this.rbtnParametrosManuales.AutoSize = true;
            this.rbtnParametrosManuales.Checked = true;
            this.rbtnParametrosManuales.Location = new System.Drawing.Point(10, 110);
            this.rbtnParametrosManuales.Name = "rbtnParametrosManuales";
            this.rbtnParametrosManuales.Size = new System.Drawing.Size(127, 17);
            this.rbtnParametrosManuales.TabIndex = 12;
            this.rbtnParametrosManuales.TabStop = true;
            this.rbtnParametrosManuales.Text = "Parametros &Manuales";
            this.rbtnParametrosManuales.UseVisualStyleBackColor = true;
            this.rbtnParametrosManuales.CheckedChanged += new System.EventHandler(this.rbtnParametrosManuales_CheckedChanged);
            // 
            // grpParametrosManuales
            // 
            this.grpParametrosManuales.Controls.Add(this.txtBaseDeDatos);
            this.grpParametrosManuales.Controls.Add(this.label4);
            this.grpParametrosManuales.Controls.Add(this.txtPassword);
            this.grpParametrosManuales.Controls.Add(this.label3);
            this.grpParametrosManuales.Controls.Add(this.txtInicioDeSesion);
            this.grpParametrosManuales.Controls.Add(this.label2);
            this.grpParametrosManuales.Controls.Add(this.txtServidor);
            this.grpParametrosManuales.Controls.Add(this.label1);
            this.grpParametrosManuales.Location = new System.Drawing.Point(24, 133);
            this.grpParametrosManuales.Name = "grpParametrosManuales";
            this.grpParametrosManuales.Size = new System.Drawing.Size(410, 129);
            this.grpParametrosManuales.TabIndex = 13;
            this.grpParametrosManuales.TabStop = false;
            this.grpParametrosManuales.Text = "Datos de Conexion";
            // 
            // txtBaseDeDatos
            // 
            this.txtBaseDeDatos.Location = new System.Drawing.Point(108, 97);
            this.txtBaseDeDatos.Name = "txtBaseDeDatos";
            this.txtBaseDeDatos.Size = new System.Drawing.Size(232, 20);
            this.txtBaseDeDatos.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Base de Datos";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(108, 71);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(232, 20);
            this.txtPassword.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Password";
            // 
            // txtInicioDeSesion
            // 
            this.txtInicioDeSesion.Location = new System.Drawing.Point(108, 45);
            this.txtInicioDeSesion.Name = "txtInicioDeSesion";
            this.txtInicioDeSesion.Size = new System.Drawing.Size(232, 20);
            this.txtInicioDeSesion.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Inicio de Sesion";
            // 
            // txtServidor
            // 
            this.txtServidor.Location = new System.Drawing.Point(108, 19);
            this.txtServidor.Name = "txtServidor";
            this.txtServidor.Size = new System.Drawing.Size(232, 20);
            this.txtServidor.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Servidor";
            // 
            // grpConnectionString
            // 
            this.grpConnectionString.Controls.Add(this.txtConnectionString);
            this.grpConnectionString.Controls.Add(this.label8);
            this.grpConnectionString.Enabled = false;
            this.grpConnectionString.Location = new System.Drawing.Point(23, 291);
            this.grpConnectionString.Name = "grpConnectionString";
            this.grpConnectionString.Size = new System.Drawing.Size(411, 54);
            this.grpConnectionString.TabIndex = 16;
            this.grpConnectionString.TabStop = false;
            this.grpConnectionString.Text = "Datos de Conexion";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(108, 19);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(292, 20);
            this.txtConnectionString.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Connection String";
            // 
            // rbtnConnectionString
            // 
            this.rbtnConnectionString.AutoSize = true;
            this.rbtnConnectionString.Location = new System.Drawing.Point(10, 268);
            this.rbtnConnectionString.Name = "rbtnConnectionString";
            this.rbtnConnectionString.Size = new System.Drawing.Size(109, 17);
            this.rbtnConnectionString.TabIndex = 17;
            this.rbtnConnectionString.Text = "Connection &String";
            this.rbtnConnectionString.UseVisualStyleBackColor = true;
            this.rbtnConnectionString.CheckedChanged += new System.EventHandler(this.rbtnConnectionString_CheckedChanged);
            // 
            // FrmGetParamSQL
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(449, 380);
            this.ControlBox = false;
            this.Controls.Add(this.rbtnConnectionString);
            this.Controls.Add(this.grpConnectionString);
            this.Controls.Add(this.grpParametrosManuales);
            this.Controls.Add(this.rbtnParametrosManuales);
            this.Controls.Add(this.txtMensajeError);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmGetParamSQL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parametros de Conexion MSSQLServer";
            this.Load += new System.EventHandler(this.FrmDatosConexion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpParametrosManuales.ResumeLayout(false);
            this.grpParametrosManuales.PerformLayout();
            this.grpConnectionString.ResumeLayout(false);
            this.grpConnectionString.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label txtMensajeError;
        private System.Windows.Forms.RadioButton rbtnParametrosManuales;
        private System.Windows.Forms.GroupBox grpParametrosManuales;
        private System.Windows.Forms.TextBox txtBaseDeDatos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInicioDeSesion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServidor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rbtnConnectionString;
    }
}