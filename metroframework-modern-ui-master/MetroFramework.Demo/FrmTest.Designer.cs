namespace MetroFramework.Demo
{
    partial class FrmTest
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
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.BtnCajaYBancos = new System.Windows.Forms.Button();
            this.BtnVentas = new System.Windows.Forms.Button();
            this.BtnCompras = new System.Windows.Forms.Button();
            this.BtnMaestros = new System.Windows.Forms.Button();
            this.metroPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroPanel1
            // 
            this.metroPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(141)))), ((int)(((byte)(202)))));
            this.metroPanel1.Controls.Add(this.BtnCajaYBancos);
            this.metroPanel1.Controls.Add(this.BtnVentas);
            this.metroPanel1.Controls.Add(this.BtnCompras);
            this.metroPanel1.Controls.Add(this.BtnMaestros);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 30);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(250, 413);
            this.metroPanel1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroPanel1.TabIndex = 1;
            this.metroPanel1.UseCustomBackColor = true;
            this.metroPanel1.UseStyleColors = true;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // BtnCajaYBancos
            // 
            this.BtnCajaYBancos.FlatAppearance.BorderSize = 0;
            this.BtnCajaYBancos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.BtnCajaYBancos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCajaYBancos.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCajaYBancos.ForeColor = System.Drawing.Color.White;
            this.BtnCajaYBancos.Image = global::MetroFramework.Demo.Properties.Resources.pagos;
            this.BtnCajaYBancos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCajaYBancos.Location = new System.Drawing.Point(-3, 141);
            this.BtnCajaYBancos.Name = "BtnCajaYBancos";
            this.BtnCajaYBancos.Size = new System.Drawing.Size(250, 40);
            this.BtnCajaYBancos.TabIndex = 8;
            this.BtnCajaYBancos.Text = "&Caja y Bancos";
            this.BtnCajaYBancos.UseVisualStyleBackColor = true;
            // 
            // BtnVentas
            // 
            this.BtnVentas.FlatAppearance.BorderSize = 0;
            this.BtnVentas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.BtnVentas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnVentas.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnVentas.ForeColor = System.Drawing.Color.White;
            this.BtnVentas.Image = global::MetroFramework.Demo.Properties.Resources.reportes;
            this.BtnVentas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnVentas.Location = new System.Drawing.Point(-3, 95);
            this.BtnVentas.Name = "BtnVentas";
            this.BtnVentas.Size = new System.Drawing.Size(250, 40);
            this.BtnVentas.TabIndex = 7;
            this.BtnVentas.Text = "&Ventas";
            this.BtnVentas.UseVisualStyleBackColor = true;
            // 
            // BtnCompras
            // 
            this.BtnCompras.FlatAppearance.BorderSize = 0;
            this.BtnCompras.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.BtnCompras.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCompras.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCompras.ForeColor = System.Drawing.Color.White;
            this.BtnCompras.Image = global::MetroFramework.Demo.Properties.Resources.venta;
            this.BtnCompras.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCompras.Location = new System.Drawing.Point(-3, 49);
            this.BtnCompras.Name = "BtnCompras";
            this.BtnCompras.Size = new System.Drawing.Size(250, 40);
            this.BtnCompras.TabIndex = 6;
            this.BtnCompras.Text = "&Compras";
            this.BtnCompras.UseVisualStyleBackColor = true;
            // 
            // BtnMaestros
            // 
            this.BtnMaestros.FlatAppearance.BorderSize = 0;
            this.BtnMaestros.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.BtnMaestros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMaestros.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMaestros.ForeColor = System.Drawing.Color.White;
            this.BtnMaestros.Image = global::MetroFramework.Demo.Properties.Resources.empleados;
            this.BtnMaestros.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnMaestros.Location = new System.Drawing.Point(-3, 3);
            this.BtnMaestros.Name = "BtnMaestros";
            this.BtnMaestros.Size = new System.Drawing.Size(250, 40);
            this.BtnMaestros.TabIndex = 5;
            this.BtnMaestros.Text = "&Archivos";
            this.BtnMaestros.UseVisualStyleBackColor = true;
            // 
            // FrmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(716, 443);
            this.Controls.Add(this.metroPanel1);
            this.DisplayHeader = false;
            this.Name = "FrmTest";
            this.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Text = "FrmTest";
            this.metroPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MetroPanel metroPanel1;
        private System.Windows.Forms.Button BtnCajaYBancos;
        private System.Windows.Forms.Button BtnVentas;
        private System.Windows.Forms.Button BtnCompras;
        private System.Windows.Forms.Button BtnMaestros;
    }
}