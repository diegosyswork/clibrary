using System.Collections.Generic;
using System.Windows.Forms;

namespace SysWork.Forms.Interfaces
{
    public interface ICRUDFormWListview<T>
    {
        /// <summary>
        /// Sets the columns and ListView properties.
        /// </summary>
        void SetColumnsAndListViewProperties();

        /// <summary>
        /// Loads the listview data.
        /// </summary>
        void LoadListViewData(IList<T> data);

        /// <summary>
        /// Loads the listview data.
        /// </summary>
        void CleanListViewData();

        /// <summary>
        /// Creates the ListView item.
        /// </summary>
        /// <returns></returns>
        ListViewItem CreateListViewItem(T item);
    }
}
