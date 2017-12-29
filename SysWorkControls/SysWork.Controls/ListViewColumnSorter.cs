using System;
using System.Collections;
using System.Windows.Forms;

/// <summary>
/// This class is an implementation of the 'IComparer' interface.
/// </summary>
namespace SysWork.Controls
{
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        private bool compareAllAsString;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter(bool compareAllAsString = false)
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();

            // by Default compare all columns as String 
            this.compareAllAsString = compareAllAsString;
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            if (!compareAllAsString)
            {
                // Intento Convertir a DateTime
                DateTime x1, y1;
                if (!DateTime.TryParse(listviewX.SubItems[ColumnToSort].Text, out x1))
                    x1 = DateTime.MinValue;

                if (!DateTime.TryParse(listviewY.SubItems[ColumnToSort].Text, out y1))
                    y1 = DateTime.MinValue;

                if (x1 != DateTime.MinValue && y1 != DateTime.MinValue)
                {
                    compareResult = DateTime.Compare(x1, y1);
                    return returnCompare(compareResult);
                }

                // Intento Convertir a Decimal
                Decimal x2, y2;
                if (!Decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out x2))
                    x2 = Decimal.MinValue;

                if (!Decimal.TryParse(listviewY.SubItems[ColumnToSort].Text, out y2))
                    y2 = Decimal.MinValue;

                if (x2 != Decimal.MinValue && y2 != Decimal.MinValue)
                {
                    compareResult = Decimal.Compare(x2, y2);
                    return returnCompare(compareResult);
                }
            }

            // Compare the two items
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            return returnCompare(compareResult);

        }


        public int returnCompare(int compareResult)
        {

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

    }
}
