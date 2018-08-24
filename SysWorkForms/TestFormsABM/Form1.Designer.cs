namespace TestFormsABM
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label codClienteLabel;
            System.Windows.Forms.Label razonSocialLabel;
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.activoCheckBox = new System.Windows.Forms.CheckBox();
            this.codClienteTextBox = new System.Windows.Forms.TextBox();
            this.razonSocialTextBox = new System.Windows.Forms.TextBox();
            this.toolBarABM1 = new SysWork.Controls.Toolbars.ToolBarABM();
            codClienteLabel = new System.Windows.Forms.Label();
            razonSocialLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // codClienteLabel
            // 
            codClienteLabel.AutoSize = true;
            codClienteLabel.Location = new System.Drawing.Point(19, 27);
            codClienteLabel.Name = "codClienteLabel";
            codClienteLabel.Size = new System.Drawing.Size(43, 13);
            codClienteLabel.TabIndex = 10;
            codClienteLabel.Text = "Codigo:";
            // 
            // razonSocialLabel
            // 
            razonSocialLabel.AutoSize = true;
            razonSocialLabel.Location = new System.Drawing.Point(19, 53);
            razonSocialLabel.Name = "razonSocialLabel";
            razonSocialLabel.Size = new System.Drawing.Size(70, 13);
            razonSocialLabel.TabIndex = 12;
            razonSocialLabel.Text = "Razon Social";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.activoCheckBox);
            this.groupBox1.Controls.Add(codClienteLabel);
            this.groupBox1.Controls.Add(this.codClienteTextBox);
            this.groupBox1.Controls.Add(razonSocialLabel);
            this.groupBox1.Controls.Add(this.razonSocialTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 114);
            this.groupBox1.TabIndex = 102;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos del Cliente";
            // 
            // activoCheckBox
            // 
            this.activoCheckBox.Location = new System.Drawing.Point(93, 76);
            this.activoCheckBox.Name = "activoCheckBox";
            this.activoCheckBox.Size = new System.Drawing.Size(104, 24);
            this.activoCheckBox.TabIndex = 2;
            this.activoCheckBox.Text = "Activo";
            this.activoCheckBox.UseVisualStyleBackColor = true;
            // 
            // codClienteTextBox
            // 
            this.codClienteTextBox.Location = new System.Drawing.Point(93, 24);
            this.codClienteTextBox.MaxLength = 6;
            this.codClienteTextBox.Name = "codClienteTextBox";
            this.codClienteTextBox.Size = new System.Drawing.Size(93, 20);
            this.codClienteTextBox.TabIndex = 0;
            // 
            // razonSocialTextBox
            // 
            this.razonSocialTextBox.Location = new System.Drawing.Point(93, 50);
            this.razonSocialTextBox.MaxLength = 250;
            this.razonSocialTextBox.Name = "razonSocialTextBox";
            this.razonSocialTextBox.Size = new System.Drawing.Size(440, 20);
            this.razonSocialTextBox.TabIndex = 1;
            // 
            // toolBarABM1
            // 
            this.toolBarABM1.AutoSize = true;
            this.toolBarABM1.btnCancelarHabilitado = true;
            this.toolBarABM1.btnConsultarHabilitado = true;
            this.toolBarABM1.btnEliminarHabilitado = true;
            this.toolBarABM1.btnExportarHabilitado = false;
            this.toolBarABM1.btnGrabarHabilitado = true;
            this.toolBarABM1.btnNuevoHabilitado = true;
            this.toolBarABM1.btnRefreshHabilitado = true;
            this.toolBarABM1.btnReporteHabilitado = false;
            this.toolBarABM1.btnSalirHabilitado = true;
            this.toolBarABM1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolBarABM1.Location = new System.Drawing.Point(0, 0);
            this.toolBarABM1.Name = "toolBarABM1";
            this.toolBarABM1.Size = new System.Drawing.Size(592, 43);
            this.toolBarABM1.TabIndex = 103;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 176);
            this.Controls.Add(this.toolBarABM1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox activoCheckBox;
        private System.Windows.Forms.TextBox codClienteTextBox;
        private System.Windows.Forms.TextBox razonSocialTextBox;
        private SysWork.Controls.Toolbars.ToolBarABM toolBarABM1;
    }
}