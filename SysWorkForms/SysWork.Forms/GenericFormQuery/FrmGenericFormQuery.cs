using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SysWork.Forms.FormsABM.FormsUtil;

namespace SysWork.Forms.GenericFormQuery
{
    
    public partial class FrmGenericFormQuery: Form
    {
        
        private ListViewColumnSorter _lvwColumnSorter;
        
        public FrmGenericFormQuery()
        {
            InitializeComponent();
            _lvwColumnSorter = new ListViewColumnSorter();
        }
        private void FrmConsultaGenerica_Shown(object sender, EventArgs e)
        {
            txtCriterioBusqueda.Text = "";
            txtCriterioBusqueda.Focus();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel ;
            this.Close();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) 
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void txtCriterioBusqueda_TextChanged(object sender, EventArgs e)
        {
            listView1.SelectedItems.Clear();
            if ((!txtCriterioBusqueda.Text.Equals("")) && (listView1.Items.Count > 0))
            {
                ListViewItem l = listView1.FindItemWithText(txtCriterioBusqueda.Text,  true, 1);
                if (l != null)
                {
                    listView1.Items[l.Index].Selected = true;
                    listView1.Items[l.Index].EnsureVisible();
                }
            }
        }
        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            listView1.ListViewItemSorter = _lvwColumnSorter;

            if (e.Column == _lvwColumnSorter.SortColumn)
            {
                if (_lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    _lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                _lvwColumnSorter.SortColumn = e.Column;
                _lvwColumnSorter.Order = SortOrder.Ascending;
            }

            listView1.Sort();
        }

        private void tsbSelectAll_Click(object sender, EventArgs e)
        {
            foreach (var item in listView1.Items)
            {
                ((ListViewItem)item).Checked = true;
            }

        }

        private void tsbUnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (var item in listView1.Items)
            {
                ((ListViewItem)item).Checked = false;
            }
        }

        private void tsbInvert_Click(object sender, EventArgs e)
        {
            foreach (var item in listView1.Items)
            {
                ((ListViewItem)item).Checked = !((ListViewItem)item).Checked;
            }

        }

    }
}
