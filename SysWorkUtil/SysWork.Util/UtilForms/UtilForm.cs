using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SysWork.Util.UtilForms
{
    public class UtilForm
    {
        /// <summary>
        /// Limpia los controles (TextBox, ComboBox,CheckBox,RadioButton)
        /// </summary>
        /// <param name="container">Control contenedor que se recorrera</param>
        /// <param name="excludedNamesControl">lista de controles separada por comas que no seran tenidos en cuenta</param>
        public static void CleanControls(Control container,params string[] excludedNamesControl)
        {

            var list = excludedNamesControl.ToList();

            foreach (Control control in container.Controls)
            {
                if (!list.Exists(c => control.Name == c))
                {
                    if (control.Controls.Count > 0)
                    {
                        CleanControls(control, excludedNamesControl);
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
                    }
                }
            }
        }
        /// <summary>
        /// Establece si los controles de un formulario se pueden editar
        /// Solo lo hace en (TextBox, MaskedTextBox, CheckBox, ComboBox)
        /// </summary>
        /// <param name="container">Control contenedor que se recorrera</param>
        /// <param name="allowEdit">permite la edicion</param>
        /// <param name="excludedNameControls">lista de controles separada por coma que no seran tenidos en cuenta</param>
        /// <param name="inverseStateNameControls">lista de controles separada por coma que deberan funcionar a la inversa de allowEdit </param>
        public static void EditModeControls(Control container, bool allowEdit, string excludedNameControls, string inverseStateNameControls)
        {

            string[] localExcludedNameControls = new string[1];
            string[] localInverseStateNameControls = new string[1];

            if (!string.IsNullOrEmpty(excludedNameControls))
                localExcludedNameControls = excludedNameControls.Split(',');

            if (!string.IsNullOrEmpty(inverseStateNameControls))
                localInverseStateNameControls = inverseStateNameControls.Split(',');

            var listExcluded = localExcludedNameControls.ToList();
            var listInverse = localInverseStateNameControls.ToList();

            foreach (Control control in container.Controls)
            {
                if (!listExcluded.Exists(c => control.Name == c))
                {
                    if (control.Controls.Count > 0)
                    {
                        EditModeControls(control,allowEdit, excludedNameControls, inverseStateNameControls);
                    }
                    else
                    {
                        if (control is TextBox)
                        {
                            ((TextBox)control).ReadOnly = (listInverse.Exists(i => control.Name == i)) ? allowEdit : !allowEdit;
                        }
                        else if (control is RadioButton)
                        {
                            ((RadioButton)control).Enabled = (listInverse.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Enabled = (listInverse.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                        else if (control is ComboBox)
                        {
                            ((ComboBox)control).Enabled = (listInverse.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                        else if (control is MaskedTextBox)
                        {
                            ((MaskedTextBox)control).Enabled = (listInverse.Exists(i => control.Name == i)) ? !allowEdit : allowEdit;
                        }
                    }
                }
            }
        }
    }
}
