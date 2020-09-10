using MetroFramework.Controls;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestMetroFramework
{
    public partial class FrmTestPrincipal : MetroForm
    {
        public FrmTestPrincipal()
        {
            InitializeComponent();
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            Console.Write("");
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            Console.Write("");
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox2_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox2_ButtonClick(object sender, EventArgs e)
        {

        }

        private void TileClick(object sender, EventArgs e)
        {
            var b = (MetroTile)sender;
            MetroLabelModulo.Text = b.Text.ToUpper();
        }
    }
}
