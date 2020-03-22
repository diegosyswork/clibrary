using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SysWork.IO
{
    /// <summary>
    /// Simple CSV Export
    /// </summary>
    /// <remarks>
    /// This CSV Exporter, 
    /// </remarks>
    /// <example>
    /// <code>
    ///   CsvExport myExport = new CsvExport();
    ///
    ///   myExport.AddRow();
    ///   myExport["Region"] = "New York, USA";
    ///   myExport["Sales"] = 100000;
    ///   myExport["Date Opened"] = new DateTime(2003, 12, 31);
    ///
    ///   myExport.AddRow();
    ///   myExport["Region"] = "Sydney \"in\" Australia";
    ///   myExport["Sales"] = 50000;
    ///   myExport["Date Opened"] = new DateTime(2005, 1, 1, 9, 30, 0);
    ///
    ///   //Then you can do any of the following three output options:
    ///   
    ///   string myCsv = myExport.Export();
    ///   myExport.ExportToFile("Somefile.csv");
    ///   byte[] myCsvData = myExport.ExportToBytes();
    /// 
    /// </code>
    /// </example>
    public class CsvExport
    {
        /// <summary>
        /// Gets or sets the delimitator.
        /// </summary>
        /// <value>
        /// The delimitator.
        /// </value>
        public char FieldDelimiter { get; set; } = ';';
        
        /// <summary>
        /// To keep the ordered list of column names.
        /// </summary>
        private List<string> _fields = new List<string>();

        /// <summary>
        /// The list of rows
        /// </summary>
        private List<Dictionary<string, object>> _rows = new List<Dictionary<string, object>>();

        /// <summary>
        /// The current row
        /// </summary>
        private Dictionary<string, object> _currentRow { get { return _rows[_rows.Count - 1]; } }

        /// <summary>
        /// Set the value to a column.
        /// </summary>
        public object this[string field]
        {
            set
            {
                // Keep track of the field names, because the dictionary loses the ordering
                if (!_fields.Contains(field)) _fields.Add(field);
                _currentRow[field] = value;
            }
        }

        /// <summary>
        /// Create a new row, call this before setting any fields on a row.
        /// </summary>
        public void AddRow()
        {
            _rows.Add(new Dictionary<string, object>());
        }

        /// <summary>
        /// Converts a value to how it should output in a csv file
        /// If it has a comma, it needs surrounding with double quotes
        /// Eg Sydney, Australia -> "Sydney, Australia"
        /// Also if it contains any double quotes ("), then they need to be replaced with quad quotes[sic] ("")
        /// Eg "Dangerous Dan" McGrew -> """Dangerous Dan"" McGrew"
        /// </summary>
        private string MakeValueCsvFriendly(object value)
        {
            if (value == null) return "";
            if (value is INullable && ((INullable)value).IsNull) return "";
            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime)value).ToString("yyyy-MM-dd");
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string output = value.ToString();
            if (output.Contains(FieldDelimiter) || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';
            if (Regex.IsMatch(output, @"(?:\r\n|\n|\r)"))
                output = string.Join(" ", Regex.Split(output, @"(?:\r\n|\n|\r)"));
            return output;
        }

        /// <summary>
        /// Output all rows as a CSV returning a string.
        /// </summary>
        public string Export(bool includeHeader)
        {
            StringBuilder sb = new StringBuilder();

            // The header
            if (includeHeader)
            {
                foreach (string field in _fields)
                    sb.Append(field).Append(FieldDelimiter);
                sb.AppendLine();
            }

            // The rows
            foreach (Dictionary<string, object> row in _rows)
            {
                foreach (string field in _fields)
                    sb.Append(MakeValueCsvFriendly(row[field])).Append(FieldDelimiter);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Exports to a file
        /// </summary>
        public void ExportToFile(string path, bool includeHeader)
        {
            File.WriteAllText(path, Export(includeHeader));
        }

        /// <summary>
        /// Exports as raw UTF8 bytes
        /// </summary>
        public byte[] ExportToBytes(bool includeHeader)
        {
            return Encoding.UTF8.GetBytes(Export(includeHeader));
        }
    }
}
