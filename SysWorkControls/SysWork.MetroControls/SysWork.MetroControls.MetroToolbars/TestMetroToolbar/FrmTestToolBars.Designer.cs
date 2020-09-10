namespace TestMetroToolbar
{
    partial class FrmTestToolBars
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
            this.metroToolbarDisplaySettings1 = new SysWork.MetroControls.MetroToolbars.MetroToolbarDisplaySettings();
            this.metroToolbarReport1 = new SysWork.MetroControls.MetroToolbars.MetroToolbarReport();
            this.metroToolbarCRUD1 = new SysWork.MetroControls.MetroToolbars.MetroToolbarCRUD();
            this.SuspendLayout();
            // 
            // metroToolbarDisplaySettings1
            // 
            this.metroToolbarDisplaySettings1.BackColor = System.Drawing.Color.White;
            this.metroToolbarDisplaySettings1.ChangeStyleEnabled = true;
            this.metroToolbarDisplaySettings1.ChangeThemeEnabled = true;
            this.metroToolbarDisplaySettings1.Location = new System.Drawing.Point(9, 9);
            this.metroToolbarDisplaySettings1.Margin = new System.Windows.Forms.Padding(0);
            this.metroToolbarDisplaySettings1.Name = "metroToolbarDisplaySettings1";
            this.metroToolbarDisplaySettings1.Size = new System.Drawing.Size(59, 29);
            this.metroToolbarDisplaySettings1.TabIndex = 2;
            this.metroToolbarDisplaySettings1.Theme = SysWork.MetroControls.MetroToolbars.MetroTheme.Light;
            this.metroToolbarDisplaySettings1.ActionSelected += new SysWork.MetroControls.MetroToolbars.MetroToolbarDisplaySettings.MetroToolbarDisplaySettingsClickEventHandler(this.metroToolbarDisplaySettings1_ActionSelected);
            // 
            // metroToolbarReport1
            // 
            this.metroToolbarReport1.BackColor = System.Drawing.Color.White;
            this.metroToolbarReport1.ClenFiltersEnabled = true;
            this.metroToolbarReport1.DisplayEnabled = true;
            this.metroToolbarReport1.ExitEnabled = true;
            this.metroToolbarReport1.ImportExportEnabled = true;
            this.metroToolbarReport1.Location = new System.Drawing.Point(9, 67);
            this.metroToolbarReport1.Margin = new System.Windows.Forms.Padding(0);
            this.metroToolbarReport1.Name = "metroToolbarReport1";
            this.metroToolbarReport1.PrintEnabled = true;
            this.metroToolbarReport1.RefreshEnabled = true;
            this.metroToolbarReport1.SearchEnabled = true;
            this.metroToolbarReport1.Size = new System.Drawing.Size(198, 29);
            this.metroToolbarReport1.TabIndex = 1;
            this.metroToolbarReport1.Theme = SysWork.MetroControls.MetroToolbars.MetroTheme.Light;
            this.metroToolbarReport1.ActionSelected += new SysWork.MetroControls.MetroToolbars.MetroToolbarReport.MetroToolbarReportClickEventHandler(this.metroToolbarReport1_ActionSelected);
            // 
            // metroToolbarCRUD1
            // 
            this.metroToolbarCRUD1.BackColor = System.Drawing.Color.White;
            this.metroToolbarCRUD1.DeleteEnabled = true;
            this.metroToolbarCRUD1.ExitEnabled = true;
            this.metroToolbarCRUD1.ImportExportEnabled = true;
            this.metroToolbarCRUD1.Location = new System.Drawing.Point(9, 38);
            this.metroToolbarCRUD1.Margin = new System.Windows.Forms.Padding(0);
            this.metroToolbarCRUD1.Name = "metroToolbarCRUD1";
            this.metroToolbarCRUD1.NewEnabled = true;
            this.metroToolbarCRUD1.RefreshEnabled = true;
            this.metroToolbarCRUD1.ReportEnabled = true;
            this.metroToolbarCRUD1.SaveEnabled = true;
            this.metroToolbarCRUD1.SearchEnabled = true;
            this.metroToolbarCRUD1.Size = new System.Drawing.Size(252, 29);
            this.metroToolbarCRUD1.TabIndex = 0;
            this.metroToolbarCRUD1.Theme = SysWork.MetroControls.MetroToolbars.MetroTheme.Light;
            this.metroToolbarCRUD1.ActionSelected += new SysWork.MetroControls.MetroToolbars.MetroToolbarCRUD.MetroToolbarCRUDClickEventHandler(this.metroToolbarCRUD1_ActionSelected);
            // 
            // FrmTestToolBars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.metroToolbarDisplaySettings1);
            this.Controls.Add(this.metroToolbarReport1);
            this.Controls.Add(this.metroToolbarCRUD1);
            this.Name = "FrmTestToolBars";
            this.Text = "FrmTestToolBars";
            this.ResumeLayout(false);

        }

        #endregion

        private SysWork.MetroControls.MetroToolbars.MetroToolbarCRUD metroToolbarCRUD1;
        private SysWork.MetroControls.MetroToolbars.MetroToolbarReport metroToolbarReport1;
        private SysWork.MetroControls.MetroToolbars.MetroToolbarDisplaySettings metroToolbarDisplaySettings1;
    }
}