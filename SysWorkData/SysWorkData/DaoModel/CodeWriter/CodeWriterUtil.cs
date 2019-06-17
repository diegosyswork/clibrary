using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.DaoModel.CodeWriter
{
    public static class CodeWriterUtil
    {
        public static string AddUsing(string assembly)
        {
            return string.Format("using {0};", assembly);
        }
        public static string StartNamespace(string NameSpace)
        {
            string ret;
            ret = string.Format("namespace {0}", NameSpace) + Environment.NewLine;
            ret += "{";
            return ret;
        }
        public static string EndNamespace()
        {
            return "}" ;
        }

        public static string AddDbTableAttribute(string dbTableName)
        {
            string ret;
            ret = string.Format("\t[DbTable (Name = \"{0}\")]", dbTableName) ;
            return ret;
        }

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
        public static string EndClass()
        {
            return "\t}" ;
        }

        public static string GetDataType(string dataType, bool allowDBNull = false)
        {
            dataType = dataType.Replace("System.", "");
            if (dataType == "Boolean")
            {
                dataType = "bool";
            }
            else if (dataType == "Int32")
                dataType = "long";
            else if (dataType != "DateTime")
                dataType = dataType.ToLower();

            if (allowDBNull && (dataType.ToLower() != "string") && (dataType.ToLower() != "bool"))
                dataType = dataType + "?";

            return dataType;
        }

        public static string AddDbColumnAttribute(bool isIdentity = false, bool isPrimary = false)
        {
            string ret = "";

            ret = "\t\t[DbColumn(";
            ret += isIdentity ? "IsIdentity = true" : "";
            ret += isPrimary ? (isIdentity ? "," : "") + " IsPrimary = true" : "";
            ret += ")]" ;
            return ret;
        }

        public static string AddPublicProperty(string dataType, string propertyName)
        {
            string ret = "";

            ret += string.Format("\t\tpublic {0} {1} ", dataType, propertyName);
            ret += "{ get; set; }";

            return ret;
        }

        public static string TableNameToClassName(string tableName)
        {
            string retorno = "";
            if (tableName.ToLower().EndsWith("as"))
            {
                retorno = tableName.Substring(0, tableName.Length - 1);
            }
            else if (tableName.ToLower().EndsWith("es"))
            {
                retorno = tableName.Substring(0, tableName.Length - 2);
                if (retorno.EndsWith("t"))
                    retorno += "e";
            }
            else if (tableName.ToLower().EndsWith("is"))
            {
                retorno = tableName.Substring(0, tableName.Length - 1);

            }
            else if (tableName.ToLower().EndsWith("os"))
            {
                retorno = tableName.Substring(0, tableName.Length - 1);
            }
            else if (tableName.ToLower().EndsWith("us"))
            {
                retorno = tableName.Substring(0, tableName.Length - 1);
            }
            else
            {
                retorno = tableName;
            }

            retorno = retorno.Substring(0, 1).ToUpper() + retorno.Substring(1);

            return retorno;
        }
    }
}
