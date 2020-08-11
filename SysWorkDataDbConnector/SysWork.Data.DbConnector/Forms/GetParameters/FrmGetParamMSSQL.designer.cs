namespace SysWork.Data.Common.Forms.GetParameters
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblErrMessage = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rbtnParametrosManuales = new System.Windows.Forms.RadioButton();
            this.grpManualParameters = new System.Windows.Forms.GroupBox();
            this.txtDataBase = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpConnectionString = new System.Windows.Forms.GroupBox();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.rbtnConnectionString = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpManualParameters.SuspendLayout();
            this.grpConnectionString.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(308, 418);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 21);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Location = new System.Drawing.Point(164, 418);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(85, 21);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "&Aceptar";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnAceptar_Click);
            // 
            // lblErrMessage
            // 
            this.lblErrMessage.AutoSize = true;
            this.lblErrMessage.Location = new System.Drawing.Point(12, 402);
            this.lblErrMessage.MaximumSize = new System.Drawing.Size(232, 0);
            this.lblErrMessage.MinimumSize = new System.Drawing.Size(410, 0);
            this.lblErrMessage.Name = "lblErrMessage";
            this.lblErrMessage.Size = new System.Drawing.Size(410, 13);
            this.lblErrMessage.TabIndex = 11;
            this.lblErrMessage.Text = "Ha Ocurrido el siguiente error: ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::SysWork.Data.DbConnector.Properties.Resources.connector_icon_mssqlserver;
            this.pictureBox1.Location = new System.Drawing.Point(198, -8);
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
            this.rbtnParametrosManuales.CheckedChanged += new System.EventHandler(this.RbtnParametrosManuales_CheckedChanged);
            // 
            // grpManualParameters
            // 
            this.grpManualParameters.Controls.Add(this.txtDataBase);
            this.grpManualParameters.Controls.Add(this.label4);
            this.grpManualParameters.Controls.Add(this.txtPassword);
            this.grpManualParameters.Controls.Add(this.label3);
            this.grpManualParameters.Controls.Add(this.txtLogin);
            this.grpManualParameters.Controls.Add(this.label2);
            this.grpManualParameters.Controls.Add(this.txtServer);
            this.grpManualParameters.Controls.Add(this.label1);
            this.grpManualParameters.Location = new System.Drawing.Point(12, 133);
            this.grpManualParameters.Name = "grpManualParameters";
            this.grpManualParameters.Size = new System.Drawing.Size(480, 129);
            this.grpManualParameters.TabIndex = 13;
            this.grpManualParameters.TabStop = false;
            this.grpManualParameters.Text = "Datos de Conexion";
            // 
            // txtDataBase
            // 
            this.txtDataBase.Location = new System.Drawing.Point(108, 97);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(361, 20);
            this.txtDataBase.TabIndex = 15;
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
            this.txtPassword.Size = new System.Drawing.Size(361, 20);
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
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(108, 45);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(361, 20);
            this.txtLogin.TabIndex = 11;
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
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(108, 19);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(361, 20);
            this.txtServer.TabIndex = 9;
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
            this.grpConnectionString.Location = new System.Drawing.Point(12, 291);
            this.grpConnectionString.Name = "grpConnectionString";
            this.grpConnectionString.Size = new System.Drawing.Size(480, 101);
            this.grpConnectionString.TabIndex = 16;
            this.grpConnectionString.TabStop = false;
            this.grpConnectionString.Text = "Datos de Conexion";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(108, 19);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConnectionString.Size = new System.Drawing.Size(365, 70);
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
            this.rbtnConnectionString.CheckedChanged += new System.EventHandler(this.RbtnConnectionString_CheckedChanged);
            // 
            // FrmGetParamSQL
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(504, 451);
            this.ControlBox = false;
            this.Controls.Add(this.rbtnConnectionString);
            this.Controls.Add(this.grpConnectionString);
            this.Controls.Add(this.grpManualParameters);
            this.Controls.Add(this.rbtnParametrosManuales);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblErrMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmGetParamSQL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parametros de Conexion MSSQLServer";
            this.Load += new System.EventHandler(this.FrmGetParamMSSQL_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpManualParameters.ResumeLayout(false);
            this.grpManualParameters.PerformLayout();
            this.grpConnectionString.ResumeLayout(false);
            this.grpConnectionString.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblErrMessage;
        private System.Windows.Forms.RadioButton rbtnParametrosManuales;
        private System.Windows.Forms.GroupBox grpManualParameters;
        private System.Windows.Forms.TextBox txtDataBase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rbtnConnectionString;
    }
}