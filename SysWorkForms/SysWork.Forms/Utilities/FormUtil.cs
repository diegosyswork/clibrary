using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SysWork.Forms.Utilities
{
    public enum EEditModeState
    {
        AllowEdit,
        NoAllowEdit
    }

    public static class FormUtil
    {
        public static void CleanControls(Control container)
        {
            var list = new List<string>();
            CleanControls(container, list);
        }

        public static void CleanControls(Control container, params Control[] excludedControls)
        {
            var list = new List<string>();
            foreach (var item in excludedControls)
                list.Add(item.Name);
        }

        public static void CleanControls(Control container, string[] excludedControls)
        {
            CleanControls(container, excludedControls.ToList());
        }

        public static void CleanControls(Control container, List<Control> excludedControls)
        {
            var list = new List<string>();
            foreach (var item in excludedControls)
                list.Add(item.Name);

            CleanControls(container,list);
        }

        public static void CleanControls(Control container, List<string> excludedControls)
        {
            foreach (Control control in container.Controls)
            {
                if (!excludedControls.Exists(c => control.Name == c))
                {
                    if (control.Controls.Count > 0)
                    {
                        CleanControls(control, excludedControls);
                    }
                    else
                    {
                        if (control is TextBox)
                        {
                            ((TextBox)control).Clear();
                        }
                        else if (control is RadioButton)
                        {
                            ((RadioButton)control).Checked = false;
                        }
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is ComboBox)
                        {
                            ((ComboBox)control).Text = "";
                            ((ComboBox)control).SelectedItem = null;
                        }
                        else if (control is MaskedTextBox)
                        {
                            ((MaskedTextBox)control).Clear();
                        }
                        else if (control is ListView)
                        {
                            ((ListView)control).Items.Clear();
                        }
                    }
                }
            }
        }
        public static void EditModeControls(Control container, EEditModeState editModeState, List<string> uniqueKeyControls)
        {
            var excludedControlsNames = new List<string>();
            EditModeControls(container, editModeState, uniqueKeyControls, excludedControlsNames);
        }
        public static void EditModeControls(Control container, EEditModeState editModeState, List<Control> uniqueKeyControls)
        {
            var listUniqueKeyControls = new List<String>();
            foreach (var item in uniqueKeyControls)
                listUniqueKeyControls.Add(item.Name);

            var excludedControlsNames = new List<string>();
            EditModeControls(container, editModeState,  listUniqueKeyControls, excludedControlsNames);
        }

        public static void EditModeControls(Control container, EEditModeState editModeState, List<string> uniqueKeyControls, List<string> excludedControlsNames)
        {
            bool allowEdit = (editModeState == EEditModeState.AllowEdit);

            foreach (Control control in container.Controls)
            {
                if (!excludedControlsNames.Exists(c => control.Name == c))
                {
                    if (control.Controls.Count > 0)
                    {
                        EditModeControls(control, editModeState , uniqueKeyControls,excludedControlsNames);
                    }
                    else
                    {
                        if (control is TextBox)
                        {
                            ((TextBox)control).ReadOnly = (uniqueKeyControls.Exists(i => control.Name == i)) ? allowEdit : !allowEdit;
                        }
                        else if (control is RadioButton)
                        {
                            ((RadioButton)control).Enabled = (uniqueKeyControls.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Enabled = (uniqueKeyControls.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                        else if (control is ComboBox)
                        {
                            ((ComboBox)control).Enabled = (uniqueKeyControls.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                        else if (control is MaskedTextBox)
                        {
                            ((MaskedTextBox)control).ReadOnly = (uniqueKeyControls.Exists(i => control.Name == i)) ? allowEdit : !allowEdit;
                        }
                        else if (control is ListView)
                        {
                            ((ListView)control).Enabled = (uniqueKeyControls.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                    }
                }
            }
        }

        public static void AsignNulleableValueToCombo(ComboBox combo, long? value)
        {
            if (value == null)
                combo.SelectedItem = null;
            else
                combo.SelectedValue = value;
        }

        public static long? AsignNulleableComboValueToEntity(ComboBox combo)
        {
            if (combo.SelectedItem == null)
                return null;
            else
                return (long) combo.SelectedValue;
        }


        /// <summary>
        /// Verifica si el combo tiene un texto que no esta dentro de la lista.
        /// </summary>
        /// <param name="combo"></param>
        /// <returns></returns>
        public static bool ValidateComboFromDataSource(ComboBox combo, bool required = false)
        {
            if (required)
            {
                return combo.SelectedItem != null;
            }
            else
            {
                if (combo.SelectedItem == null && !combo.Text.Equals(""))
                    return false;
            }


            return true;
        }

        public static void SetDateFormatMaskedTextBox(MaskedTextBox msk)
        {
            msk.Mask = "00/00/0000";
            msk.Text = "";
        }

        public static bool ValidateDateFormatMaskedTextBox(MaskedTextBox msk, out string errMessage, bool required = false)
        {
            errMessage = "";
            if (required && (msk.Text.Trim().Equals("") || !msk.MaskCompleted))
            {
                errMessage = "Debe informar la fecha";
                return false;
            }

            if (msk.MaskCompleted)
            {
                if (!DateTime.TryParse(msk.Text, out DateTime fecha))
                {
                    errMessage = "La fecha ingresada no es valida";
                    return false;
                }
            }
            else
            {
                if (!required)
                    msk.Text = "";
            }
            return true;
        }

        public static void ListViewCheckAll(ListView listView)
        {
            foreach (ListViewItem item in listView.Items)
                item.Checked = true;
        }

        public static void ListViewUnCheckAll(ListView listView)
        {
            foreach (int index in listView.CheckedIndices)
                listView.Items[index].Checked = false;
        }

        public static void ListViewInverseCheck(ListView listView)
        {
            foreach (ListViewItem item in listView.Items)
                item.Checked = !item.Checked;
        }

        public static void ListViewSelectAll(ListView listView)
        {
            foreach (ListViewItem item in listView.Items)
                item.Selected = true;
        }

        public static void ListViewUnSelectAll(ListView listView)
        {
            foreach (int index in listView.SelectedIndices)
                listView.Items[index].Checked = false;
        }

        public static void ListViewInverseSelect(ListView listView)
        {
            foreach (ListViewItem item in listView.Items)
                item.Selected = !item.Selected;
        }

        public static bool ValidateDateFormatMaskedFromTo(MaskedTextBox msk1, MaskedTextBox msk2, out string errMessage, string nombreParametro = null)
        {
            errMessage = "";
            if (nombreParametro == null)
                nombreParametro = "fecha";

            DateTime fecha1;
            DateTime fecha2;

            if (msk1.MaskCompleted && msk2.MaskCompleted)
            {
                if (!DateTime.TryParse(msk1.Text, out fecha1))
                {
                    errMessage = "La " + nombreParametro + " desde ingresada no es valida";
                    return false;
                }
                if (!DateTime.TryParse(msk2.Text, out fecha2))
                {
                    errMessage = "La " + nombreParametro + " hasta ingresada no es valida";
                    return false;
                }

                if (fecha1 > fecha2)
                {
                    errMessage = "" + nombreParametro + " desde mayor que hasta";
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 
        /// En caso que en un DataGridView necesitemos representar una entidad que
        /// posea otra entidad dentro, esta rutina se encarga de acceder a la propiedad. 
        /// 
        /// Ejemplo: Entidad Factura, tiene una entidad Cliente  deseamos acceder a la Razon Social
        /// 
        ///  Factura.Cliente.RazonSocial
        /// 
        /// En el DataPropertyName de la columna deberemos poner:Factura.Cliente.RazonSocial
        /// 
        /// Y llamar a este metodo en el evento CellFormatting
        /// 
        /// </summary>
        /// <param name="DataGridView1"></param>
        /// <param name="e"></param>
        public static void DataGridAccessProperty(DataGridView DataGridView1, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewColumn column = DataGridView1.Columns[e.ColumnIndex];
            if (column.DataPropertyName.Contains("."))
            {

                object data = DataGridView1.Rows[e.RowIndex].DataBoundItem;
                string[] properties = column.DataPropertyName.Split('.');
                for (int i = 0; i < properties.Length && data != null; i++)
                    data = data.GetType().GetProperty(properties[i]).GetValue(data);

                DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = data;
            }
        }

        public static ListViewItem FindItemContainsText(ListView listview, string text)
        {
            foreach (ListViewItem item in listview.Items)
            {
                if (item.Text.ToLower().Contains(text.ToLower()))
                    return item;
                else
                {
                    foreach (ListViewItem.ListViewSubItem subitem in item.SubItems)
                        if (subitem.Text.ToLower().Contains(text.ToLower()))
                            return item;
                }
            }

            return null;
        }

    }
}
