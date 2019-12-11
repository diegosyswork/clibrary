namespace SysWork.Data.Common.FormsGetParam
{
    partial class FrmGetParamSQLite
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
            this.txtErrMessage = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.grpConnectionString = new System.Windows.Forms.GroupBox();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpConnectionString.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtErrMessage
            // 
            this.txtErrMessage.AutoSize = true;
            this.txtErrMessage.Location = new System.Drawing.Point(12, 189);
            this.txtErrMessage.MaximumSize = new System.Drawing.Size(232, 0);
            this.txtErrMessage.MinimumSize = new System.Drawing.Size(450, 0);
            this.txtErrMessage.Name = "txtErrMessage";
            this.txtErrMessage.Size = new System.Drawing.Size(450, 13);
            this.txtErrMessage.TabIndex = 23;
            this.txtErrMessage.Text = "Ha Ocurrido el siguiente error: ";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Location = new System.Drawing.Point(141, 207);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(85, 21);
            this.btnOk.TabIndex = 21;
            this.btnOk.Text = "&Aceptar";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnAceptar_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(263, 207);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 21);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "&Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::SysWork.Data.Properties.Resources.connector_icon_sqlite;
            this.pictureBox1.Location = new System.Drawing.Point(184, -9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 135);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // grpConnectionString
            // 
            this.grpConnectionString.Controls.Add(this.txtConnectionString);
            this.grpConnectionString.Controls.Add(this.label8);
            this.grpConnectionString.Location = new System.Drawing.Point(12, 132);
            this.grpConnectionString.Name = "grpConnectionString";
            this.grpConnectionString.Size = new System.Drawing.Size(458, 54);
            this.grpConnectionString.TabIndex = 26;
            this.grpConnectionString.TabStop = false;
            this.grpConnectionString.Text = "Datos de Conexion";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(108, 19);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(344, 20);
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
            // FrmGetParamSQLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 240);
            this.ControlBox = false;
            this.Controls.Add(this.grpConnectionString);
            this.Controls.Add(this.txtErrMessage);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmGetParamSQLite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parametros de Conexion SQLite";
            this.Load += new System.EventHandler(this.FrmGetParamSQLite_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpConnectionString.ResumeLayout(false);
            this.grpConnectionString.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txtErrMessage;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox grpConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label label8;
    }
}