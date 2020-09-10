using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SysWork.MetroControls.MetroToolbars.Properties;

namespace SysWork.MetroControls.MetroToolbars
{
    /// <summary>
    /// Action from MetroToolbarCRUD
    /// </summary>
    public enum MetroToolbarReportAction
    {
        Display,
        Print,
        Refresh,
        Search,
        ImportExport,
        CleanFilters,
        Exit
    }

    [DefaultEvent("ActionSelected")]
    public partial class MetroToolbarReport: UserControl
    {
        /// <summary>
        /// Occurs when an action is selected.
        /// </summary>
        public event MetroToolbarReportClickEventHandler ActionSelected;
        public delegate void MetroToolbarReportClickEventHandler(Object sender, MetroToolbarReportClickEventArgs e);

        public bool DisplayEnabled
        {
            get{return ButtonDisplay.Enabled;}
            set{ButtonDisplay.Enabled = value;}
        }

        public bool PrintEnabled
        {
            get{return ButtonPrint.Enabled;}
            set{ButtonPrint.Enabled = value;}
        }

        public bool RefreshEnabled
        {
            get{return ButtonRefresh.Enabled;}
            set{ButtonRefresh.Enabled = value;}
        }

        public bool SearchEnabled
        {
            get{return ButtonSearch.Enabled;}
            set{ButtonSearch.Enabled = value;}
        }

        public bool ImportExportEnabled
        {
            get{return ButtonImportExport.Enabled;}
            set{ButtonImportExport.Enabled = value;}
        }
        public bool ClenFiltersEnabled
        {
            get{return ButtonCleanFilters.Enabled;}
            set{ButtonCleanFilters.Enabled = value;}
        }

        public bool ExitEnabled
        {
            get{return ButtonExit.Enabled;}
            set{ButtonExit.Enabled = value;}
        }


        private MetroTheme _theme = MetroTheme.Light;
        public MetroTheme Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;
                ApplyTheme();
            }
        }

        public MetroToolbarReport()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            var _backColor = Color.White;
            switch (_theme)
            {
                case MetroTheme.Default:
                case MetroTheme.Light:
                    _backColor = Color.White;
                    break;
                case MetroTheme.Dark:
                    _backColor = Color.FromArgb(17, 17, 17);
                    break;
            }

            this.ToolbarReport.BackColor = _backColor;
            this.BackColor = _backColor;
            this.ButtonDisplay.BackColor = _backColor;
            this.ButtonDisplay.Image = (_theme  == MetroTheme.Dark) ? Resources.monitorDark : Resources.monitorLight;

            this.ButtonPrint.BackColor = _backColor;
            this.ButtonPrint.Image = (_theme == MetroTheme.Dark) ? Resources.printDark : Resources.printLight;

            this.ButtonRefresh.BackColor = _backColor;
            this.ButtonRefresh.Image = (_theme == MetroTheme.Dark) ? Resources.refreshDark : Resources.refreshLight;

            this.ButtonSearch.BackColor = _backColor;
            this.ButtonSearch.Image = (_theme == MetroTheme.Dark) ? Resources.searchDark : Resources.searchLight;

            this.ButtonImportExport.BackColor = _backColor;
            this.ButtonImportExport.Image = (_theme == MetroTheme.Dark) ? Resources.importExportDark : Resources.importExportLight;

            this.ButtonCleanFilters.BackColor = _backColor;
            this.ButtonCleanFilters.Image = (_theme == MetroTheme.Dark) ? Resources.clearFiltersDark : Resources.clearFiltersLight;

            this.ButtonExit.BackColor = _backColor;
            this.ButtonExit.Image = (_theme == MetroTheme.Dark) ? Resources.closeDark : Resources.closeLight;
        }

        public void AnalizeKey(Keys keyCode)
        {

            switch (keyCode)
            {
                case Keys.F2:
                    if (ButtonDisplay.Enabled)
                        ThrowDisplayEvent();
                    break;
                case Keys.F3:
                    if (ButtonPrint.Enabled)
                        ThrowPrintEvent();
                    break;
                case Keys.F4:
                    if (ButtonCleanFilters.Enabled)
                        ThrowCleanFilterEvent();
                    break;
                case Keys.F12:
                    if (ButtonRefresh.Enabled)
                        ThrowRefreshEvent();
                    break;
                case Keys.F5:
                    if (ButtonSearch.Enabled)
                        ThrowSearchEvent();
                    break;
                case Keys.Escape:
                    if (ButtonExit.Enabled)
                        ThrowExitEvent();

                    break;
            }
        }

        protected virtual void OnMetroToolbarReportActionSelected(MetroToolbarReportClickEventArgs e)
        {
            MetroToolbarReportClickEventHandler handler = ActionSelected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ThrowDisplayEvent()
        {
            var args = new MetroToolbarReportClickEventArgs();
            args.Action = MetroToolbarReportAction.Display;
            OnMetroToolbarReportActionSelected(args);
        }
        private void ThrowPrintEvent()
        {
            var args = new MetroToolbarReportClickEventArgs();
            args.Action = MetroToolbarReportAction.Print;
            OnMetroToolbarReportActionSelected(args);
        }

        private void ThrowRefreshEvent()
        {
            var args = new MetroToolbarReportClickEventArgs();
            args.Action = MetroToolbarReportAction.Refresh;
            OnMetroToolbarReportActionSelected(args);
        }

        private void ThrowSearchEvent()
        {
            var args = new MetroToolbarReportClickEventArgs();
            args.Action = MetroToolbarReportAction.Search;
            OnMetroToolbarReportActionSelected(args);
        }

        private void ThrowImporExportEvent()
        {
            var args = new MetroToolbarReportClickEventArgs();
            args.Action = MetroToolbarReportAction.ImportExport;
            OnMetroToolbarReportActionSelected(args);
        }

        private void ThrowCleanFilterEvent()
        {
            var args = new MetroToolbarReportClickEventArgs();
            args.Action = MetroToolbarReportAction.CleanFilters;
            OnMetroToolbarReportActionSelected(args);
        }

        private void ThrowExitEvent()
        {
            var args = new MetroToolbarReportClickEventArgs();
            args.Action = MetroToolbarReportAction.Exit;
            OnMetroToolbarReportActionSelected(args);
        }

        private void ButtonDisplay_Click(object sender, EventArgs e)
        {
            ThrowDisplayEvent();
        }

        private void ButtonPrint_Click(object sender, EventArgs e)
        {
            ThrowPrintEvent();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            ThrowRefreshEvent();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            ThrowSearchEvent();
        }

        private void ButtonImportExport_Click(object sender, EventArgs e)
        {
            ThrowImporExportEvent();
        }

        private void ButtonCleanFilters_Click(object sender, EventArgs e)
        {
            ThrowCleanFilterEvent();
        }
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            ThrowExitEvent();
        }

        private void MetroToolbarReport_Resize(object sender, EventArgs e)
        {
            if (this.Height > 29)
                this.Height = 29;
        }
    }

    public class MetroToolbarReportClickEventArgs : EventArgs
    {
        public MetroToolbarReportAction Action;
    }
}
