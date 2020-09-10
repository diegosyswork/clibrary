namespace SysWork.MetroControls.MetroToolbars
{
    partial class MetroToolbarReport
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.ToolbarReport = new System.Windows.Forms.ToolStrip();
            this.ButtonDisplay = new System.Windows.Forms.ToolStripButton();
            this.ButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.ButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.ButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.ButtonImportExport = new System.Windows.Forms.ToolStripButton();
            this.ButtonCleanFilters = new System.Windows.Forms.ToolStripButton();
            this.ButtonExit = new System.Windows.Forms.ToolStripButton();
            this.ToolbarReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolbarReport
            // 
            this.ToolbarReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.ToolbarReport.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolbarReport.GripMargin = new System.Windows.Forms.Padding(0);
            this.ToolbarReport.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolbarReport.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ToolbarReport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonDisplay,
            this.ButtonPrint,
            this.ButtonImportExport,
            this.ButtonRefresh,
            this.ButtonSearch,
            this.ButtonCleanFilters,
            this.ButtonExit});
            this.ToolbarReport.Location = new System.Drawing.Point(0, 0);
            this.ToolbarReport.Name = "ToolbarReport";
            this.ToolbarReport.Padding = new System.Windows.Forms.Padding(0);
            this.ToolbarReport.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolbarReport.Size = new System.Drawing.Size(198, 31);
            this.ToolbarReport.TabIndex = 2;
            this.ToolbarReport.Text = "toolStrip1";
            // 
            // ButtonDisplay
            // 
            this.ButtonDisplay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonDisplay.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.monitorLight;
            this.ButtonDisplay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonDisplay.Name = "ButtonDisplay";
            this.ButtonDisplay.Size = new System.Drawing.Size(28, 28);
            this.ButtonDisplay.ToolTipText = "Visualizar por Pantalla (F2)";
            this.ButtonDisplay.Click += new System.EventHandler(this.ButtonDisplay_Click);
            // 
            // ButtonPrint
            // 
            this.ButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonPrint.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.printLight;
            this.ButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonPrint.Name = "ButtonPrint";
            this.ButtonPrint.Size = new System.Drawing.Size(28, 28);
            this.ButtonPrint.ToolTipText = "Imprimir (F3)";
            this.ButtonPrint.Click += new System.EventHandler(this.ButtonPrint_Click);
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonRefresh.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.refreshLight;
            this.ButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Size = new System.Drawing.Size(28, 28);
            this.ButtonRefresh.Text = "toolStripButton2";
            this.ButtonRefresh.ToolTipText = "Refresh (F12)";
            this.ButtonRefresh.Click += new System.EventHandler(this.ButtonRefresh_Click);
            // 
            // ButtonSearch
            // 
            this.ButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSearch.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.searchLight;
            this.ButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSearch.Name = "ButtonSearch";
            this.ButtonSearch.Size = new System.Drawing.Size(28, 28);
            this.ButtonSearch.Text = "toolStripButton3";
            this.ButtonSearch.ToolTipText = "Buscar (F5)";
            this.ButtonSearch.Click += new System.EventHandler(this.ButtonSearch_Click);
            // 
            // ButtonImportExport
            // 
            this.ButtonImportExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonImportExport.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.importExportLight;
            this.ButtonImportExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonImportExport.Name = "ButtonImportExport";
            this.ButtonImportExport.Size = new System.Drawing.Size(28, 28);
            this.ButtonImportExport.Text = "toolStripButton4";
            this.ButtonImportExport.ToolTipText = "Exportar";
            this.ButtonImportExport.Click += new System.EventHandler(this.ButtonImportExport_Click);
            // 
            // ButtonCleanFilters
            // 
            this.ButtonCleanFilters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonCleanFilters.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.clearFiltersLight;
            this.ButtonCleanFilters.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonCleanFilters.Name = "ButtonCleanFilters";
            this.ButtonCleanFilters.Size = new System.Drawing.Size(28, 28);
            this.ButtonCleanFilters.Text = "toolStripButton6";
            this.ButtonCleanFilters.ToolTipText = "Limpiar Filtros";
            this.ButtonCleanFilters.Click += new System.EventHandler(this.ButtonCleanFilters_Click);
            // 
            // ButtonExit
            // 
            this.ButtonExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonExit.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.closeLight;
            this.ButtonExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonExit.Name = "ButtonExit";
            this.ButtonExit.Size = new System.Drawing.Size(28, 28);
            this.ButtonExit.Text = "toolStripButton8";
            this.ButtonExit.ToolTipText = "Salir (ESC)";
            this.ButtonExit.Click += new System.EventHandler(this.ButtonExit_Click);
            // 
            // MetroToolbarReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.Controls.Add(this.ToolbarReport);
            this.Location = new System.Drawing.Point(-1, -1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MetroToolbarReport";
            this.Size = new System.Drawing.Size(199, 29);
            this.Resize += new System.EventHandler(this.MetroToolbarReport_Resize);
            this.ToolbarReport.ResumeLayout(false);
            this.ToolbarReport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolbarReport;
        private System.Windows.Forms.ToolStripButton ButtonDisplay;
        private System.Windows.Forms.ToolStripButton ButtonPrint;
        private System.Windows.Forms.ToolStripButton ButtonRefresh;
        private System.Windows.Forms.ToolStripButton ButtonSearch;
        private System.Windows.Forms.ToolStripButton ButtonImportExport;
        private System.Windows.Forms.ToolStripButton ButtonCleanFilters;
        private System.Windows.Forms.ToolStripButton ButtonExit;
    }
}
