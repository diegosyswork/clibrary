using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.IO
{
    /// <summary>
    ///  This class handles fixed length files.
    /// </summary>
    public class FixedLengthFileReader
    {
        /// <summary>
        /// Gets the index of the current line.
        /// </summary>
        /// <returns></returns>
        public long CurrentLineIndex { get { return _currentlLineIndex; } private set { } }

        /// <summary>
        /// Determines whether [is file open].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is file open]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFileOpen { get { return _fileOpen; } private set { } }

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <value>
        /// The record count.
        /// </value>
        public long RecordCount
        {
            get
            {
                if (_lines == null)
                    return 0;

                return _lines.Count();
            }
            private set { }
        }


        private string _fileName;

        private string[] _lines = null;

        private long _currentlLineIndex;

        private Hashtable _hashFixedFields;

        private bool _flag;

        private bool _fileOpen;

        private FixedFields _lastFixedFieldCreated = null;

        /// <summary>
        /// Get the value of the field of the current record
        /// </summary>
        public string this[string fieldName]
        {
            get
            {
                return GetFieldValue(fieldName);
            }
        }

        public FixedLengthFileReader(string fileName)
        {
            this._currentlLineIndex = 0;
            this._fileName = fileName;
            this._hashFixedFields = new Hashtable();
            this._flag = true;
            this._fileOpen = false;
        }

        /// <summary>
        /// Reads the current file
        /// </summary>
        public void ReadFile()
        {
            try
            {
                _lines = File.ReadAllLines(_fileName);
                _fileOpen = true;
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
        public void AddField(string fieldName, int initField, int endField)
        {
            // TODO: validar que inicio o fin no esten dentro de otro campo
            _lastFixedFieldCreated = new FixedFields(initField, endField);

            _hashFixedFields.Add(fieldName, _lastFixedFieldCreated);
        }
        
        /// <summary>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="lenght">Lenght of the field. The initial and final position is calculated based on the final position of the previous field</param>
        /// </summary>
        public void AddField(string fieldName, int lenght)
        {
            int pos = 0;
            if (_lastFixedFieldCreated != null)
                pos = _lastFixedFieldCreated.EndField;

            pos++;

            AddField(fieldName, pos, pos + lenght - 1);
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <returns></returns>
        /// <exception cref="SysWork.IO.NoFieldsSetException">No fields Set</exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="SysWork.IO.FileNotOpenException"></exception>
        /// <exception cref="SysWork.IO.LineLenghException"></exception>
        public string GetFieldValue(string fieldName, long lineNumber)
        {
            if (_hashFixedFields.Count == 0)
                throw new NoFieldsSetException("No fields Set");

            if (lineNumber > _lines.Count())
                throw new IndexOutOfRangeException();

            if (!_fileOpen)
                throw new FileNotOpenException();

            FixedFields ff = (FixedFields)_hashFixedFields[fieldName];
            string lineValue = _lines[lineNumber - 1];
            int initField = ff.InitField;
            int endField = ff.EndField;

            if (lineValue.Equals(String.Empty))
                return "";

            if (initField + (endField - initField) > lineValue.Length)
                throw new LineLenghException();


            return _lines[lineNumber - 1].Substring(initField - 1, endField - initField + 1);
        }
        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public string GetFieldValue(string fieldName)
        {
            return GetFieldValue(fieldName, _currentlLineIndex + 1);
        }

        /// <summary>
        /// Gets the current line value.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SysWork.IO.FileNotOpenException"></exception>
        public string GetCurrentLineValue()
        {
            if (!_fileOpen)
                throw new FileNotOpenException();

            if (_lines != null)
            {
                if (_lines.Count() > 0)
                    return _lines[_currentlLineIndex];
                else
                    return null;
            }
            else
                return null;
        }


        /// <summary>
        ///     Determines whether [has next line].
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [has next line]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="SysWork.IO.FileNotOpenException"></exception>
        public bool HasNextLine()
        {
            if (!_fileOpen)
                throw new FileNotOpenException();

            if (_flag)
            {
                _flag = false;
                if (_lines.Count() > 0)
                    return true;
                else
                    return false;
            }

            if (_currentlLineIndex + 1 < _lines.Count())
            {
                _currentlLineIndex++;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Gets the length of the record.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SysWork.IO.NoFieldsSetException"></exception>
        public int GetRecordLength()
        {
            int length = 0;

            if (_hashFixedFields.Count == 0)
                throw new NoFieldsSetException();

            foreach (DictionaryEntry pair in _hashFixedFields)
            {
                FixedFields fixedField = (FixedFields)pair.Value;
                length += (fixedField.EndField - fixedField.InitField) + 1;
            }

            return length;
        }

    }

    class FixedFields
    {
        public int InitField { get; set; }
        public int EndField { get; set; }

        public FixedFields(int initField, int endField)
        {
            this.InitField = initField;
            this.EndField = endField;
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
    class FileNotOpenException : Exception
    {
        public FileNotOpenException() : base()
        {
        }
        public FileNotOpenException(string message) : base(message)
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
