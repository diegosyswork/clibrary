using MetroFramework;
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
using SysWork.MetroControls.MetroToolbars;

namespace TestMetroFramework
{
    public partial class FrmTestPrincipal : MetroForm
    {
        public FrmTestPrincipal()
        {
            InitializeComponent();
            StyleManager = this.metroStyleManager1;
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

        private void metroToolbarDisplaySettings1_ActionSelected(object sender, SysWork.MetroControls.MetroToolbars.MetroToolbarDisplaySettingsClickEventArgs e)
        {
            switch (e.Action)
            {
                case MetroToolbarDisplaySettingsAction.ChangeTheme:
                    metroStyleManager1.Theme = metroStyleManager1.Theme == MetroThemeStyle.Dark ? MetroFramework.MetroThemeStyle.Light : MetroFramework.MetroThemeStyle.Dark;
                    metroToolbarDisplaySettings1.Theme = metroStyleManager1.Theme == MetroThemeStyle.Dark ? MetroTheme.Dark: MetroTheme.Light;
                    break;
                case MetroToolbarDisplaySettingsAction.ChangeStyle:
                    var m = new Random();
                    int next = m.Next(0, 13);
                    var style = (MetroColorStyle)next;
                    metroStyleManager1.Style = style;
                    break;
                default:
                    break;
            }

            this.metroRendererManager1.Theme = metroStyleManager1.Theme;
            this.metroRendererManager1.Style= metroStyleManager1.Style;


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
