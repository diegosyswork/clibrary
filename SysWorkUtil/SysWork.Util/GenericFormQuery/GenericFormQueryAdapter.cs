using System.Collections.Generic;
using System.Windows.Forms;

namespace SysWork.Util.GenericFormQuery
{
    public class GenericFormQueryAdapter<T>
    {
        protected ListView ListViewForm;
        protected List<T> ListForm;

        protected bool AcceptForm { get; set; }
        protected T EntityForm { get; set; }
        protected string FormTitle { get; set; }

        private FrmGenericFormQuery frmGenericFormQuery;

        public GenericFormQueryAdapter()
        {
            frmGenericFormQuery = new FrmGenericFormQuery();
            ListViewForm = frmGenericFormQuery.listView1;
        }

        protected void FillListView()
        {
            SetColumns();

            foreach (T t in ListForm)
            {
                ListViewForm.Items.Add(GetListViewItem(t));
            }
        }

        protected virtual void SetColumns()
        {

        }

        protected virtual ListViewItem GetListViewItem(T t)
        {
            return null;
        }

        protected void ShowForm()
        {
            FillListView();

            frmGenericFormQuery.Text = FormTitle;
            frmGenericFormQuery.lblCantRegistros.Text = "Cantidad de Registros: " + ListForm.Count;

            frmGenericFormQuery.ShowDialog();
            AcceptForm = (frmGenericFormQuery.DialogResult == System.Windows.Forms.DialogResult.OK);

            if (AcceptForm)
            {
                if (ListViewForm.SelectedItems.Count == 0)
                {
                    AcceptForm = false;
                }
                else
                {
                    EntityForm = (T)ListViewForm.SelectedItems[0].Tag;
                }
            }
        }

    }
}
