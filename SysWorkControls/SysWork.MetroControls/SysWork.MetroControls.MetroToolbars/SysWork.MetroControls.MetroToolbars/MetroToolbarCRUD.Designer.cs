namespace SysWork.MetroControls.MetroToolbars
{
    partial class MetroToolbarCRUD
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
            this.ToolbarCRUD = new System.Windows.Forms.ToolStrip();
            this.ButtonNew = new System.Windows.Forms.ToolStripButton();
            this.ButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.ButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.ButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.ButtonImportExport = new System.Windows.Forms.ToolStripButton();
            this.ButtonReport = new System.Windows.Forms.ToolStripButton();
            this.ButtonInitialize = new System.Windows.Forms.ToolStripButton();
            this.ButtonSave = new System.Windows.Forms.ToolStripButton();
            this.ButtonExit = new System.Windows.Forms.ToolStripButton();
            this.ToolbarCRUD.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolbarCRUD
            // 
            this.ToolbarCRUD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.ToolbarCRUD.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolbarCRUD.GripMargin = new System.Windows.Forms.Padding(0);
            this.ToolbarCRUD.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolbarCRUD.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ToolbarCRUD.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonNew,
            this.ButtonDelete,
            this.ButtonRefresh,
            this.ButtonSearch,
            this.ButtonImportExport,
            this.ButtonReport,
            this.ButtonInitialize,
            this.ButtonSave,
            this.ButtonExit});
            this.ToolbarCRUD.Location = new System.Drawing.Point(0, 0);
            this.ToolbarCRUD.Name = "ToolbarCRUD";
            this.ToolbarCRUD.Padding = new System.Windows.Forms.Padding(0);
            this.ToolbarCRUD.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolbarCRUD.Size = new System.Drawing.Size(254, 31);
            this.ToolbarCRUD.TabIndex = 2;
            this.ToolbarCRUD.Text = "toolStrip1";
            // 
            // ButtonNew
            // 
            this.ButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonNew.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.newLight;
            this.ButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonNew.Name = "ButtonNew";
            this.ButtonNew.Size = new System.Drawing.Size(28, 28);
            this.ButtonNew.Text = "TsBtnNew";
            this.ButtonNew.ToolTipText = "Nuevo (F3)";
            this.ButtonNew.Click += new System.EventHandler(this.ButtonNew_Click);
            // 
            // ButtonDelete
            // 
            this.ButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonDelete.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.deleteLight;
            this.ButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonDelete.Name = "ButtonDelete";
            this.ButtonDelete.Size = new System.Drawing.Size(28, 28);
            this.ButtonDelete.Text = "toolStripButton1";
            this.ButtonDelete.ToolTipText = "Eliminar (F10)";
            this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
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
            this.ButtonImportExport.ToolTipText = "Importar / Exportar";
            this.ButtonImportExport.Click += new System.EventHandler(this.ButtonImportExport_Click);
            // 
            // ButtonReport
            // 
            this.ButtonReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonReport.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.reportLight;
            this.ButtonReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonReport.Name = "ButtonReport";
            this.ButtonReport.Size = new System.Drawing.Size(28, 28);
            this.ButtonReport.Text = "toolStripButton6";
            this.ButtonReport.ToolTipText = "Reporte (F4)";
            this.ButtonReport.Click += new System.EventHandler(this.ButtonReport_Click);
            // 
            // ButtonInitialize
            // 
            this.ButtonInitialize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonInitialize.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.initializeLight;
            this.ButtonInitialize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonInitialize.Name = "ButtonInitialize";
            this.ButtonInitialize.Size = new System.Drawing.Size(28, 28);
            this.ButtonInitialize.Text = "toolStripButton5";
            this.ButtonInitialize.ToolTipText = "Inicializar Formulario (ESC)";
            this.ButtonInitialize.Click += new System.EventHandler(this.ButtonInitialize_Click);
            // 
            // ButtonSave
            // 
            this.ButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSave.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.saveLight;
            this.ButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(28, 28);
            this.ButtonSave.Text = "toolStripButton7";
            this.ButtonSave.ToolTipText = "Grabar (F2)";
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
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
            // MetroToolbarCRUD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.Controls.Add(this.ToolbarCRUD);
            this.Location = new System.Drawing.Point(-1, -1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MetroToolbarCRUD";
            this.Size = new System.Drawing.Size(254, 29);
            this.Resize += new System.EventHandler(this.MetroToolbarCRUD_Resize);
            this.ToolbarCRUD.ResumeLayout(false);
            this.ToolbarCRUD.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolbarCRUD;
        private System.Windows.Forms.ToolStripButton ButtonNew;
        private System.Windows.Forms.ToolStripButton ButtonDelete;
        private System.Windows.Forms.ToolStripButton ButtonRefresh;
        private System.Windows.Forms.ToolStripButton ButtonSearch;
        private System.Windows.Forms.ToolStripButton ButtonImportExport;
        private System.Windows.Forms.ToolStripButton ButtonInitialize;
        private System.Windows.Forms.ToolStripButton ButtonReport;
        private System.Windows.Forms.ToolStripButton ButtonSave;
        private System.Windows.Forms.ToolStripButton ButtonExit;
    }
}
