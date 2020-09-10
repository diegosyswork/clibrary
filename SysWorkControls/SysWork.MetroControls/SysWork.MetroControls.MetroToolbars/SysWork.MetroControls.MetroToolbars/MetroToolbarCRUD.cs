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
    public enum MetroToolbarCRUDAction
    {
        New,
        Delete,
        Refresh,
        Search,
        ImportExport,
        Report,
        Initialize,
        Save,
        Exit
    }

    [DefaultEvent("ActionSelected")]
    public partial class MetroToolbarCRUD: UserControl
    {
        /// <summary>
        /// Occurs when an action is selected.
        /// </summary>
        public event MetroToolbarCRUDClickEventHandler ActionSelected;
        public delegate void MetroToolbarCRUDClickEventHandler(Object sender, MetroToolbarCRUDlickEventArgs e);

        public bool NewEnabled
        {
            get{return ButtonNew.Enabled;}
            set{ButtonNew.Enabled = value;}
        }

        public bool DeleteEnabled
        {
            get{return ButtonDelete.Enabled;}
            set{ButtonDelete.Enabled = value;}
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
        public bool ReportEnabled
        {
            get{return ButtonReport.Enabled;}
            set{ButtonReport.Enabled = value;}
        }

        public bool SaveEnabled
        {
            get{return ButtonSave.Enabled;}
            set{ButtonSave.Enabled = value;}
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

        public MetroToolbarCRUD()
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
            
            this.ToolbarCRUD.BackColor = _backColor;
            this.BackColor = _backColor;
            this.ButtonNew.BackColor = _backColor;
            this.ButtonNew.Image = (_theme  == MetroTheme.Dark) ? Resources.newDark : Resources.newLight;

            this.ButtonDelete.BackColor = _backColor;
            this.ButtonDelete.Image = (_theme == MetroTheme.Dark) ? Resources.deleteDark : Resources.deleteLight;

            this.ButtonRefresh.BackColor = _backColor;
            this.ButtonRefresh.Image = (_theme == MetroTheme.Dark) ? Resources.refreshDark : Resources.refreshLight;

            this.ButtonSearch.BackColor = _backColor;
            this.ButtonSearch.Image = (_theme == MetroTheme.Dark) ? Resources.searchDark : Resources.searchLight;

            this.ButtonImportExport.BackColor = _backColor;
            this.ButtonImportExport.Image = (_theme == MetroTheme.Dark) ? Resources.importExportDark : Resources.importExportLight;

            this.ButtonReport.BackColor = _backColor;
            this.ButtonReport.Image = (_theme == MetroTheme.Dark) ? Resources.reportDark : Resources.reportLight;

            this.ButtonInitialize.BackColor = _backColor;
            this.ButtonInitialize.Image = (_theme == MetroTheme.Dark) ? Resources.initializeDark : Resources.initializeLight;

            this.ButtonSave.BackColor = _backColor;
            this.ButtonSave.Image = (_theme == MetroTheme.Dark) ? Resources.saveDark : Resources.saveLight;

            this.ButtonExit.BackColor = _backColor;
            this.ButtonExit.Image = (_theme == MetroTheme.Dark) ? Resources.closeDark : Resources.closeLight;
        }

        public void AnalizeKey(Keys keyCode)
        {

            switch (keyCode)
            {
                case Keys.F3:
                    if (ButtonNew.Enabled)
                        ThrowNewEvent();
                    break;
                case Keys.F4:
                    if (ButtonReport.Enabled)
                        ThrowReportEvent();
                    break;
                case Keys.F10:
                    if (ButtonDelete.Enabled)
                        ThrowDeleteEvent();
                    break;
                case Keys.F12:
                    if (ButtonRefresh.Enabled)
                        ThrowRefreshEvent();
                    break;
                case Keys.F5:
                    if (ButtonSearch.Enabled)
                        ThrowSearchEvent();
                    break;

                case Keys.F2:
                    if (ButtonSave.Enabled)
                        ThrowSaveEvent();
                    break;

                case Keys.Escape:
                    if (ButtonInitialize.Enabled)
                        ThrowInitializeEvent();
                    else if (ButtonExit.Enabled)
                        ThrowExitEvent();

                    break;
            }
        }

        protected virtual void OnMetroToolbarCRUDActionSelected(MetroToolbarCRUDlickEventArgs e)
        {
            MetroToolbarCRUDClickEventHandler handler = ActionSelected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ThrowNewEvent()
        {
            var args = new MetroToolbarCRUDlickEventArgs();
            args.Action = MetroToolbarCRUDAction.New;
            OnMetroToolbarCRUDActionSelected(args);
        }
        private void ThrowDeleteEvent()
        {
            var args = new MetroToolbarCRUDlickEventArgs();
            args.Action = MetroToolbarCRUDAction.Delete;

            OnMetroToolbarCRUDActionSelected(args);
        }

        private void ThrowRefreshEvent()
        {
            var args = new MetroToolbarCRUDlickEventArgs();
            args.Action = MetroToolbarCRUDAction.Refresh;
            OnMetroToolbarCRUDActionSelected(args);
        }

        private void ThrowSearchEvent()
        {
            var args = new MetroToolbarCRUDlickEventArgs();
            args.Action = MetroToolbarCRUDAction.Search;
            OnMetroToolbarCRUDActionSelected(args);
        }

        private void ThrowImporExportEvent()
        {
            var args = new MetroToolbarCRUDlickEventArgs();
            args.Action = MetroToolbarCRUDAction.ImportExport;
            OnMetroToolbarCRUDActionSelected(args);
        }

        private void ThrowReportEvent()
        {
            var args = new MetroToolbarCRUDlickEventArgs();
            args.Action = MetroToolbarCRUDAction.Report;
            OnMetroToolbarCRUDActionSelected(args);
        }

        private void ThrowInitializeEvent()
        {
            var args = new MetroToolbarCRUDlickEventArgs();
            args.Action = MetroToolbarCRUDAction.Initialize;
            OnMetroToolbarCRUDActionSelected(args);
        }

        private void ThrowSaveEvent()
        {
            var args = new MetroToolbarCRUDlickEventArgs();
            args.Action = MetroToolbarCRUDAction.Save;
            OnMetroToolbarCRUDActionSelected(args);
        }

        private void ThrowExitEvent()
        {
            var args = new MetroToolbarCRUDlickEventArgs();
            args.Action = MetroToolbarCRUDAction.Exit;
            OnMetroToolbarCRUDActionSelected(args);
        }

        private void ButtonNew_Click(object sender, EventArgs e)
        {
            ThrowNewEvent();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            ThrowDeleteEvent();
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

        private void ButtonReport_Click(object sender, EventArgs e)
        {
            ThrowReportEvent();
        }

        private void ButtonInitialize_Click(object sender, EventArgs e)
        {
            ThrowInitializeEvent();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            ThrowSaveEvent();
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            ThrowExitEvent();
        }

        private void MetroToolbarCRUD_Resize(object sender, EventArgs e)
        {
            if (this.Height > 29)
                this.Height = 29;
        }
    }

    public class MetroToolbarCRUDlickEventArgs : EventArgs
    {
        public MetroToolbarCRUDAction Action;
    }
}
