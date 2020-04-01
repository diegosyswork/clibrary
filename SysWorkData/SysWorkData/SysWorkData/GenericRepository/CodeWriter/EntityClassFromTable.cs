using System;
using System.Data;
using System.Data.Common;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Syntax;

namespace SysWork.Data.GenericRepository.CodeWriter
{
    /// <summary>
    /// Creates an Entity Class from a table.
    /// </summary>
    public class EntityClassFromTable
    {
        private string _connectionString;
        private EDataBaseEngine _databaseEngine;
        private string _nameSpace;
        private string _dbTableName;
        private string _className;
        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityClassFromTable"/> class.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="DbTableName">Name of the database table.</param>
        /// <param name="ClassName">Name of the class.</param>
        /// <param name="NameSpace">The name space.</param>
        public EntityClassFromTable(EDataBaseEngine dataBaseEngine, string ConnectionString, string DbTableName, string ClassName, string NameSpace)
        {
            EntityClassFromDbConstructorResolver(dataBaseEngine, ConnectionString, DbTableName, ClassName, NameSpace);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityClassFromTable"/> class.
        /// The databaseEngine is MSSqlServer.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="DbTableName">Name of the database table.</param>
        /// <param name="ClassName">Name of the class.</param>
        /// <param name="NameSpace">The name space.</param>
        
        public EntityClassFromTable(string ConnectionString,string DbTableName,string ClassName,string NameSpace)
        {
            EntityClassFromDbConstructorResolver(EDataBaseEngine.MSSqlServer, ConnectionString, DbTableName, ClassName, NameSpace);
        }
        
        private void EntityClassFromDbConstructorResolver(EDataBaseEngine dataBaseEngine, string ConnectionString, string DbTableName, string ClassName, string NameSpace)
        {
            _connectionString = ConnectionString;
            _databaseEngine = dataBaseEngine;
            _nameSpace = NameSpace;
            _dbTableName = DbTableName;
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
            builder.AppendLine(CodeWriterHelper.AddUsing("System"));
            builder.AppendLine(CodeWriterHelper.AddUsing("System.Collections.Generic"));
            builder.AppendLine(CodeWriterHelper.AddUsing("System.Linq"));
            builder.AppendLine(CodeWriterHelper.AddUsing("System.Text"));
            builder.AppendLine(CodeWriterHelper.AddUsing("SysWork.Data.Common.Attributes"));
            builder.AppendLine(CodeWriterHelper.StartNamespace(_nameSpace + ".Entities"));
            builder.AppendLine(CodeWriterHelper.AddDbTableAttribute(_dbTableName));
            builder.AppendLine(CodeWriterHelper.StartClass(_className));
            builder.AppendLine(AddSummary());


            using (DbConnection dbConnection = StaticDbObjectProvider.GetDbConnection(_databaseEngine,_connectionString))
            {
                dbConnection.Open();

                string dbCommandText = string.Format(_syntaxProvider.GetQuerySelectTop0(_dbTableName));
                var dbCommand = dbConnection.CreateCommand(); 
                dbCommand.CommandText = dbCommandText;

                DataTable schema = dbCommand.ExecuteReader(CommandBehavior.KeyInfo).GetSchemaTable();

                foreach (DataRow dataRow in schema.Rows)
                {
                    bool isPrimary = (bool)dataRow["IsKey"];
                    bool isIdentity=false;
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

                    string dataType = CodeWriterHelper.GetDataType(columnDataType, allowDbNull);

                    builder.AppendLine(CodeWriterHelper.AddDbColumnAttribute(isIdentity, isPrimary,(propertyName != columnName) ? columnName : null));
                    builder.AppendLine(CodeWriterHelper.AddPublicProperty(dataType, propertyName));
                }
            }

            builder.AppendLine(CodeWriterHelper.EndClass());
            builder.AppendLine(CodeWriterHelper.EndNamespace());

            return builder.ToString();
        }

        private string AddSummary()
        {
            string ret = "\t/// <summary>\r\n";
            ret += "\t /// This class was created automatically with the class EntityClassFromDb.\r\n";
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