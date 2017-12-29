using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///  This class handles fixed length files.
/// </summary>
namespace SysWork.IO
{
    public class FixedLengthFileReader
    {
        private string fileName;

        private string[] lines = null;

        private long currentlLineIndex;

        private Hashtable hashFixedFields;

        private bool flag;

        private bool fileOpen;

        private FixedFields lastFixedFieldCreated = null;

        /// <summary>
        /// Get the value of the field of the current record
        /// </summary>
        public string this[string fieldName]
        {
            get
            {
                return getFieldValue(fieldName);
            }
        }

        public FixedLengthFileReader(string fileName)
        {
            this.currentlLineIndex = 0;
            this.fileName = fileName;
            this.hashFixedFields = new Hashtable();
            this.flag = true;
            this.fileOpen = false;
        }

        /// <summary>
        /// Reads the current file
        /// </summary>
        public void readFile()
        {
            try
            {
                lines = File.ReadAllLines(fileName);
                fileOpen = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="initField">Initial position of the field</param>
        /// <param name="endField">Final position of the field</param>
        /// </summary>

        public void addField(string fieldName, int initField, int endField)
        {
            // TODO: validar que inicio o fin no esten dentro de otro campo

            lastFixedFieldCreated = new FixedFields(initField, endField);

            hashFixedFields.Add(fieldName, lastFixedFieldCreated);
        }
        /// <summary>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="lenght">Lenght of the field. The initial and final position is calculated based on the final position of the previous field</param>
        /// </summary>
        public void addField(string fieldName, int lenght)
        {
            int pos = 0;
            if (lastFixedFieldCreated != null)
                pos = lastFixedFieldCreated.endField;

            pos++;

            addField(fieldName, pos, pos + lenght - 1);
        }

        public string getFieldValue(string fieldName, long lineNumber)
        {
            if (hashFixedFields.Count == 0)
                throw new NoFieldsSetException("No fields Set");

            if (lineNumber > lines.Count())
                throw new IndexOutOfRangeException();

            if (!fileOpen)
                throw new FileNoOpenException();

            FixedFields ff = (FixedFields)hashFixedFields[fieldName];
            string lineValue = lines[lineNumber - 1];
            int initField = ff.initField;
            int endField = ff.endField;

            if (lineValue.Equals(String.Empty))
                return "";

            if (initField + (endField - initField) > lineValue.Length)
                throw new LineLenghException();


            return lines[lineNumber - 1].Substring(initField - 1, endField - initField + 1);
        }
        public string getFieldValue(string fieldName)
        {
            return getFieldValue(fieldName, currentlLineIndex + 1);
        }

        public string getCurrentLineValue()
        {
            if (!fileOpen)
                throw new FileNoOpenException();

            if (lines != null)
            {
                if (lines.Count() > 0)
                    return lines[currentlLineIndex];
                else
                    return null;
            }
            else
                return null;
        }

        public long getCurrentLineIndex()
        {
            return currentlLineIndex;
        }

        public bool hasNextLine()
        {
            if (!fileOpen)
                throw new FileNoOpenException();

            if (flag)
            {
                flag = false;
                if (lines.Count() > 0)
                    return true;
                else
                    return false;
            }

            if (currentlLineIndex + 1 < lines.Count())
            {
                currentlLineIndex++;
                return true;
            }

            return false;
        }
        public int getRecordLength()
        {
            int length = 0;

            if (hashFixedFields.Count == 0)
                throw new NoFieldsSetException();

            foreach (DictionaryEntry pair in hashFixedFields)
            {
                FixedFields fixedField = (FixedFields)pair.Value;
                length += (fixedField.endField - fixedField.initField) + 1;
            }

            return length;
        }

        public bool isFileOpen()
        {
            return fileOpen;
        }

        public long getRecordsCount()
        {
            if (lines == null)
                return 0;

            return lines.Count();
        }

    }

    class FixedFields
    {
        public int initField { get; set; }
        public int endField { get; set; }

        public FixedFields(int initField, int endField)
        {
            this.initField = initField;
            this.endField = endField;
        }
    }

    class NoFieldsSetException : Exception
    {
        public NoFieldsSetException() : base()
        {
        }
        public NoFieldsSetException(string message) : base(message)
        {
        }
    }
    class FileNoOpenException : Exception
    {
        public FileNoOpenException() : base()
        {
        }
        public FileNoOpenException(string message) : base(message)
        {
        }
    }
    class LineLenghException : Exception
    {
        public LineLenghException() : base()
        {
        }
        public LineLenghException(string message) : base(message)
        {
        }
    }
}
