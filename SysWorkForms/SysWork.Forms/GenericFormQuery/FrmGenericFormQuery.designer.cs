namespace SysWork.Forms.GenericFormQuery
{
    partial class FrmGenericFormQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGenericFormQuery));
            this.listView1 = new System.Windows.Forms.ListView();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtCriterioBusqueda = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblCantRegistros = new System.Windows.Forms.Label();
            this.tsSeleccion = new System.Windows.Forms.ToolStrip();
            this.tsbSelectAll = new System.Windows.Forms.ToolStripButton();
            this.tsbUnSelectAll = new System.Windows.Forms.ToolStripButton();
            this.tsbInvert = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tsSeleccion.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 59);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(598, 242);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAceptar.Location = new System.Drawing.Point(182, 328);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(96, 24);
            this.btnAceptar.TabIndex = 1;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancelar.Location = new System.Drawing.Point(345, 328);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(96, 24);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // txtCriterioBusqueda
            // 
            this.txtCriterioBusqueda.Location = new System.Drawing.Point(69, 15);
            this.txtCriterioBusqueda.Name = "txtCriterioBusqueda";
            this.txtCriterioBusqueda.Size = new System.Drawing.Size(385, 20);
            this.txtCriterioBusqueda.TabIndex = 4;
            this.txtCriterioBusqueda.TextChanged += new System.EventHandler(this.txtCriterioBusqueda_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(381, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "<Ingresa un criterio de busqueda, por cualquiera de los campos de la consulta>";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SysWork.Forms.Properties.Resources.consulta_48x48;
            this.pictureBox1.Location = new System.Drawing.Point(28, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 39);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // lblCantRegistros
            // 
            this.lblCantRegistros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCantRegistros.AutoSize = true;
            this.lblCantRegistros.Location = new System.Drawing.Point(462, 312);
            this.lblCantRegistros.Name = "lblCantRegistros";
            this.lblCantRegistros.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblCantRegistros.Size = new System.Drawing.Size(148, 13);
            this.lblCantRegistros.TabIndex = 7;
            this.lblCantRegistros.Text = "Cant. de Registros: 1.000.000";
            // 
            // tsSeleccion
            // 
            this.tsSeleccion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tsSeleccion.Dock = System.Windows.Forms.DockStyle.None;
            this.tsSeleccion.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsSeleccion.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSelectAll,
            this.tsbUnSelectAll,
            this.tsbInvert});
            this.tsSeleccion.Location = new System.Drawing.Point(15, 309);
            this.tsSeleccion.Name = "tsSeleccion";
            this.tsSeleccion.Size = new System.Drawing.Size(103, 25);
            this.tsSeleccion.TabIndex = 8;
            this.tsSeleccion.Text = "toolStrip1";
            // 
            // tsbSelectAll
            // 
            this.tsbSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbSelectAll.Image")));
            this.tsbSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSelectAll.Name = "tsbSelectAll";
            this.tsbSelectAll.Size = new System.Drawing.Size(23, 22);
            this.tsbSelectAll.Text = "Seleccionar Todo";
            this.tsbSelectAll.Click += new System.EventHandler(this.tsbSelectAll_Click);
            // 
            // tsbUnSelectAll
            // 
            this.tsbUnSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUnSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbUnSelectAll.Image")));
            this.tsbUnSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUnSelectAll.Name = "tsbUnSelectAll";
            this.tsbUnSelectAll.Size = new System.Drawing.Size(23, 22);
            this.tsbUnSelectAll.Text = "Deseleccionar Todo";
            this.tsbUnSelectAll.Click += new System.EventHandler(this.tsbUnSelectAll_Click);
            // 
            // tsbInvert
            // 
            this.tsbInvert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbInvert.Image = ((System.Drawing.Image)(resources.GetObject("tsbInvert.Image")));
            this.tsbInvert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbInvert.Name = "tsbInvert";
            this.tsbInvert.Size = new System.Drawing.Size(23, 22);
            this.tsbInvert.Text = "Invertir Seleccion";
            this.tsbInvert.Click += new System.EventHandler(this.tsbInvert_Click);
            // 
            // FrmGenericFormQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 364);
            this.Controls.Add(this.tsSeleccion);
            this.Controls.Add(this.lblCantRegistros);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtCriterioBusqueda);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.listView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGenericFormQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmConsultaGenerica";
            this.Shown += new System.EventHandler(this.FrmConsultaGenerica_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tsSeleccion.ResumeLayout(false);
            this.tsSeleccion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtCriterioBusqueda;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblCantRegistros;
        private System.Windows.Forms.ToolStripButton tsbSelectAll;
        private System.Windows.Forms.ToolStripButton tsbUnSelectAll;
        private System.Windows.Forms.ToolStripButton tsbInvert;
        public System.Windows.Forms.ToolStrip tsSeleccion;
    }
}