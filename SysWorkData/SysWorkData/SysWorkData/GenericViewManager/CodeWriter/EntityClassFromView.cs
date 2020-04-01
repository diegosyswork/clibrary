using System;
using System.Data;
using System.Data.Common;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Syntax;

namespace SysWork.Data.GenericViewManager.CodeWriter
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityClassFromView
    {
        private string _connectionString;
        private EDataBaseEngine _databaseEngine;
        private string _nameSpace;
        private string _dbViewName;
        private string _className;
        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityClassFromView"/> class.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="DbViewName">Name of the database view.</param>
        /// <param name="ClassName">Name of the class.</param>
        /// <param name="NameSpace">The name space.</param>
        public EntityClassFromView(EDataBaseEngine dataBaseEngine, string ConnectionString, string DbViewName, string ClassName, string NameSpace)
        {
            EntityClassFromViewConstructorResolver(dataBaseEngine, ConnectionString, DbViewName, ClassName, NameSpace);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityClassFromView"/> class.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="DbViewName">Name of the database view.</param>
        /// <param name="ClassName">Name of the class.</param>
        /// <param name="NameSpace">The name space.</param>
        public EntityClassFromView(string ConnectionString, string DbViewName, string ClassName, string NameSpace)
        {
            EntityClassFromViewConstructorResolver(EDataBaseEngine.MSSqlServer, ConnectionString, DbViewName, ClassName, NameSpace);
        }

        private void EntityClassFromViewConstructorResolver(EDataBaseEngine dataBaseEngine, string ConnectionString, string DbViewName, string ClassName, string NameSpace)
        {
            _connectionString = ConnectionString;
            _databaseEngine = dataBaseEngine;
            _nameSpace = NameSpace;
            _dbViewName = DbViewName;
            _className = ClassName;
            _syntaxProvider = new SyntaxProvider(dataBaseEngine);
        }

        /// <summary>
        /// Gets the created Entity class.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine is not supported by this method (GetTextClass)</exception>
        public string GetEntityTextClass()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(CodeWriterViewHelper.AddUsing("System"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("System.Collections.Generic"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("System.Linq"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("System.Text"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("SysWork.Data.Common.Attributes"));
            builder.AppendLine(CodeWriterViewHelper.StartNamespace(_nameSpace + ".Entities"));
            builder.AppendLine(CodeWriterViewHelper.AddDbViewAttribute(_dbViewName));
            builder.AppendLine(CodeWriterViewHelper.StartClass(_className));
            builder.AppendLine(AddSummary());


            using (DbConnection dbConnection = StaticDbObjectProvider.GetDbConnection(_databaseEngine, _connectionString))
            {
                dbConnection.Open();

                string dbCommandText = string.Format(_syntaxProvider.GetQuerySelectTop0(_dbViewName));
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = dbCommandText;

                DataTable schema = dbCommand.ExecuteReader(CommandBehavior.KeyInfo).GetSchemaTable();

                foreach (DataRow dataRow in schema.Rows)
                {
                    bool isPrimary = (bool)dataRow["IsKey"];
                    bool isIdentity = false;
                    switch (_databaseEngine)
                    {
                        case EDataBaseEngine.MSSqlServer:
                            isIdentity = (bool)dataRow["IsIdentity"];
                            break;
                        case EDataBaseEngine.SqLite:
                            isIdentity = (bool)dataRow["IsAutoIncrement"];
                            break;
                        case EDataBaseEngine.OleDb:
                            isIdentity = (bool)dataRow["IsAutoIncrement"];
                            break;
                        case EDataBaseEngine.MySql:
                            isIdentity = (bool)dataRow["IsAutoIncrement"];
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("The databaseEngine is not supported by this method (GetTextClass)");
                    }

                    bool allowDbNull = (bool)dataRow["AllowDbNull"];
                    string columnName = dataRow["ColumnName"].ToString();
                    string columnDataType = dataRow["DataType"].ToString();

                    string propertyName = columnName;
                    propertyName = propertyName.Replace(" ", "").Replace("-", "").Replace("/", "_");

                    string dataType = CodeWriterViewHelper.GetDataType(columnDataType, allowDbNull);

                    builder.AppendLine(CodeWriterViewHelper.AddDbColumnAttribute(isIdentity, isPrimary, (propertyName != columnName) ? columnName : null));
                    builder.AppendLine(CodeWriterViewHelper.AddPublicProperty(dataType, propertyName));
                }
            }

            builder.AppendLine(CodeWriterViewHelper.EndClass());
            builder.AppendLine(CodeWriterViewHelper.EndNamespace());

            return builder.ToString();
        }

        private string AddSummary()
        {
            string ret = "\t/// <summary>\r\n";
            ret += "\t /// This class was created automatically with the class EntityClassFromView.\r\n";
            ret += "\t /// Please check the DbTypes and the field names.\r\n";
            ret += "\t /// </summary>\r\n";

            return ret;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetEntityTextClass();
        }

    }
}
