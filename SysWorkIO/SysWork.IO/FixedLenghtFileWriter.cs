using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SysWork.IO
{
    public class FixedLenghtFileWriter
    {
        private string _fileName;
        private List<Record> _recordList;
        private string _caracterCR = System.Environment.NewLine;

        public FixedLenghtFileWriter()
        {
            InitValues(null, _caracterCR);
        }
        public FixedLenghtFileWriter(string fileName)
        {
            InitValues(fileName, _caracterCR);
        }

        public FixedLenghtFileWriter(string fileName, String caracterCR)
        {
            InitValues(fileName, caracterCR);
        }

        public void SetFileName(string fileName)
        {
            this._fileName = fileName;
        }

        private void InitValues(string fileName, String caracterCR)
        {
            this._fileName = fileName;
            this._caracterCR = caracterCR;
            _recordList = new List<Record>();
        }
        public void AddRecord(Record record)
        {
            _recordList.Add(record);
        }
        public void ClearRecord()
        {
            _recordList.Clear();
        }
        public int GetRecordCount()
        {
            return _recordList.Count;
        }

        public string GetContentFile()
        {
            StringBuilder sbFileContent = new StringBuilder();

            for (int pos = 0; pos < _recordList.Count; pos++)
            {
                sbFileContent.Append(_recordList[pos].ToString());
                sbFileContent.Append(_caracterCR);
            }

            return sbFileContent.ToString();
        }
        public bool SaveFile()
        {
            bool returnBool = true;

            if (_fileName == null || _fileName.Trim().Equals(""))
            {
                throw new ArgumentException("No se ha informado el nombre del archivo");
            }

            try
            {
                File.WriteAllText(_fileName, GetContentFile());
            }
            catch (Exception ex)
            {
                returnBool = false;
                throw ex;
            }

            return returnBool;
        }
    }

    public enum EFieldFormat
    {
        DATETIMEF_ddMMyyyy,                  // Recibe un Date o String Parseable a Date
        DATETIMEF_ddMMyy,                    // 
        DATETIMEF_ddMM,                      //  
        DATETIMEF_MMyy,                      //  
        DATETIMEF_MMyyyy,                    //  
        DATETIMEF_yyyyMMdd,                  //    
        DATETIMEF_yyyyMM,                    //    
        DATETIMEF_MMdd,                      //    
        DATETIMEF_yyMMdd,                    // 
        DATETIMEF_yyMM,                      // 
        DATETIMEF_HHmmss,                    // 
        DATETIMEF_HHmm,                      // 
        DATETIMEF_mmss,                      // 
        STRING,                              // Recibe un String si supera el largo lo recorta (alinea a la IZQUIERDA) 
        STRING_RIGHT,                        // Recibe un String si supera el largo lo recorta (alinea a la DERECHA)
        NUMBER_AS400F_2DEC,                  // Recibe un numero (float, double, int, decimal)
        NUMBER_AS400F_3DEC,                  //  
        NUMBER_AS400F_4DEC,                  // 
        NUMBER_INT,                          // 
        NUMBER_1DEC,                         // 
        NUMBER_2DEC,                         // 
        NUMBER_3DEC,                         // 
        NUMBER_4DEC,                         // 
        NUMBER_INT_LEFT_FILLED_W_ZEROS,      // 
        NUMBER_1DEC_LEFT_FILLED_W_ZEROS,     // 
        NUMBER_2DEC_LEFT_FILLED_W_ZEROS,     //  
        NUMBER_3DEC_LEFT_FILLED_W_ZEROS,     //  
        NUMBER_4DEC_LEFT_FILLED_W_ZEROS,     //  
    }
    public class FieldDefinition
    {
        public string FieldName { get; set; }
        public int InitField { get; set; }
        public int EndField { get; set; }
        public EFieldFormat Fieldformat { get; set; }
        public int Length { get; private set; }

        public FieldDefinition(string fieldName, int initField, int endField, EFieldFormat fieldformat)
        {
            this.FieldName = fieldName;
            this.InitField = initField;
            this.EndField = endField;
            this.Fieldformat = fieldformat;
            this.Length = endField - initField + 1;
        }
    }
    public class RecordDefinition
    {
        Dictionary<string, FieldDefinition> fieldsDefinition;
        private FieldDefinition lastFieldDefinitionCreated;

        public RecordDefinition()
        {
            fieldsDefinition = new Dictionary<string, FieldDefinition>();
            lastFieldDefinitionCreated = null;
        }

        public void AddFieldDefinition(FieldDefinition fieldDefinition)
        {
            if (!ValidateFieldDefinitionFormatLenght(fieldDefinition))
            {
                throw new FormatException("El formato elegido no cabe en el largo del campo informado, verifiquelo y vueva a intentarlo");
            }
            else
            {
                try
                {
                    fieldsDefinition.Add(fieldDefinition.FieldName, fieldDefinition);
                    lastFieldDefinitionCreated = fieldDefinition;
                }
                catch (Exception ex)
                {
                    throw new Exception("ocurrio la siguiente excepcion mientras se intentaba agregar la definicion  (" + fieldDefinition.FieldName + ") " + ex.Message);
                }
            }
        }

        private void AddFieldDefinition(string fieldName, int initField, int endField, EFieldFormat fieldformat, string defaultValue = null)
        {
            FieldDefinition fieldDefinition = new FieldDefinition(fieldName, initField, endField, fieldformat);
            AddFieldDefinition(fieldDefinition);
        }

        public void AddFieldDefinition(string fieldName, int lenght, EFieldFormat fieldformat)
        {
            int initField = 0, endField = 0;
            if (lastFieldDefinitionCreated == null)
            {
                initField = 1;
                endField = lenght;
            }
            else
            {
                initField = lastFieldDefinitionCreated.EndField + 1;
                endField = initField + lenght - 1;
            }

            FieldDefinition fieldDefinition = new FieldDefinition(fieldName, initField, endField, fieldformat);
            AddFieldDefinition(fieldDefinition);
        }

        public Dictionary<string, FieldDefinition> GetFieldsDefinition()
        {
            return fieldsDefinition;
        }

        private bool ValidateFieldDefinitionFormatLenght(FieldDefinition fieldDefinition)
        {
            bool returnValue = true;

            if ((fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_ddMM) || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_ddMM)
            || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_MMyy) || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_yyMM)
            || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_yyMM) || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_HHmm)
            || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_mmss))
            {
                returnValue = (fieldDefinition.Length >= 4);
            }
            else if ((fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_ddMMyy) || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_MMyyyy)
            || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_yyMMdd) || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_yyyyMM)
            || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_HHmmss))
            {
                returnValue = (fieldDefinition.Length >= 6);
            }
            else if ((fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_yyyyMMdd) || (fieldDefinition.Fieldformat == EFieldFormat.DATETIMEF_ddMMyyyy))
            {
                returnValue = (fieldDefinition.Length >= 8);
            }
            else if ((fieldDefinition.Fieldformat == EFieldFormat.NUMBER_INT) || (fieldDefinition.Fieldformat == EFieldFormat.NUMBER_INT_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.Length >= 1);
            }
            else if ((fieldDefinition.Fieldformat == EFieldFormat.NUMBER_1DEC) || (fieldDefinition.Fieldformat == EFieldFormat.NUMBER_1DEC_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.Length >= 3);
            }
            else if ((fieldDefinition.Fieldformat == EFieldFormat.NUMBER_2DEC) || (fieldDefinition.Fieldformat == EFieldFormat.NUMBER_2DEC_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.Length >= 4);
            }
            else if ((fieldDefinition.Fieldformat == EFieldFormat.NUMBER_3DEC) || (fieldDefinition.Fieldformat == EFieldFormat.NUMBER_3DEC_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.Length >= 5);
            }
            else if ((fieldDefinition.Fieldformat == EFieldFormat.NUMBER_4DEC) || (fieldDefinition.Fieldformat == EFieldFormat.NUMBER_4DEC_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.Length >= 6);
            }

            return returnValue;
        }
    }

    public class Record
    {
        private RecordDefinition recordDefinition;
        private Dictionary<string, string> recordValues;
        private string currentFieldName;

        public Record(RecordDefinition recordDefinition)
        {
            this.recordDefinition = recordDefinition;
            this.recordValues = new Dictionary<string, string>();
            CreateRecordValueFieldsAndCompleteWithEmptyValues();
        }

        public Record this[string fieldName]
        {
            get
            {
                currentFieldName = fieldName;
                return this;
            }
        }

        public string GetValue()
        {
            return (string)recordValues[currentFieldName];
        }

        public void SetValue(String value)
        {
            recordValues[currentFieldName] = FormatValue(value);
        }

        public void SetValue(DateTime value)
        {
            recordValues[currentFieldName] = FormatValue(value);
        }

        public void SetValue(double value)
        {
            recordValues[currentFieldName] = FormatValue(value);
        }
        public void SetValue(float value)
        {
            recordValues[currentFieldName] = FormatValue(value);
        }
        public void SetValue(decimal value)
        {
            recordValues[currentFieldName] = FormatValue(value);
        }
        public void SetValue(int value)
        {
            recordValues[currentFieldName] = FormatValue(value);
        }
        public void SetValue(long value)
        {
            recordValues[currentFieldName] = FormatValue(value);
        }

        private string FormatValue(object value)
        {
            FieldDefinition fd = (FieldDefinition)recordDefinition.GetFieldsDefinition()[currentFieldName];
            string newValue = "";

            if (IsDateField(fd.Fieldformat))
            {
                newValue = GetDateValueFormated(value, fd);
            }
            else if (IsStringField(fd.Fieldformat))
            {
                newValue = value.ToString();
                if (newValue.Length > fd.Length)
                    newValue = newValue.Substring(0, fd.Length);
                else
                {
                    if (fd.Fieldformat == EFieldFormat.STRING_RIGHT)
                        newValue = newValue.PadLeft(fd.Length);
                    else
                        newValue = newValue.PadRight(fd.Length);
                }
            }
            else if (IsNumberField(fd.Fieldformat))
            {
                newValue = GetNumberValueFormated(value, fd);
            }
            return newValue;
        }

        private string GetDateValueFormated(object value, FieldDefinition fd)
        {
            string format = GetFormatDateByEnum(fd.Fieldformat);
            DateTime dateToEvaluate;

            if (value.GetType() == typeof(DateTime))
                dateToEvaluate = (DateTime)value;
            else if (value.GetType() == typeof(string) || value.GetType() == typeof(String))
            {
                if (!DateTime.TryParse((String)value, out dateToEvaluate))
                    throw new ArgumentException("EL VALOR RECIBO <--" + value.ToString() + "--> NO PUEDO CONVERTIRSE A DATETIME");
            }
            else
            {
                throw new FormatException("Solo pueden recibirse valores del tipo DATETIME, String o string");
            }

            return String.Format(format, dateToEvaluate);
        }

        private string GetNumberValueFormated(object value, FieldDefinition fd)
        {
            decimal numberToEvaluate = 0;
            string newValue = "0";

            if ((value.GetType() == typeof(String)) || (value.GetType() == typeof(string)))
            {
                if (!decimal.TryParse(value.ToString(), out numberToEvaluate))
                    throw new ArgumentException("EL VALOR RECIBO <--" + value.ToString() + "--> NO PUEDO CONVERTIRSE A NUMERO");

            }
            else if ((value.GetType() == typeof(decimal)) || (value.GetType() == typeof(float)) || (value.GetType() == typeof(double)) || (value.GetType() == typeof(int)) || (value.GetType() == typeof(long)))
            {
                numberToEvaluate = decimal.Parse(value.ToString());
            }
            else
            {
                throw new FormatException("Solo pueden Formatearse valores del tipo String o string(Parceable a numero), decimal, float, double, int o long");
            }

            switch (fd.Fieldformat)
            {
                case EFieldFormat.NUMBER_AS400F_2DEC:
                    newValue = Number2AS400Format(numberToEvaluate, fd.Length - 2, 2);
                    break;
                case EFieldFormat.NUMBER_AS400F_3DEC:
                    newValue = Number2AS400Format(numberToEvaluate, fd.Length - 3, 3);
                    break;
                case EFieldFormat.NUMBER_AS400F_4DEC:
                    newValue = Number2AS400Format(numberToEvaluate, fd.Length - 4, 4);
                    break;
                case EFieldFormat.NUMBER_INT:
                    newValue = string.Format("{0:#0}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length);
                    break;
                case EFieldFormat.NUMBER_1DEC:
                    newValue = string.Format("{0:#0.0}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length);
                    break;
                case EFieldFormat.NUMBER_2DEC:
                    newValue = string.Format("{0:#0.00}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length);
                    break;
                case EFieldFormat.NUMBER_3DEC:
                    newValue = string.Format("{0:#0.000}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length);
                    break;
                case EFieldFormat.NUMBER_4DEC:
                    newValue = string.Format("{0:#0.0000}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length);
                    break;
                case EFieldFormat.NUMBER_INT_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length, '0');
                    break;
                case EFieldFormat.NUMBER_1DEC_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0.0}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length, '0');
                    break;
                case EFieldFormat.NUMBER_2DEC_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0.00}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length, '0');
                    break;
                case EFieldFormat.NUMBER_3DEC_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0.000}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length, '0');
                    break;
                case EFieldFormat.NUMBER_4DEC_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0.0000}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.Length, '0');
                    break;
            }

            return newValue;
        }

        private bool IsDateField(EFieldFormat fieldformat)
        {
            bool isDateField = (fieldformat == EFieldFormat.DATETIMEF_ddMMyyyy) ||
            (fieldformat == EFieldFormat.DATETIMEF_ddMMyy) ||
            (fieldformat == EFieldFormat.DATETIMEF_ddMM) ||
            (fieldformat == EFieldFormat.DATETIMEF_MMyy) ||
            (fieldformat == EFieldFormat.DATETIMEF_MMyyyy) ||
            (fieldformat == EFieldFormat.DATETIMEF_yyyyMMdd) ||
            (fieldformat == EFieldFormat.DATETIMEF_yyyyMM) ||
            (fieldformat == EFieldFormat.DATETIMEF_MMdd) ||
            (fieldformat == EFieldFormat.DATETIMEF_yyMMdd) ||
            (fieldformat == EFieldFormat.DATETIMEF_yyMM) ||
            (fieldformat == EFieldFormat.DATETIMEF_HHmmss) ||
            (fieldformat == EFieldFormat.DATETIMEF_HHmm) ||
            (fieldformat == EFieldFormat.DATETIMEF_mmss);

            return isDateField;
        }

        private bool IsStringField(EFieldFormat fieldformat)
        {
            return (fieldformat == EFieldFormat.STRING) || (fieldformat == EFieldFormat.STRING_RIGHT);
        }

        private bool IsNumberField(EFieldFormat fieldformat)
        {
            bool isNumberField =

            (fieldformat == EFieldFormat.NUMBER_INT) ||
            (fieldformat == EFieldFormat.NUMBER_1DEC) ||
            (fieldformat == EFieldFormat.NUMBER_2DEC) ||
            (fieldformat == EFieldFormat.NUMBER_3DEC) ||
            (fieldformat == EFieldFormat.NUMBER_4DEC) ||
            (fieldformat == EFieldFormat.NUMBER_INT_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == EFieldFormat.NUMBER_1DEC_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == EFieldFormat.NUMBER_2DEC_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == EFieldFormat.NUMBER_3DEC_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == EFieldFormat.NUMBER_4DEC_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == EFieldFormat.NUMBER_AS400F_2DEC) ||
            (fieldformat == EFieldFormat.NUMBER_AS400F_3DEC) ||
            (fieldformat == EFieldFormat.NUMBER_AS400F_4DEC);
            return isNumberField;
        }

        private string GetFormatDateByEnum(EFieldFormat fieldformat)
        {
            string newFormat = "";
            switch (fieldformat)
            {
                case EFieldFormat.DATETIMEF_ddMMyyyy:
                    newFormat = "{0:ddMMyyyy}";
                    break;
                case EFieldFormat.DATETIMEF_ddMMyy:
                    newFormat = "{0:ddMMyy}";
                    break;
                case EFieldFormat.DATETIMEF_ddMM:
                    newFormat = "{0:ddMM}";
                    break;
                case EFieldFormat.DATETIMEF_MMyy:
                    newFormat = "{0:MMyy}";
                    break;
                case EFieldFormat.DATETIMEF_MMyyyy:
                    newFormat = "{0:MMyyyy}";
                    break;
                case EFieldFormat.DATETIMEF_yyyyMMdd:
                    newFormat = "{0:yyyyMMdd}";
                    break;
                case EFieldFormat.DATETIMEF_yyyyMM:
                    newFormat = "{0:yyyyMM}";
                    break;
                case EFieldFormat.DATETIMEF_MMdd:
                    newFormat = "{0:MMdd}";
                    break;
                case EFieldFormat.DATETIMEF_yyMMdd:
                    newFormat = "{0:yyMMdd}";
                    break;
                case EFieldFormat.DATETIMEF_yyMM:
                    newFormat = "{0:yyMM}";
                    break;
                case EFieldFormat.DATETIMEF_HHmmss:
                    newFormat = "{0:HHmmss}";
                    break;
                case EFieldFormat.DATETIMEF_HHmm:
                    newFormat = "{0:HHmm}";
                    break;
                case EFieldFormat.DATETIMEF_mmss:
                    newFormat = "{0:mmss}";
                    break;
            }
            return newFormat;
        }
        private void CreateRecordValueFieldsAndCompleteWithEmptyValues()
        {
            foreach (string key in recordDefinition.GetFieldsDefinition().Keys)
            {
                FieldDefinition fieldDefinition = recordDefinition.GetFieldsDefinition()[key];
                recordValues.Add(key, DefaultFieldValue(fieldDefinition));
            }
        }

        private string DefaultFieldValue(FieldDefinition fieldDefinition)
        {
            string defaultValue = "";

            switch (fieldDefinition.Fieldformat)
            {
                case EFieldFormat.STRING:
                    defaultValue = new string(' ', fieldDefinition.Length);
                    break;

                default:
                    defaultValue = new string(' ', fieldDefinition.Length);
                    break;
            }

            return defaultValue;
        }
        public override string ToString()
        {
            string outputValue = "";
            foreach (string key in recordValues.Keys)
            {
                outputValue += (string)recordValues[key];
            }
            return outputValue;
        }

        private string Number2AS400Format(decimal number, int positionsIntegerPart, int positionsDecimalPart)
        {
            string newValue = "";
            string format = new string('0', positionsIntegerPart);

            if (positionsDecimalPart > 0)
                format += "." + new string('0', positionsDecimalPart);

            newValue = string.Format("{0:" + format + "}", number);
            newValue = newValue.Replace(".", "");
            newValue = newValue.Replace(",", "");

            return newValue;
        }
    }
}

