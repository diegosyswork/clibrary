using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MetroFramework.Demo
{
    public partial class FrmMenu : Form
    {
        private Controls.MetroPanel metroPanelIconos;

        private int _defaultWidthMetroTile = 146;
        private int _defaultHeightMetroTile = 146;

        public FrmMenu()
        {
            InitializeComponent();
        }

        void CrearIconosMaestros()
        {
            this.SuspendLayout();
            metroPanel1.SuspendLayout();

            Controls.MetroTile metroTileProveedores;
            Controls.MetroTile metroTileClientes;
            Controls.MetroTile metroTileArticulos;

            metroPanelIconos = new MetroFramework.Controls.MetroPanel();
            metroTileProveedores = new MetroFramework.Controls.MetroTile();
            metroTileClientes = new MetroFramework.Controls.MetroTile();
            metroTileArticulos = new MetroFramework.Controls.MetroTile();

            metroPanelIconos.SuspendLayout();
            // 
            // metroPanelIconos
            // 
            metroPanelIconos.AutoSize = true;
            metroPanelIconos.Controls.Add(metroTileProveedores);
            metroPanelIconos.Controls.Add(metroTileClientes);
            metroPanelIconos.Controls.Add(metroTileArticulos);
            metroPanelIconos.HorizontalScrollbarBarColor = true;
            metroPanelIconos.HorizontalScrollbarHighlightOnWheel = false;
            metroPanelIconos.HorizontalScrollbarSize = 10;
            metroPanelIconos.Location = new System.Drawing.Point(81, 45);
            metroPanelIconos.Name = "metroPanelIconos";
            metroPanelIconos.Size = new System.Drawing.Size(453, 304);
            metroPanelIconos.TabIndex = 38;
            metroPanelIconos.VerticalScrollbarBarColor = true;
            metroPanelIconos.VerticalScrollbarHighlightOnWheel = false;
            metroPanelIconos.VerticalScrollbarSize = 10;
            // 
            // metroTileProveedores
            // 
            metroTileProveedores.ActiveControl = null;
            metroTileProveedores.Location = new System.Drawing.Point(307, 3);
            metroTileProveedores.Name = "metroTileProveedores";
            metroTileProveedores.Size = new System.Drawing.Size(146, 146);
            metroTileProveedores.Style = MetroFramework.MetroColorStyle.Teal;
            metroTileProveedores.TabIndex = 42;
            metroTileProveedores.Text = "&Proveedores";
            metroTileProveedores.TileImage = global::MetroFramework.Demo.Properties.Resources.compras;
            metroTileProveedores.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            metroTileProveedores.UseSelectable = true;
            metroTileProveedores.UseTileImage = true;
            metroTileProveedores.UseMnemonic = true;
            // 
            // metroTileClientes
            // 
            metroTileClientes.ActiveControl = null;
            metroTileClientes.Location = new System.Drawing.Point(155, 3);
            metroTileClientes.Name = "metroTileClientes";
            metroTileClientes.Size = new System.Drawing.Size(146, 146);
            metroTileClientes.Style = MetroFramework.MetroColorStyle.Lime;
            metroTileClientes.TabIndex = 41;
            metroTileClientes.Text = "&Clientes";
            metroTileClientes.TileImage = global::MetroFramework.Demo.Properties.Resources.clientes;
            metroTileClientes.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            metroTileClientes.UseCustomForeColor = true;
            metroTileClientes.UseSelectable = true;
            metroTileClientes.UseStyleColors = true;
            metroTileClientes.UseTileImage = true;
            // 
            // metroTileArticulos
            // 
            metroTileArticulos.ActiveControl = null;
            metroTileArticulos.BackColor = System.Drawing.Color.Yellow;
            metroTileArticulos.ForeColor = System.Drawing.Color.Red;
            metroTileArticulos.Location = new System.Drawing.Point(3, 3);
            metroTileArticulos.Name = "metroTileArticulos";
            metroTileArticulos.Size = new System.Drawing.Size(146, 146);
            metroTileArticulos.Style = MetroFramework.MetroColorStyle.Orange;
            metroTileArticulos.TabIndex = 40;
            metroTileArticulos.Text = "&Articulos";
            metroTileArticulos.TileImage = global::MetroFramework.Demo.Properties.Resources._2362209_64;
            metroTileArticulos.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            metroTileArticulos.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            metroTileArticulos.UseSelectable = true;
            metroTileArticulos.UseStyleColors = true;
            metroTileArticulos.UseTileImage = true;

            metroPanelIconos.ResumeLayout(false);

            metroPanel1.Controls.Add(metroPanelIconos);
            metroPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

            metroPanel1.Tag = "";

            CenterMetroPanelIconos();
        }

        private Controls.MetroTile CreateMetroTile(string text,string image, int xPosition, int yPosition, MetroFramework.MetroColorStyle style = MetroColorStyle.Default ,int width = 0, int height = 0)
        {
            Controls.MetroTile metroTile = new MetroFramework.Controls.MetroTile();

            if (height == 0)
                height = _defaultWidthMetroTile;

            if (height == 0)
                height = _defaultHeightMetroTile;

            
            metroTile.ActiveControl = null;
            metroTile.Location = new System.Drawing.Point(307, 3);
            metroTile.Size = new System.Drawing.Size(width, height);
            metroTile.Style = style;
            metroTile.TabIndex = 42;
            metroTile.Text = text;
            metroTile.TileImage = global::MetroFramework.Demo.Properties.Resources.compras;
            metroTile.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            metroTile.UseSelectable = true;
            metroTile.UseTileImage = true;

            return metroTile;

        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]

        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam); private void BtnSlide_Click(object sender, EventArgs e)
        {
            if (MenuVertical.Width == 250)
                MenuVertical.Width = 70;
            else
                MenuVertical.Width = 250;
        }
        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.BtnRestaturar.Visible = true;
            BtnMaximizar.Visible = false;
        }

        private void BtnRestaturar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.BtnRestaturar.Visible = false;
            BtnMaximizar.Visible = true;

        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MenuTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);

        }

        private void metroTile7_Click(object sender, EventArgs e)
        {

        }

        private void FrmMenu_Resize(object sender, EventArgs e)
        {
            if (metroPanel1.Tag != null)
            {
                CenterMetroPanelIconos();
            }
        }

        private void metroPanel1_Resize(object sender, EventArgs e)
        {
            if (metroPanel1.Tag  != null)
            {
                CenterMetroPanelIconos();
            }

        }
        void CenterMetroPanelIconos()
        {
            metroPanelIconos.Left = (metroPanel1.Width / 2) - (metroPanelIconos.Width / 2);
            metroPanelIconos.Top = (metroPanel1.Height / 2) - (metroPanelIconos.Height / 2);
        }
        private void BtnMaestros_Click(object sender, EventArgs e)
        {
            CrearIconosMaestros();
        }
    }


    public class OpcionMenu
    {
        public long idOpcionMenu { get; set; }
    }

}
