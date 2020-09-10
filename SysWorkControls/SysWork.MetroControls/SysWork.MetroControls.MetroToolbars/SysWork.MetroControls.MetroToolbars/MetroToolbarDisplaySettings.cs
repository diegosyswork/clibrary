using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SysWork.MetroControls.MetroToolbars.Properties;

namespace SysWork.MetroControls.MetroToolbars
{
    /// <summary>
    /// Actions for MetroToolbarDisplaySettings
    /// </summary>
    public enum MetroToolbarDisplaySettingsAction
    {
        ChangeTheme,
        ChangeStyle,
    }

    [DefaultEvent("ActionSelected")]
    public partial class MetroToolbarDisplaySettings: UserControl
    {
        /// <summary>
        /// Occurs when an action is selected.
        /// </summary>
        public event MetroToolbarDisplaySettingsClickEventHandler ActionSelected;
        public delegate void MetroToolbarDisplaySettingsClickEventHandler(Object sender, MetroToolbarDisplaySettingsClickEventArgs e);

        public bool ChangeThemeEnabled
        {
            get{return ButtonChangeTheme.Enabled;}
            set{ButtonChangeTheme.Enabled = value;}
        }

        public bool ChangeStyleEnabled
        {
            get{return ButtonChangeStyle.Enabled;}
            set{ButtonChangeStyle.Enabled = value;}
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

        public MetroToolbarDisplaySettings()
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

            this.ToolbarDisplatSettings.BackColor = _backColor;
            this.BackColor = _backColor;

            this.ButtonChangeTheme.BackColor = _backColor;
            this.ButtonChangeTheme.Image = (_theme  == MetroTheme.Dark) ? Resources.changeThemeDark : Resources.changeThemeLight;

            this.ButtonChangeStyle.BackColor = _backColor;
            this.ButtonChangeStyle.Image = (_theme == MetroTheme.Dark) ? Resources.changeStyleDark : Resources.changeStyleLight;

        }


        protected virtual void OnMetroToolbarDisplaySettingsActionSelected(MetroToolbarDisplaySettingsClickEventArgs e)
        {
            MetroToolbarDisplaySettingsClickEventHandler handler = ActionSelected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ThrowChangeThemeEvent()
        {
            var args = new MetroToolbarDisplaySettingsClickEventArgs();
            args.Action = MetroToolbarDisplaySettingsAction.ChangeTheme;
            OnMetroToolbarDisplaySettingsActionSelected(args);
        }
        private void ThrowChangeStyleEvent()
        {
            var args = new MetroToolbarDisplaySettingsClickEventArgs();
            args.Action = MetroToolbarDisplaySettingsAction.ChangeStyle;
            OnMetroToolbarDisplaySettingsActionSelected(args);
        }


        private void ButtonChangeTheme_Click(object sender, EventArgs e)
        {
            ThrowChangeThemeEvent();
        }

        private void ButtonChangeStyle_Click(object sender, EventArgs e)
        {
            ThrowChangeStyleEvent();
        }

        private void MetroToolbarDisplaySettings_Resize(object sender, EventArgs e)
        {
            if (this.Height > 29)
                this.Height = 29;
        }
    }

    public class MetroToolbarDisplaySettingsClickEventArgs : EventArgs
    {
        public MetroToolbarDisplaySettingsAction Action;
    }
}
