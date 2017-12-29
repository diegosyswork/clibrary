using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Controls
{
    public class DataGridViewDecimalColumn : DataGridViewColumn
    {
        public DataGridViewDecimalColumn(): base(new DecimalCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DecimalCell)))
                {
                    throw new InvalidCastException("Must be a DecimalCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class DecimalCell : DataGridViewTextBoxCell
    {

        public DecimalCell(): base()
        {
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,dataGridViewCellStyle);

            DecimalEditingControl ctl = DataGridView.EditingControl as DecimalEditingControl;

            try
            {
                if (this.Value == null)
                {
                    ctl.Text = Convert.ToString(this.DefaultNewRowValue);
                }
                else
                {
                    ctl.Text = Convert.ToString(this.Value);
                }

            }
            catch (ArgumentOutOfRangeException e)
            {
                ctl.Text = Convert.ToString(this.DefaultNewRowValue);
            }

        }

        public override Type EditType
        {
            get
            {
                return typeof(DecimalEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(Decimal);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return 0;
            }
        }
    }

    class DecimalEditingControl : TextBoxNumeric, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public DecimalEditingControl()
        {
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value is String)
                {
                    try
                    {
                        this.Text = value.ToString();
                    }
                    catch
                    {
                        this.Text = "0";
                    }
                }
            }
        }

        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnTextChanged(EventArgs eventargs)
        {
            valueChanged = true;
            if (this.EditingControlDataGridView != null)
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);

            base.OnTextChanged(eventargs);
        }
    }

}
