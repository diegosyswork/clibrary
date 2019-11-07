using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.IO
{
    /// <summary>
    /// Export IDataReader to CSV.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class DataReaderToCSV
    {
        /// <summary>
        /// Exports the specified data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="includeHeaderAsFirstRow">if set to <c>true</c> [include header as first row].</param>
        /// <param name="fieldDelimiter">Character Delimiter.</param>
        /// <returns></returns>
        public static string Export(IDataReader dataReader, bool includeHeaderAsFirstRow = true, string fieldDelimiter = ";")
        {
            DataTable dataTable = new DataTable();
            StringBuilder csvRows = new StringBuilder();
            string row = "";

            int columns;

            try
            {
                dataTable.Load(dataReader);
                columns = dataTable.Columns.Count;
                //Create Header
                if (includeHeaderAsFirstRow)
                {
                    for (int index = 0; index < columns; index++)
                    {
                        row += (dataTable.Columns[index].ToString().Replace(fieldDelimiter,""));
                        if (index < columns - 1)
                            row += (fieldDelimiter);
                    }
                    row += (Environment.NewLine);
                }

                csvRows.Append(row);

                //Create Rows
                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    row = "";
                    //Row
                    for (int index = 0; index < columns - 1; index++)
                    {
                        string value = dataTable.Rows[rowIndex][index].ToString();

                        //If type of field is string
                        if (dataTable.Rows[rowIndex][index] is string)
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
                        if (index < columns - 1)
                            row += fieldDelimiter;
                    }
                    dataTable.Rows[rowIndex][columns - 1].ToString().ToString().Replace(fieldDelimiter, " ");
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
