namespace SysWork.MetroControls.MetroToolbars
{
    partial class MetroToolbarDisplaySettings
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
            this.ToolbarDisplatSettings = new System.Windows.Forms.ToolStrip();
            this.ButtonChangeTheme = new System.Windows.Forms.ToolStripButton();
            this.ButtonChangeStyle = new System.Windows.Forms.ToolStripButton();
            this.ToolbarDisplatSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolbarDisplatSettings
            // 
            this.ToolbarDisplatSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.ToolbarDisplatSettings.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolbarDisplatSettings.GripMargin = new System.Windows.Forms.Padding(0);
            this.ToolbarDisplatSettings.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolbarDisplatSettings.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ToolbarDisplatSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonChangeTheme,
            this.ButtonChangeStyle});
            this.ToolbarDisplatSettings.Location = new System.Drawing.Point(0, 0);
            this.ToolbarDisplatSettings.Name = "ToolbarDisplatSettings";
            this.ToolbarDisplatSettings.Padding = new System.Windows.Forms.Padding(0);
            this.ToolbarDisplatSettings.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolbarDisplatSettings.Size = new System.Drawing.Size(58, 31);
            this.ToolbarDisplatSettings.TabIndex = 2;
            this.ToolbarDisplatSettings.Text = "toolStrip1";
            // 
            // ButtonChangeTheme
            // 
            this.ButtonChangeTheme.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonChangeTheme.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.changeThemeLight;
            this.ButtonChangeTheme.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonChangeTheme.Name = "ButtonChangeTheme";
            this.ButtonChangeTheme.Size = new System.Drawing.Size(28, 28);
            this.ButtonChangeTheme.ToolTipText = "Cambiar Tema";
            this.ButtonChangeTheme.Click += new System.EventHandler(this.ButtonChangeTheme_Click);
            // 
            // ButtonChangeStyle
            // 
            this.ButtonChangeStyle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonChangeStyle.Image = global::SysWork.MetroControls.MetroToolbars.Properties.Resources.changeStyleLight;
            this.ButtonChangeStyle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonChangeStyle.Name = "ButtonChangeStyle";
            this.ButtonChangeStyle.Size = new System.Drawing.Size(28, 28);
            this.ButtonChangeStyle.ToolTipText = "Cambiar Estilo";
            this.ButtonChangeStyle.Click += new System.EventHandler(this.ButtonChangeStyle_Click);
            // 
            // MetroToolbarDisplaySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.Controls.Add(this.ToolbarDisplatSettings);
            this.Location = new System.Drawing.Point(-1, -1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MetroToolbarDisplaySettings";
            this.Size = new System.Drawing.Size(58, 29);
            this.Resize += new System.EventHandler(this.MetroToolbarDisplaySettings_Resize);
            this.ToolbarDisplatSettings.ResumeLayout(false);
            this.ToolbarDisplatSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolbarDisplatSettings;
        private System.Windows.Forms.ToolStripButton ButtonChangeTheme;
        private System.Windows.Forms.ToolStripButton ButtonChangeStyle;
    }
}
