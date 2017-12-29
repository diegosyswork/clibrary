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
        private string fileName;
        List<Record> recordList;
        private string caracterCR = System.Environment.NewLine;


        public FixedLenghtFileWriter()
        {
            initValues(null, caracterCR);
        }
        public FixedLenghtFileWriter(string fileName)
        {
            initValues(fileName, caracterCR);
        }

        public FixedLenghtFileWriter(string fileName, String caracterCR)
        {
            initValues(fileName, caracterCR);
        }

        public void setFileName(string fileName)
        {
            this.fileName = fileName;
        }

        private void initValues(string fileName, String caracterCR)
        {
            this.fileName = fileName;
            this.caracterCR = caracterCR;
            recordList = new List<Record>();
        }
        public void addRecord(Record record)
        {
            recordList.Add(record);
        }
        public void clearRecord()
        {
            recordList.Clear();
        }
        public int getRecordCount()
        {
            return recordList.Count;
        }

        public string getContentFile()
        {
            StringBuilder sbFileContent = new StringBuilder();

            for (int pos = 0; pos < recordList.Count; pos++)
            {
                sbFileContent.Append(recordList[pos].ToString());
                sbFileContent.Append(caracterCR);
            }

            return sbFileContent.ToString();
        }
        public bool saveFile()
        {
            bool returnBool = true;

            if (fileName == null || fileName.Trim().Equals(""))
            {
                throw new ArgumentException("No se ha informado el nombre del archivo");
            }

            try
            {
                File.WriteAllText(fileName, getContentFile());
            }
            catch (Exception ex)
            {
                returnBool = false;
                throw ex;
            }

            return returnBool;
        }
    }

    public enum FieldFormat
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
        public string fieldName { get; set; }
        public int initField { get; set; }
        public int endField { get; set; }
        public FieldFormat fieldformat { get; set; }
        public int length { get; private set; }

        public FieldDefinition(string fieldName, int initField, int endField, FieldFormat fieldformat)
        {
            this.fieldName = fieldName;
            this.initField = initField;
            this.endField = endField;
            this.fieldformat = fieldformat;
            this.length = endField - initField + 1;
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

        public void addFieldDefinition(FieldDefinition fieldDefinition)
        {
            if (!validateFieldDefinitionFormatLenght(fieldDefinition))
            {
                throw new FormatException("El formato elegido no cabe en el largo del campo informado, verifiquelo y vueva a intentarlo");
            }
            else
            {
                try
                {
                    fieldsDefinition.Add(fieldDefinition.fieldName, fieldDefinition);
                    lastFieldDefinitionCreated = fieldDefinition;
                }
                catch (Exception ex)
                {
                    throw new Exception("ocurrio la siguiente excepcion mientras se intentaba agregar la definicion  (" + fieldDefinition.fieldName + ") " + ex.Message);
                }
            }
        }

        private void addFieldDefinition(string fieldName, int initField, int endField, FieldFormat fieldformat, string defaultValue = null)
        {
            FieldDefinition fieldDefinition = new FieldDefinition(fieldName, initField, endField, fieldformat);
            addFieldDefinition(fieldDefinition);
        }

        public void addFieldDefinition(string fieldName, int lenght, FieldFormat fieldformat)
        {
            int initField = 0, endField = 0;
            if (lastFieldDefinitionCreated == null)
            {
                initField = 1;
                endField = lenght;
            }
            else
            {
                initField = lastFieldDefinitionCreated.endField + 1;
                endField = initField + lenght - 1;
            }

            FieldDefinition fieldDefinition = new FieldDefinition(fieldName, initField, endField, fieldformat);
            addFieldDefinition(fieldDefinition);
        }

        public Dictionary<string, FieldDefinition> getFieldsDefinition()
        {
            return fieldsDefinition;
        }

        private bool validateFieldDefinitionFormatLenght(FieldDefinition fieldDefinition)
        {
            bool returnValue = true;

            if ((fieldDefinition.fieldformat == FieldFormat.DATETIMEF_ddMM) || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_ddMM)
            || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_MMyy) || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_yyMM)
            || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_yyMM) || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_HHmm)
            || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_mmss))
            {
                returnValue = (fieldDefinition.length >= 4);
            }
            else if ((fieldDefinition.fieldformat == FieldFormat.DATETIMEF_ddMMyy) || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_MMyyyy)
            || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_yyMMdd) || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_yyyyMM)
            || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_HHmmss))
            {
                returnValue = (fieldDefinition.length >= 6);
            }
            else if ((fieldDefinition.fieldformat == FieldFormat.DATETIMEF_yyyyMMdd) || (fieldDefinition.fieldformat == FieldFormat.DATETIMEF_ddMMyyyy))
            {
                returnValue = (fieldDefinition.length >= 8);
            }
            else if ((fieldDefinition.fieldformat == FieldFormat.NUMBER_INT) || (fieldDefinition.fieldformat == FieldFormat.NUMBER_INT_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.length >= 1);
            }
            else if ((fieldDefinition.fieldformat == FieldFormat.NUMBER_1DEC) || (fieldDefinition.fieldformat == FieldFormat.NUMBER_1DEC_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.length >= 3);
            }
            else if ((fieldDefinition.fieldformat == FieldFormat.NUMBER_2DEC) || (fieldDefinition.fieldformat == FieldFormat.NUMBER_2DEC_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.length >= 4);
            }
            else if ((fieldDefinition.fieldformat == FieldFormat.NUMBER_3DEC) || (fieldDefinition.fieldformat == FieldFormat.NUMBER_3DEC_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.length >= 5);
            }
            else if ((fieldDefinition.fieldformat == FieldFormat.NUMBER_4DEC) || (fieldDefinition.fieldformat == FieldFormat.NUMBER_4DEC_LEFT_FILLED_W_ZEROS))
            {
                returnValue = (fieldDefinition.length >= 6);
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
            createRecordValueFieldsAndCompleteWithEmptyValues();
        }

        public Record this[string fieldName]
        {
            get
            {
                currentFieldName = fieldName;
                return this;
            }
        }

        public string getValue()
        {
            return (string)recordValues[currentFieldName];
        }

        public void setValue(String value)
        {
            recordValues[currentFieldName] = formatValue(value);
        }

        public void setValue(DateTime value)
        {
            recordValues[currentFieldName] = formatValue(value);
        }

        public void setValue(double value)
        {
            recordValues[currentFieldName] = formatValue(value);
        }
        public void setValue(float value)
        {
            recordValues[currentFieldName] = formatValue(value);
        }
        public void setValue(decimal value)
        {
            recordValues[currentFieldName] = formatValue(value);
        }
        public void setValue(int value)
        {
            recordValues[currentFieldName] = formatValue(value);
        }
        public void setValue(long value)
        {
            recordValues[currentFieldName] = formatValue(value);
        }

        private string formatValue(object value)
        {
            FieldDefinition fd = (FieldDefinition)recordDefinition.getFieldsDefinition()[currentFieldName];
            string newValue = "";

            if (isDateField(fd.fieldformat))
            {
                newValue = getDateValueFormated(value, fd);
            }
            else if (isStringField(fd.fieldformat))
            {
                newValue = value.ToString();
                if (newValue.Length > fd.length)
                    newValue = newValue.Substring(0, fd.length);
                else
                {
                    if (fd.fieldformat == FieldFormat.STRING_RIGHT)
                        newValue = newValue.PadLeft(fd.length);
                    else
                        newValue = newValue.PadRight(fd.length);
                }
            }
            else if (isNumberField(fd.fieldformat))
            {
                newValue = getNumberValueFormated(value, fd);
            }
            return newValue;
        }

        private string getDateValueFormated(object value, FieldDefinition fd)
        {
            string format = getFormatDateByEnum(fd.fieldformat);
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

        private string getNumberValueFormated(object value, FieldDefinition fd)
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

            switch (fd.fieldformat)
            {
                case FieldFormat.NUMBER_AS400F_2DEC:
                    newValue = number2AS400Format(numberToEvaluate, fd.length - 2, 2);
                    break;
                case FieldFormat.NUMBER_AS400F_3DEC:
                    newValue = number2AS400Format(numberToEvaluate, fd.length - 3, 3);
                    break;
                case FieldFormat.NUMBER_AS400F_4DEC:
                    newValue = number2AS400Format(numberToEvaluate, fd.length - 4, 4);
                    break;
                case FieldFormat.NUMBER_INT:
                    newValue = string.Format("{0:#0}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length);
                    break;
                case FieldFormat.NUMBER_1DEC:
                    newValue = string.Format("{0:#0.0}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length);
                    break;
                case FieldFormat.NUMBER_2DEC:
                    newValue = string.Format("{0:#0.00}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length);
                    break;
                case FieldFormat.NUMBER_3DEC:
                    newValue = string.Format("{0:#0.000}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length);
                    break;
                case FieldFormat.NUMBER_4DEC:
                    newValue = string.Format("{0:#0.0000}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length);
                    break;
                case FieldFormat.NUMBER_INT_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length, '0');
                    break;
                case FieldFormat.NUMBER_1DEC_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0.0}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length, '0');
                    break;
                case FieldFormat.NUMBER_2DEC_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0.00}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length, '0');
                    break;
                case FieldFormat.NUMBER_3DEC_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0.000}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length, '0');
                    break;
                case FieldFormat.NUMBER_4DEC_LEFT_FILLED_W_ZEROS:
                    newValue = string.Format("{0:#0.0000}", numberToEvaluate);
                    newValue = newValue.PadLeft(fd.length, '0');
                    break;
            }

            return newValue;
        }

        private bool isDateField(FieldFormat fieldformat)
        {
            bool isDateField = (fieldformat == FieldFormat.DATETIMEF_ddMMyyyy) ||
            (fieldformat == FieldFormat.DATETIMEF_ddMMyy) ||
            (fieldformat == FieldFormat.DATETIMEF_ddMM) ||
            (fieldformat == FieldFormat.DATETIMEF_MMyy) ||
            (fieldformat == FieldFormat.DATETIMEF_MMyyyy) ||
            (fieldformat == FieldFormat.DATETIMEF_yyyyMMdd) ||
            (fieldformat == FieldFormat.DATETIMEF_yyyyMM) ||
            (fieldformat == FieldFormat.DATETIMEF_MMdd) ||
            (fieldformat == FieldFormat.DATETIMEF_yyMMdd) ||
            (fieldformat == FieldFormat.DATETIMEF_yyMM) ||
            (fieldformat == FieldFormat.DATETIMEF_HHmmss) ||
            (fieldformat == FieldFormat.DATETIMEF_HHmm) ||
            (fieldformat == FieldFormat.DATETIMEF_mmss);

            return isDateField;
        }

        private bool isStringField(FieldFormat fieldformat)
        {
            return (fieldformat == FieldFormat.STRING) || (fieldformat == FieldFormat.STRING_RIGHT);
        }

        private bool isNumberField(FieldFormat fieldformat)
        {
            bool isNumberField =

            (fieldformat == FieldFormat.NUMBER_INT) ||
            (fieldformat == FieldFormat.NUMBER_1DEC) ||
            (fieldformat == FieldFormat.NUMBER_2DEC) ||
            (fieldformat == FieldFormat.NUMBER_3DEC) ||
            (fieldformat == FieldFormat.NUMBER_4DEC) ||
            (fieldformat == FieldFormat.NUMBER_INT_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == FieldFormat.NUMBER_1DEC_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == FieldFormat.NUMBER_2DEC_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == FieldFormat.NUMBER_3DEC_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == FieldFormat.NUMBER_4DEC_LEFT_FILLED_W_ZEROS) ||
            (fieldformat == FieldFormat.NUMBER_AS400F_2DEC) ||
            (fieldformat == FieldFormat.NUMBER_AS400F_3DEC) ||
            (fieldformat == FieldFormat.NUMBER_AS400F_4DEC);
            return isNumberField;
        }

        private string getFormatDateByEnum(FieldFormat fieldformat)
        {
            string newFormat = "";
            switch (fieldformat)
            {
                case FieldFormat.DATETIMEF_ddMMyyyy:
                    newFormat = "{0:ddMMyyyy}";
                    break;
                case FieldFormat.DATETIMEF_ddMMyy:
                    newFormat = "{0:ddMMyy}";
                    break;
                case FieldFormat.DATETIMEF_ddMM:
                    newFormat = "{0:ddMM}";
                    break;
                case FieldFormat.DATETIMEF_MMyy:
                    newFormat = "{0:MMyy}";
                    break;
                case FieldFormat.DATETIMEF_MMyyyy:
                    newFormat = "{0:MMyyyy}";
                    break;
                case FieldFormat.DATETIMEF_yyyyMMdd:
                    newFormat = "{0:yyyyMMdd}";
                    break;
                case FieldFormat.DATETIMEF_yyyyMM:
                    newFormat = "{0:yyyyMM}";
                    break;
                case FieldFormat.DATETIMEF_MMdd:
                    newFormat = "{0:MMdd}";
                    break;
                case FieldFormat.DATETIMEF_yyMMdd:
                    newFormat = "{0:yyMMdd}";
                    break;
                case FieldFormat.DATETIMEF_yyMM:
                    newFormat = "{0:yyMM}";
                    break;
                case FieldFormat.DATETIMEF_HHmmss:
                    newFormat = "{0:HHmmss}";
                    break;
                case FieldFormat.DATETIMEF_HHmm:
                    newFormat = "{0:HHmm}";
                    break;
                case FieldFormat.DATETIMEF_mmss:
                    newFormat = "{0:mmss}";
                    break;
            }
            return newFormat;
        }
        private void createRecordValueFieldsAndCompleteWithEmptyValues()
        {
            /*
            foreach (string fieldName in recordDefinition.getFieldDefinition().Keys)
            {
                FieldDefinition fieldDefinition = (FieldDefinition) recordDefinition.getFieldDefinition()[fieldName];
                recordValues.Add(fieldName, defaultFieldValue(fieldDefinition));
            } 

            foreach (KeyValuePair<string, FieldDefinition> kv in recordDefinition.getFieldDefinition())
            {
                recordValues.Add(kv.Key, defaultFieldValue(kv.Value));
            }
            */

            foreach (string key in recordDefinition.getFieldsDefinition().Keys)
            {
                FieldDefinition fieldDefinition = recordDefinition.getFieldsDefinition()[key];
                recordValues.Add(key, defaultFieldValue(fieldDefinition));
            }
        }

        private string defaultFieldValue(FieldDefinition fieldDefinition)
        {
            string defaultValue = "";

            switch (fieldDefinition.fieldformat)
            {
                case FieldFormat.STRING:
                    defaultValue = new string(' ', fieldDefinition.length);
                    break;

                default:
                    defaultValue = new string(' ', fieldDefinition.length);
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

        private string number2AS400Format(decimal number, int positionsIntegerPart, int positionsDecimalPart)
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

