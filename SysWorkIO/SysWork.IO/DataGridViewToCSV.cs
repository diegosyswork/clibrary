using System;
using System.Text;
using System.Windows.Forms;

namespace SysWork.IO
{
    public static class DataGridViewToCSV
    {
        public static string Export(DataGridView datagridView, bool includeHeaderAsFirstRow = true, string fieldDelimiter = ";")
        {
            StringBuilder csvRows = new StringBuilder();
            string row = "";

            int columns;
            int rowIndex;
            int columnIndex;

            try
            {
                columns = datagridView.Columns.Count;
                //Create Header
                if (includeHeaderAsFirstRow)
                {
                    for (columnIndex = 0; columnIndex < columns; columnIndex++)
                    {
                        row += (datagridView.Columns[columnIndex].HeaderText.ToString().Replace(fieldDelimiter, ""));
                        if (columnIndex < columns - 1)
                            row += (fieldDelimiter);
                    }
                    row += (Environment.NewLine);
                }

                csvRows.Append(row);

                //Create Rows

                for (rowIndex = 0; rowIndex < datagridView.Rows.Count; rowIndex++)
                {
                    if (rowIndex == 83)
                    {
                        Console.Write("");
                    }

                    row = "";
                    //Row
                    for (columnIndex = 0; columnIndex < columns - 1; columnIndex++)
                    {
                        string value = datagridView.Rows[rowIndex].Cells[columnIndex].Value.ToString();

                        //If type of field is string
                        if (datagridView.Rows[rowIndex].Cells[columnIndex].Value is string)
                        {
                            //If double quotes are used in value, ensure each are replaced by double quotes.
                            if (value.IndexOf("\"") >= 0)
                                value = value.Replace("\"", "\"\"");

                            //If separtor are is in value, ensure it is put in double quotes.
                            if (value.IndexOf(fieldDelimiter) >= 0)
                                value = "\"" + value + "\"";

                            //If string contain new line character
                            while (value.Contains("\r"))
                            {
                                value = value.Replace("\r", "");
                            }
                            while (value.Contains("\n"))
                            {
                                value = value.Replace("\n", "");
                            }
                        }
                        row += value;
                        if (columnIndex < columns - 1)
                            row += fieldDelimiter;
                    }
                    //datagridView.Rows[rowIndex].Cells[columns - 1].ToString().Replace(fieldDelimiter, " ");
                    row += Environment.NewLine;
                    csvRows.Append(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return csvRows.ToString();
        }
    }
}
