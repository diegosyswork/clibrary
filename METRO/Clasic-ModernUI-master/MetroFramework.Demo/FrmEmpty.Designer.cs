namespace MetroFramework.Demo
{
    partial class FrmEmpty
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
            this.metroButtonStyled1 = new MetroFramework.Controls.MetroButtonStyled();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.metroButton3 = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroButtonStyled1
            // 
            this.metroButtonStyled1.FlatAppearance = false;
            this.metroButtonStyled1.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.metroButtonStyled1.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.metroButtonStyled1.Highlight = false;
            this.metroButtonStyled1.Location = new System.Drawing.Point(98, 127);
            this.metroButtonStyled1.Name = "metroButtonStyled1";
            this.metroButtonStyled1.Size = new System.Drawing.Size(83, 52);
            this.metroButtonStyled1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroButtonStyled1.StyleManager = null;
            this.metroButtonStyled1.TabIndex = 0;
            this.metroButtonStyled1.Text = "metroButtonStyled1";
            this.metroButtonStyled1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(99, 212);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(62, 24);
            this.metroButton1.TabIndex = 1;
            this.metroButton1.Text = "metroButton1";
            this.metroButton1.UseSelectable = true;
            // 
            // metroButton2
            // 
            this.metroButton2.Location = new System.Drawing.Point(120, 278);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(102, 17);
            this.metroButton2.TabIndex = 2;
            this.metroButton2.Text = "metroButton2";
            this.metroButton2.UseSelectable = true;
            // 
            // metroButton3
            // 
            this.metroButton3.Location = new System.Drawing.Point(278, 114);
            this.metroButton3.Name = "metroButton3";
            this.metroButton3.Size = new System.Drawing.Size(66, 37);
            this.metroButton3.TabIndex = 3;
            this.metroButton3.Text = "metroButton3";
            this.metroButton3.UseSelectable = true;
            // 
            // FrmEmpty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.metroButton3);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.metroButtonStyled1);
            this.Name = "FrmEmpty";
            this.Text = "FrmEmpty";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MetroButtonStyled metroButtonStyled1;
        private Controls.MetroButton metroButton1;
        private Controls.MetroButton metroButton2;
        private Controls.MetroButton metroButton3;
    }
}