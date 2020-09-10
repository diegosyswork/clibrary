using MetroFramework;
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
    public partial class FrmTestReport : MetroForm
    {
        MetroColorStyle _metroColorStyle;
        MetroThemeStyle _metroThemeStyle;

        public FrmTestReport()
        {
            InitializeComponent();
        }

        public void SetAppearance(MetroThemeStyle MetroThemeStyle, MetroColorStyle MetroColorStyle)
        {
            _metroThemeStyle = MetroThemeStyle;
            _metroColorStyle = MetroColorStyle;

            metroStyleManager1.Theme = _metroThemeStyle;
            metroStyleManager1.Style = _metroColorStyle;

            StyleManager = this.metroStyleManager1;
        }

        private void metroToolbarDisplaySettings1_ActionSelected(object sender, MetroToolbarDisplaySettingsClickEventArgs e)
        {
            switch (e.Action)
            {
                case MetroToolbarDisplaySettingsAction.ChangeTheme:
                    metroStyleManager1.Theme = metroStyleManager1.Theme== MetroThemeStyle.Dark ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
                    this.Refresh();
                    metroToolbarReport1.Theme = metroStyleManager1.Theme == MetroThemeStyle.Dark ?  MetroTheme.Dark : MetroTheme.Light;
                    metroToolbarDisplaySettings1.Theme = metroToolbarReport1.Theme;

                    break;
                case MetroToolbarDisplaySettingsAction.ChangeStyle:
                    metroStyleManager1.Theme = MetroThemeStyle.Light;
                    metroStyleManager1.Style = MetroColorStyle.Purple ;
                    break;
                default:
                    break;
            }
        }
    }
}
