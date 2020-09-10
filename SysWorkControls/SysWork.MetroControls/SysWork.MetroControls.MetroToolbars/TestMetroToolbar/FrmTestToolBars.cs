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

namespace TestMetroToolbar
{
    public partial class FrmTestToolBars : Form
    {
        public FrmTestToolBars()
        {
            InitializeComponent();
        }

        private void metroToolbarReport1_ActionSelected(object sender, MetroToolbarReportClickEventArgs e)
        {
            MessageBox.Show(e.Action.ToString());

            switch (e.Action)
            {
                case MetroToolbarReportAction.Display:
                    break;
                case MetroToolbarReportAction.Print:
                    break;
                case MetroToolbarReportAction.Refresh:
                    break;
                case MetroToolbarReportAction.Search:
                    break;
                case MetroToolbarReportAction.ImportExport:
                    break;
                case MetroToolbarReportAction.CleanFilters:
                    break;
                case MetroToolbarReportAction.Exit:
                    break;
                default:
                    break;
            }
        }

        private void metroToolbarCRUD1_ActionSelected(object sender, MetroToolbarCRUDlickEventArgs e)
        {
            MessageBox.Show(e.Action.ToString());

            switch (e.Action)
            {
                case MetroToolbarCRUDAction.New:
                    break;
                case MetroToolbarCRUDAction.Delete:
                    break;
                case MetroToolbarCRUDAction.Refresh:
                    break;
                case MetroToolbarCRUDAction.Search:
                    break;
                case MetroToolbarCRUDAction.ImportExport:
                    break;
                case MetroToolbarCRUDAction.Report:
                    break;
                case MetroToolbarCRUDAction.Initialize:
                    break;
                case MetroToolbarCRUDAction.Save:
                    break;
                case MetroToolbarCRUDAction.Exit:
                    break;
                default:
                    break;
            }
        }

        private void metroToolbarDisplaySettings1_ActionSelected(object sender, MetroToolbarDisplaySettingsClickEventArgs e)
        {
            MessageBox.Show(e.Action.ToString());
            switch (e.Action)
            {
                case MetroToolbarDisplaySettingsAction.ChangeTheme:
                    metroToolbarDisplaySettings1.Theme = metroToolbarDisplaySettings1.Theme == MetroTheme.Dark ? MetroTheme.Light : MetroTheme.Dark;
                    metroToolbarCRUD1.Theme = metroToolbarDisplaySettings1.Theme;
                    metroToolbarReport1.Theme = metroToolbarDisplaySettings1.Theme;

                    if (metroToolbarReport1.Theme == MetroTheme.Dark)
                        this.BackColor = Color.FromArgb(17, 17, 17);
                    else
                        this.BackColor = Color.White;


                    break;
                case MetroToolbarDisplaySettingsAction.ChangeStyle:
                    break;
                default:
                    break;
            }
        }
    }
}
