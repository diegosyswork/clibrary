using System;
using System.Text;

namespace SysWork.Data.GenericRepository.CodeWriter
{
    /// <summary>
    /// Helper to write code
    /// </summary>
    public static class CodeWriterHelper
    {
        /// <summary>
        /// Adds the using.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static string AddUsing(string assembly)
        {
            return string.Format("using {0};", assembly);
        }

        /// <summary>
        /// Starts the namespace.
        /// </summary>
        /// <param name="NameSpace">The name space.</param>
        /// <returns></returns>
        public static string StartNamespace(string NameSpace)
        {
            string ret;
            ret = string.Format("namespace {0}", NameSpace) + Environment.NewLine;
            ret += "{";
            return ret;
        }

        /// <summary>
        /// Ends the namespace.
        /// </summary>
        /// <returns></returns>
        public static string EndNamespace()
        {
            return "}" ;
        }

        /// <summary>
        /// Adds the database table attribute.
        /// </summary>
        /// <param name="dbTableName">Name of the database table.</param>
        /// <returns></returns>
        public static string AddDbTableAttribute(string dbTableName)
        {
            string ret;
            ret = string.Format("\t[DbTable (Name = \"{0}\")]", dbTableName) ;
            return ret;
        }

        /// <summary>
        /// Starts the class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="inherits">The inherits.</param>
        /// <returns></returns>
        public static string StartClass(string className, string inherits = null)
        {

            string ret = "";
            if (inherits==null) 
                ret = string.Format("\tpublic class {0}", className) + Environment.NewLine;
            else
                ret = string.Format("\tpublic class {0} : {1}", className, inherits) + Environment.NewLine;

            ret += "\t{";

            return ret;
        }

        /// <summary>
        /// Ends the class.
        /// </summary>
        /// <returns></returns>
        public static string EndClass()
        {
            return "\t}" ;
        }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="allowDBNull">if set to <c>true</c> [allow database null].</param>
        /// <returns></returns>
        public static string GetDataType(string dataType, bool allowDBNull = false)
        {
            dataType = dataType.Replace("System.", "");
            if (dataType == "Boolean")
                dataType = "bool";
            else if (dataType == "Int32")
                dataType = "long";
            else if (dataType != "DateTime")
                dataType = dataType.ToLower();

            if (allowDBNull && (dataType.ToLower() != "string") && (dataType.ToLower() != "bool"))
                dataType = dataType + "?";

            return dataType;
        }

        /// <summary>
        /// Adds the database column attribute.
        /// </summary>
        /// <param name="isIdentity">if set to <c>true</c> [is identity].</param>
        /// <param name="isPrimary">if set to <c>true</c> [is primary].</param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static string AddDbColumnAttribute(bool isIdentity = false, bool isPrimary = false, string ColumnName=null)
        {
            string ret = "";

            ret = "\t\t[DbColumn(";
            ret += isIdentity ? "IsIdentity = true" : "";
            ret += isPrimary ? (isIdentity ? "," : "") + " IsPrimary = true" : "";
            ret += ColumnName != null ? " ColumnName = \"" + ColumnName + "\"" : "";
            ret += ")]" ;
            return ret;
        }

        /// <summary>
        /// Adds the public property.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static string AddPublicProperty(string dataType, string propertyName)
        {
            string ret = "";

            ret += string.Format("\t\tpublic {0} {1} ", dataType, propertyName);
            ret += "{ get; set; }";

            return ret;
        }

        /// <summary>
        /// Tables the name of the name to class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static string TableNameToClassName(string tableName)
        {
            string result = "";
            string[] words = CamelCaseToSpaceSeparated(tableName);
            foreach (var word in words)
                result += SingularizeWord(word);

            return result;

        }
        private static string SingularizeWord(string word)
        {
            string ret = "";
            if (word.ToLower().EndsWith("as"))
            {
                ret = word.Substring(0, word.Length - 1);
            }
            else if (word.ToLower().EndsWith("jes"))
            {
                ret = word.Substring(0, word.Length - 1);
            }
            else if (word.ToLower().EndsWith("lles"))
            {
                ret = word.Substring(0, word.Length - 1);
            }
            else if (word.ToLower().EndsWith("ls"))
            {
                ret = word.Substring(0, word.Length - 1);
            }
            else if (word.ToLower().EndsWith("rnes"))
            {
                ret = word.Substring(0, word.Length - 1);
            }
            else if (word.ToLower().EndsWith("bles"))
            {
                ret = word.Substring(0, word.Length - 1);
            }
            else if (word.ToLower().EndsWith("es"))
            {
                ret = word.Substring(0, word.Length - 2);
                if (ret.EndsWith("t"))
                    ret += "e";
            }
            else if (word.ToLower().EndsWith("is"))
            {
                ret = word.Substring(0, word.Length - 1);

            }
            else if (word.ToLower().EndsWith("os"))
            {
                ret = word.Substring(0, word.Length - 1);
            }
            else if (word.ToLower().EndsWith("us"))
            {
                ret = word.Substring(0, word.Length - 1);
            }
            else
            {
                ret = word;
            }

            ret = ret.Substring(0, 1).ToUpper() + ret.Substring(1);

            return ret;
        }

        /// <summary>
        /// Camels the case to space separated.
        /// </summary>
        /// <param name="camelCaseString">The camel case string.</param>
        /// <returns></returns>
        public static string[] CamelCaseToSpaceSeparated(string camelCaseString)
        {
            if (string.IsNullOrEmpty(camelCaseString))
            {
                return new string[0];
            }

            var res = new StringBuilder();
            res.Append(camelCaseString[0]);
            for (var i = 1; i < camelCaseString.Length; i++)
            {
                if (char.IsUpper(camelCaseString[i]))
                {
                    res.Append(' ');
                }
                res.Append(camelCaseString[i]);

            }
            return res.ToString().Split(' ');
        }


    }
}
