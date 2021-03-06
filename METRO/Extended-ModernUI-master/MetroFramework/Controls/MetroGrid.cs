/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2016 Angelo Cresta, http://quarztech.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
namespace MetroFramework.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using MetroFramework.Components;
    using MetroFramework.Drawing;
    using MetroFramework.Design;
    using MetroFramework.Interfaces;

    [Designer(typeof(MetroGridDesigner))]
    [ToolboxBitmap(typeof(DataGridView))]
    public partial class MetroGrid : DataGridView, IMetroControl
    {
        #region ... Interface ...
        [Category("Metro Appearance")]
        public event EventHandler<MetroPaintEventArgs> CustomPaintBackground;
        protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null)
            {
                CustomPaintBackground(this, e);
            }
        }

        [Category("Metro Appearance")]
        public event EventHandler<MetroPaintEventArgs> CustomPaint;
        protected virtual void OnCustomPaint(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null)
            {
                CustomPaint(this, e);
            }
        }

        [Category("Metro Appearance")]
        public event EventHandler<MetroPaintEventArgs> CustomPaintForeground;
        protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null)
            {
                CustomPaintForeground(this, e);
            }
        }

        private MetroColorStyle metroStyle = MetroColorStyle.Default;
        [Category("Metro Appearance")]
        [DefaultValue(MetroColorStyle.Default)]
        public MetroColorStyle Style
        {
            get
            {
                if (DesignMode || metroStyle != MetroColorStyle.Default)
                {
                    return metroStyle;
                }

                if (StyleManager != null && metroStyle == MetroColorStyle.Default)
                {
                    return StyleManager.Style;
                }
                if (StyleManager == null && metroStyle == MetroColorStyle.Default)
                {
                    return MetroColorStyle.Blue;
                }

                return metroStyle;
            }
            set { metroStyle = value; StyleGrid(); }
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;
        [Category("Metro Appearance")]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != MetroThemeStyle.Default)
                {
                    return metroTheme;
                }

                if (StyleManager != null && metroTheme == MetroThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }
                if (StyleManager == null && metroTheme == MetroThemeStyle.Default)
                {
                    return MetroThemeStyle.Light;
                }

                return metroTheme;
            }
            set { metroTheme = value; StyleGrid(); }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; StyleGrid(); }
        }
        #endregion

        #region ... Properties ...
        private MetroDataGridSize metroDataGridSize = MetroDataGridSize.Medium;
        [Browsable(false)]
        public MetroDataGridSize FontSize
        {
            get { return metroDataGridSize; }
            set { metroDataGridSize = value; Refresh(); }
        }

        private MetroDataGridWeight metroDataGridWeight = MetroDataGridWeight.Regular;
        [Category("Metro Appearance")]
        [Browsable(false)]
        public MetroDataGridWeight FontWeight
        {
            get { return metroDataGridWeight; }
            set { metroDataGridWeight = value; Refresh(); }
        }

        private bool useCustomBackColor = false;
        [DefaultValue(false)]
        [Category("Metro Appearance")]
        public bool UseCustomBackColor
        {
            get { return useCustomBackColor; }
            set { useCustomBackColor = value; }
        }

        private bool useCustomForeColor = false;
        [DefaultValue(false)]
        [Category("Metro Appearance")]
        public bool UseCustomForeColor
        {
            get { return useCustomForeColor; }
            set { useCustomForeColor = value; }
        }

        private bool useStyleColors = false;
        [DefaultValue(false)]
        [Category("Metro Appearance")]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; }
        }

        [Browsable(false)]
        [Category("Metro Behaviour")]
        [DefaultValue(true)]
        public bool UseSelectable
        {
            get { return GetStyle(ControlStyles.Selectable); }
            set { SetStyle(ControlStyles.Selectable, value); }
        }
        #endregion

        #region ... Constructor ...
        MetroDataGridHelper scrollhelper = null;
        MetroDataGridHelper scrollhelperH = null;

        public MetroGrid()
        {
            InitializeComponent();

            StyleGrid();

            this.Controls.Add(_vertical);
            this.Controls.Add(_horizontal);

            this.Controls.SetChildIndex(_vertical, 0);
            this.Controls.SetChildIndex(_horizontal, 1);

            _horizontal.Visible = false;
            _vertical.Visible = false;

            scrollhelper = new MetroDataGridHelper(_vertical, this);
            scrollhelperH = new MetroDataGridHelper(_horizontal, this, false);
        }
        #endregion

        #region ... Overrides ...
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (this.RowCount > 1)
            {
                if (e.Delta > 0 && this.FirstDisplayedScrollingRowIndex > 0)
                    this.FirstDisplayedScrollingRowIndex--;
                else if (e.Delta < 0)
                    this.FirstDisplayedScrollingRowIndex++;
            }
        }
        #endregion

        private void StyleGrid()
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.EnableHeadersVisualStyles = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.BackColor = MetroPaint.BackColor.DataGrid.Normal(Theme); //MetroPaint.BackColor.Form(Theme);
            this.BackgroundColor = MetroPaint.BackColor.DataGrid.Background(Theme); //MMetroPaint.BackColor.Form(Theme);
            this.GridColor = MetroPaint.BorderColor.DataGrid.GridColor(Theme); // MetroPaint.BackColor.Form(Theme);
            this.ForeColor = MetroPaint.ForeColor.DataGrid.Normal(Theme);

            this.Font = MetroFonts.DataGrid(metroDataGridSize, metroDataGridWeight); //new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.Pixel);

            this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.AllowUserToResizeRows = false;

            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.ColumnHeadersDefaultCellStyle.BackColor = MetroPaint.BackColor.DataGrid.ColumnHeadersDefaultCellStyle(Theme, Style); // MetroPaint.GetStyleColor(Style);
            this.ColumnHeadersDefaultCellStyle.ForeColor = MetroPaint.ForeColor.DataGrid.ColumnHeadersDefaultCellStyle(Theme, Style);
            this.ColumnHeadersDefaultCellStyle.SelectionBackColor = MetroPaint.BackColor.DataGrid.ColumnHeadersDefaultCellStyleSelectionBackColor(Theme, Style); //MetroPaint.GetStyleColor(Style);
            this.ColumnHeadersDefaultCellStyle.SelectionForeColor = MetroPaint.ForeColor.DataGrid.ColumnHeadersDefaultCellStyleSelectionForeColor(Theme, Style); //(Theme == MetroThemeStyle.Light) ? Color.FromArgb(17, 17, 17) : Color.FromArgb(255, 255, 255);
            this.ColumnHeadersDefaultCellStyle.Font = MetroFonts.DataGrid(MetroDataGridSize.Medium, MetroDataGridWeight.Bold);

            this.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.RowHeadersDefaultCellStyle.BackColor = MetroPaint.BackColor.DataGrid.RowHeadersDefaultCellStyle(Theme, Style); //MetroPaint.GetStyleColor(Style);
            this.RowHeadersDefaultCellStyle.ForeColor = MetroPaint.ForeColor.DataGrid.RowHeadersDefaultCellStyle(Theme, Style);
            this.RowHeadersDefaultCellStyle.SelectionBackColor = MetroPaint.BackColor.DataGrid.RowHeadersDefaultCellStyleSelectionBackColor(Theme, Style); //MetroPaint.BackColor.DataGrid.RowHeadersDefaultCellStyleSelectionBackColor(Theme, Style); //MetroPaint.GetStyleColor(Style);
            this.RowHeadersDefaultCellStyle.SelectionForeColor = MetroPaint.ForeColor.DataGrid.RowHeadersDefaultCellStyleSelectionForeColor(Theme, Style); //(Theme == MetroThemeStyle.Light) ? Color.FromArgb(17, 17, 17) : Color.FromArgb(255, 255, 255);
            this.RowHeadersDefaultCellStyle.Font = MetroFonts.DataGrid(MetroDataGridSize.Medium, MetroDataGridWeight.Bold);

            this.DefaultCellStyle.BackColor = MetroPaint.BackColor.DataGrid.DefaultCellStyle(Theme); //MetroPaint.BackColor.Form(Theme);
            this.DefaultCellStyle.SelectionBackColor = MetroPaint.BackColor.DataGrid.DefaultCellStyleSelectionBackColor(Theme, Style); //MetroPaint.GetStyleColor(Style);
            this.DefaultCellStyle.SelectionForeColor = MetroPaint.ForeColor.DataGrid.DefaultCellStyleSelectionForeColor(Theme, Style); //(Theme == MetroThemeStyle.Light) ? Color.FromArgb(17, 17, 17) : Color.FromArgb(255, 255, 255);
            this.DefaultCellStyle.SelectionBackColor = MetroPaint.BackColor.DataGrid.DefaultCellStyleSelectionBackColor(Theme, Style); //MetroPaint.GetStyleColor(Style);
            this.DefaultCellStyle.SelectionForeColor = MetroPaint.ForeColor.DataGrid.DefaultCellStyleSelectionForeColor(Theme, Style); //(Theme == MetroThemeStyle.Light) ? Color.FromArgb(17, 17, 17) : Color.FromArgb(255, 255, 255);
            this.DefaultCellStyle.Font = MetroFonts.DataGrid(MetroDataGridSize.Medium, MetroDataGridWeight.Regular);

        }
    }

    #region ... dataGrid Helper ...
    public class MetroDataGridHelper
    {
        /// <summary>
        /// The associated scrollbar or scrollbar collector
        /// </summary>
        private MetroScrollBar _scrollbar;

        /// <summary>
        /// Associated Grid
        /// </summary>
        private DataGridView _grid;

        /// <summary>
        /// if greater zero, scrollbar changes are ignored
        /// </summary>
        private int _ignoreScrollbarChange = 0;

        /// <summary>
        /// 
        /// </summary>
        private bool _ishorizontal = false;
        private HScrollBar hScrollbar = null;
        private VScrollBar vScrollbar = null;

        public MetroDataGridHelper(MetroScrollBar scrollbar, DataGridView grid, bool vertical = true)
        {
            _scrollbar = scrollbar;
            _scrollbar.UseBarColor = true;
            _grid = grid;
            _ishorizontal = !vertical;

            foreach (var item in _grid.Controls)
            {
                if (item.GetType() == typeof(VScrollBar))
                {
                    vScrollbar = (VScrollBar)item;
                }

                if (item.GetType() == typeof(HScrollBar))
                {
                    hScrollbar = (HScrollBar)item;
                }
            }

            _grid.RowsAdded += new DataGridViewRowsAddedEventHandler(_grid_RowsAdded);
            _grid.UserDeletedRow += new DataGridViewRowEventHandler(_grid_UserDeletedRow);
            _grid.Scroll += new ScrollEventHandler(_grid_Scroll);
            _grid.Resize += new EventHandler(_grid_Resize);
            _scrollbar.Scroll += _scrollbar_Scroll;
            _scrollbar.ScrollbarSize = 17;

            UpdateScrollbar();
        }

        void _grid_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateScrollbar();
        }

        void _grid_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            UpdateScrollbar();
        }

        void _grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateScrollbar();
        }

        void _scrollbar_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            if (_ignoreScrollbarChange > 0) return;

            if (_ishorizontal)
            {
                hScrollbar.Value = _scrollbar.Value;

                try
                {
                    _grid.HorizontalScrollingOffset = _scrollbar.Value;
                }
                catch { }
            }
            else
            {
                if (_scrollbar.Value >= 0 && _scrollbar.Value < _grid.Rows.Count)
                    _grid.FirstDisplayedScrollingRowIndex = _scrollbar.Value + (_scrollbar.Value == 1 ? -1 : 1);
            }

            _grid.Invalidate();
        }

        private void BeginIgnoreScrollbarChangeEvents()
        {
            _ignoreScrollbarChange++;
        }

        private void EndIgnoreScrollbarChangeEvents()
        {
            if (_ignoreScrollbarChange > 0)
                _ignoreScrollbarChange--;
        }

        /// <summary>
        /// Updates the scrollbar values
        /// </summary>
        public void UpdateScrollbar()
        {
            try
            {
                BeginIgnoreScrollbarChangeEvents();

                if (_ishorizontal)
                {
                    int visibleCols = VisibleFlexGridCols();
                    _scrollbar.Maximum = hScrollbar.Maximum;
                    _scrollbar.Minimum = hScrollbar.Minimum;
                    _scrollbar.SmallChange = hScrollbar.SmallChange;
                    _scrollbar.LargeChange = hScrollbar.LargeChange;
                    _scrollbar.Location = new Point(0, _grid.Height - _scrollbar.ScrollbarSize);
                    _scrollbar.Width = _grid.Width - (vScrollbar.Visible ? _scrollbar.ScrollbarSize : 0);
                    _scrollbar.BringToFront();
                    _scrollbar.Visible = hScrollbar.Visible;
                    _scrollbar.Value = hScrollbar.Value == 0 ? 1 : hScrollbar.Value;
                }
                else
                {
                    int visibleRows = VisibleFlexGridRows();
                    _scrollbar.Maximum = _grid.RowCount;
                    _scrollbar.Minimum = 1;
                    _scrollbar.SmallChange = 1;
                    _scrollbar.LargeChange = Math.Max(1, visibleRows - 1);
                    _scrollbar.Value = _grid.FirstDisplayedScrollingRowIndex;

                    _scrollbar.Location = new Point(_grid.Width - _scrollbar.ScrollbarSize, 0);
                    _scrollbar.Height = _grid.Height - (hScrollbar.Visible ? _scrollbar.ScrollbarSize : 0);
                    _scrollbar.BringToFront();
                    _scrollbar.Visible = vScrollbar.Visible;
                }
            }
            finally
            {
                EndIgnoreScrollbarChangeEvents();
            }
        }

        /// <summary>
        /// Determine the current count of visible rows
        /// </summary>
        /// <returns></returns>
        private int VisibleFlexGridRows()
        {
            return _grid.DisplayedRowCount(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int VisibleFlexGridCols()
        {
            return _grid.DisplayedColumnCount(true);
        }

        public bool VisibleVerticalScroll()
        {
            bool _return = false;

            if (_grid.DisplayedRowCount(true) < _grid.RowCount + (_grid.RowHeadersVisible ? 1 : 0))
            {
                _return = true;
            }

            return _return;
        }

        public bool VisibleHorizontalScroll()
        {
            bool _return = false;

            if (_grid.DisplayedColumnCount(true) < _grid.ColumnCount + (_grid.ColumnHeadersVisible ? 1 : 0))
            {
                _return = true;
            }

            return _return;
        }

        #region Events of interest

        void _grid_Resize(object sender, EventArgs e)
        {
            UpdateScrollbar();
        }

        void _grid_AfterDataRefresh(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            UpdateScrollbar();
        }
        #endregion

    }
    #endregion
}
