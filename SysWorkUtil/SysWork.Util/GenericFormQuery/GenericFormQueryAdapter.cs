using System.Collections.Generic;
using System.Windows.Forms;

namespace SysWork.Util.GenericFormQuery
{
    public class GenericFormQueryAdapter<T>
    {

        public bool CheckBoxes { get; set; }
        public bool MultiSelect { get; set; }

        protected ListView ListViewForm;
        protected List<T> DataSourceList;

        protected bool AcceptForm { get; set; }
        protected T EntityForm { get; set; }
        protected List<T> EntityListForm { get; set; }
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

            foreach (T t in DataSourceList)
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
            frmGenericFormQuery.lblCantRegistros.Text = "Cantidad de Registros: " + DataSourceList.Count;

            frmGenericFormQuery.listView1.CheckBoxes = CheckBoxes;
            frmGenericFormQuery.listView1.MultiSelect = MultiSelect;
            frmGenericFormQuery.tsSeleccion.Visible = CheckBoxes;

            frmGenericFormQuery.ShowDialog();
            AcceptForm = (frmGenericFormQuery.DialogResult == System.Windows.Forms.DialogResult.OK);

            if (AcceptForm)
            {
                if (!CheckBoxes && ListViewForm.SelectedItems.Count == 0)
                {
                    AcceptForm = false;
                }
                else
                {
                    if (!CheckBoxes && !MultiSelect)
                    {
                        EntityForm = (T)ListViewForm.SelectedItems[0].Tag;
                    }
                    else
                    {
                        EntityListForm= new List<T>();
                        if (CheckBoxes)
                        {
                            foreach (var index in frmGenericFormQuery.listView1.CheckedIndices)
                            {
                                EntityListForm.Add((T)frmGenericFormQuery.listView1.Items[(int)index].Tag);
                            }
                        }
                        else
                        {
                            foreach (var index in frmGenericFormQuery.listView1.SelectedIndices)
                            {
                                EntityListForm.Add((T)frmGenericFormQuery.listView1.Items[(int)index].Tag);
                            }
                        }
                    }
                }
            }
        }
    }
}
