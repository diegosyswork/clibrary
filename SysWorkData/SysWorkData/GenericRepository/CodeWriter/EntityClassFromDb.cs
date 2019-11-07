using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.GenericRepostory.CodeWriter
{
    /// <summary>
    /// 
    /// Fecha: 29/07/2018
    /// Author: Diego Martinez
    /// 
    /// Dada una Cadena de conexion y una Tabla, crea una entidad para representarla
    /// 
    /// </summary>
    public class EntityClassFromDb
    {
        private string _connectionString;
        private string _nameSpace;
        private string _dbTableName;
        private string _className;

        private string _dataSource;
        private string _initialCatalog;

        public EntityClassFromDb(string ConnectionString,string DbTableName,string ClassName,string NameSpace)
        {
            _connectionString = ConnectionString;
            _nameSpace = NameSpace;
            _dbTableName = DbTableName;
            _className = ClassName;

            SqlConnectionStringBuilder s = new SqlConnectionStringBuilder(_connectionString);
            _dataSource = s.DataSource;
            _initialCatalog = s.InitialCatalog;
        }

        private string GetTextClass()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(CodeWriterUtil.AddUsing("System"));
            builder.AppendLine(CodeWriterUtil.AddUsing("System.Collections.Generic"));
            builder.AppendLine(CodeWriterUtil.AddUsing("System.Linq"));
            builder.AppendLine(CodeWriterUtil.AddUsing("System.Text"));
            builder.AppendLine(CodeWriterUtil.AddUsing("System.Threading.Tasks"));
            builder.AppendLine(CodeWriterUtil.AddUsing("SysWork.Data.GenericRepostory.Attributes"));
            builder.AppendLine(CodeWriterUtil.StartNamespace(_nameSpace + ".Entities"));
            builder.AppendLine(CodeWriterUtil.AddDbTableAttribute(_dbTableName));
            builder.AppendLine(CodeWriterUtil.StartClass(_className));
            builder.AppendLine(AddSummary());


            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                string sql = string.Format("SELECT TOP 0 * FROM [{0}]", _dbTableName);
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);

                DataTable schema = sqlCommand.ExecuteReader(CommandBehavior.KeyInfo).GetSchemaTable();

                foreach (DataRow dataRow in schema.Rows)
                {

                    bool isPrimary = (bool) dataRow["IsKey"];
                    bool isIdentity = (bool) dataRow["IsIdentity"];
                    bool allowDbNull = (bool) dataRow["AllowDbNull"];
                    string columnName = dataRow["ColumnName"].ToString();
                    string columnDataType = dataRow["DataType"].ToString();

                    string dataType = CodeWriterUtil.GetDataType(columnDataType,allowDbNull);

                    builder.AppendLine(CodeWriterUtil.AddDbColumnAttribute(isIdentity, isPrimary));
                    builder.AppendLine(CodeWriterUtil.AddPublicProperty(dataType, columnName));
                }
            }


            builder.AppendLine(CodeWriterUtil.EndClass());
            builder.AppendLine(CodeWriterUtil.EndNamespace());


            return builder.ToString();
        }

        private string AddSummary()
        {
            string ret = "\t\t/// <summary>" + Environment.NewLine;
            ret += "\t\t/// **********************************************************************" + Environment.NewLine;
            ret += "\t\t/// " + Environment.NewLine;
            ret += "\t\t/// Esta clase fue generada automaticamente por la clase EntityClassFromDb" + Environment.NewLine;
            ret += "\t\t/// " + Environment.NewLine;
            ret += string.Format("\t\t/// Fecha: {0}", string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now)) + Environment.NewLine;
            ret += "\t\t/// " + Environment.NewLine;
            ret += string.Format("\t\t/// DataSource: {0}", _dataSource) + Environment.NewLine;
            ret += string.Format("\t\t/// InitialCatalog: {0}", _initialCatalog) + Environment.NewLine;
            ret += "\t\t/// " + Environment.NewLine;
            ret += "\t\t/// **********************************************************************" + Environment.NewLine;
            ret += "\t\t/// </summary>" + Environment.NewLine;

            return ret;
        }
        public override string ToString()
        {
            return GetTextClass();
        }

    }
}



