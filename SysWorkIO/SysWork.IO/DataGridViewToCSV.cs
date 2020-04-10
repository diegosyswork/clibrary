using System;
using System.IO;
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

            int columns,rowIndex,columnIndex;

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

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="datagridView">The datagrid view.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="maxLinePerFile">The maximum line per file. If unlimited, 0</param>
        /// <param name="includeHeaderAsFirstRow">if set to <c>true</c> [include header as first row].</param>
        /// <param name="fieldDelimiter">The field delimiter.</param>
        /// <returns></returns>
        public static bool ExportToFile(DataGridView datagridView, string fileName, int maxLinePerFile = 0,  bool includeHeaderAsFirstRow = true, string fieldDelimiter = ";")
        {
            string row = "";
            int columns, rowIndex, columnIndex;
            string header = "";
            columns = datagridView.Columns.Count;
            for (columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                header += (datagridView.Columns[columnIndex].HeaderText.ToString().Replace(fieldDelimiter, ""));
                if (columnIndex < columns - 1)
                    header += (fieldDelimiter);
            }

            int counter = 0;
            int fileNumber = 1;
            StreamWriter streamWriter = CreateStreamWriter(fileName, fileNumber);

            if (includeHeaderAsFirstRow)
                streamWriter.WriteLine(header);

            //Create Rows
            for (rowIndex = 0; rowIndex < datagridView.Rows.Count; rowIndex++)
            {
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
                            value = value.Replace("\r", "");

                        while (value.Contains("\n"))
                            value = value.Replace("\n", "");
                    }
                    row += value;
                    if (columnIndex < columns - 1)
                        row += fieldDelimiter;
                }

                if (counter >= maxLinePerFile)
                {
                    streamWriter.Flush();
                    counter = 0;
                    fileNumber++;
                    streamWriter = CreateStreamWriter(fileName, fileNumber);
                    if (includeHeaderAsFirstRow)
                        streamWriter.WriteLine(header);
                }

                streamWriter.WriteLine(row);
                counter++;
            }

            streamWriter.Flush();
            return true;
        }

        private static StreamWriter CreateStreamWriter(string fileName, int fileNumber)
        {
            string outputFileName = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + "_" + string.Format("{0:0000}", fileNumber) + Path.GetExtension(fileName));
            if (File.Exists(outputFileName))
                File.Delete(outputFileName);

            var fileStream = new FileStream(outputFileName, FileMode.CreateNew);
            return new StreamWriter(fileStream, Encoding.UTF8);
        }
    }
}
