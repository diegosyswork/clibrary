namespace SysWork.Controls.Toolbars
{
    partial class ToolBarReporte
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbarReporte = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnConsultar = new System.Windows.Forms.ToolStripButton();
            this.btnExportar = new System.Windows.Forms.ToolStripButton();
            this.btnSalir = new System.Windows.Forms.ToolStripButton();
            this.btnPantalla = new System.Windows.Forms.ToolStripButton();
            this.btnImprimir = new System.Windows.Forms.ToolStripButton();
            this.tbarReporte.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbarReporte
            // 
            this.tbarReporte.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.tbarReporte.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPantalla,
            this.btnImprimir,
            this.btnExportar,
            this.btnRefresh,
            this.btnConsultar,
            this.btnSalir});
            this.tbarReporte.Location = new System.Drawing.Point(0, 0);
            this.tbarReporte.Name = "tbarReporte";
            this.tbarReporte.Size = new System.Drawing.Size(540, 43);
            this.tbarReporte.TabIndex = 11;
            this.tbarReporte.Text = "toolStrip1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = global::SysWork.Controls.Properties.Resources.refresh_48x48;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(40, 40);
            this.btnRefresh.Text = "toolStripButton3";
            this.btnRefresh.ToolTipText = "Inicializar Formulario (F10)";
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // btnConsultar
            // 
            this.btnConsultar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConsultar.Image = global::SysWork.Controls.Properties.Resources.consulta_48x48;
            this.btnConsultar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(40, 40);
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.ToolTipText = "Consultar (F5)";
            this.btnConsultar.Click += new System.EventHandler(this.BtnConsultar_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExportar.Enabled = false;
            this.btnExportar.Image = global::SysWork.Controls.Properties.Resources.file_export;
            this.btnExportar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(40, 40);
            this.btnExportar.Text = "Exportar";
            this.btnExportar.Click += new System.EventHandler(this.BtnExportar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSalir.Image = global::SysWork.Controls.Properties.Resources.salir_48x48;
            this.btnSalir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(40, 40);
            this.btnSalir.Text = "Salir";
            this.btnSalir.ToolTipText = "Salir (ESC)";
            this.btnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // btnPantalla
            // 
            this.btnPantalla.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPantalla.Image = global::SysWork.Controls.Properties.Resources.pantalla_48x48;
            this.btnPantalla.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPantalla.Name = "btnPantalla";
            this.btnPantalla.Size = new System.Drawing.Size(40, 40);
            this.btnPantalla.Text = "toolStripButton1";
            this.btnPantalla.ToolTipText = "Previsualizar por pantalla";
            this.btnPantalla.Click += new System.EventHandler(this.BtnPantalla_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnImprimir.Image = global::SysWork.Controls.Properties.Resources.printer;
            this.btnImprimir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(40, 40);
            this.btnImprimir.Text = "toolStripButton1";
            this.btnImprimir.ToolTipText = "Imprimir";
            this.btnImprimir.Click += new System.EventHandler(this.BtnImprimir_Click);
            // 
            // ToolBarReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tbarReporte);
            this.Name = "ToolBarReportes";
            this.Size = new System.Drawing.Size(540, 43);
            this.tbarReporte.ResumeLayout(false);
            this.tbarReporte.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tbarReporte;
        private System.Windows.Forms.ToolStripButton btnPantalla;
        private System.Windows.Forms.ToolStripButton btnImprimir;
        private System.Windows.Forms.ToolStripButton btnExportar;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnConsultar;
        private System.Windows.Forms.ToolStripButton btnSalir;





    }
}
