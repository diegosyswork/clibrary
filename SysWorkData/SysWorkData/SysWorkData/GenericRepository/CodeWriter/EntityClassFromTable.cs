using System;
using System.Data;
using System.Data.Common;
using System.Text;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Syntax;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.GenericRepository.CodeWriter
{
    /// <summary>
    /// Creates an Entity Class from a table.
    /// </summary>
    public class EntityClassFromTable
    {
        private string _connectionString;
        private EDatabaseEngine _databaseEngine;
        private string _nameSpace;
        private string _dbTableName;
        private string _className;
        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityClassFromTable"/> class.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="DbTableName">Name of the database table.</param>
        /// <param name="ClassName">Name of the class.</param>
        /// <param name="NameSpace">The name space.</param>
        public EntityClassFromTable(EDatabaseEngine databaseEngine, string ConnectionString, string DbTableName, string ClassName, string NameSpace)
        {
            EntityClassFromDbConstructorResolver(databaseEngine, ConnectionString, DbTableName, ClassName, NameSpace);
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
            EntityClassFromDbConstructorResolver(DefaultValues.DefaultDatabaseEngine, ConnectionString, DbTableName, ClassName, NameSpace);
        }
        
        private void EntityClassFromDbConstructorResolver(EDatabaseEngine databaseEngine, string ConnectionString, string DbTableName, string ClassName, string NameSpace)
        {
            _connectionString = ConnectionString;
            _databaseEngine = databaseEngine;
            _nameSpace = NameSpace;
            _dbTableName = DbTableName;
            _className = ClassName;
            _syntaxProvider = new SyntaxProvider(databaseEngine);
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
            builder.AppendLine(CodeWriterHelper.AddUsing("SysWork.Data.Mapping"));
            builder.AppendLine(CodeWriterHelper.StartNamespace(_nameSpace + ".Entities"));
            builder.AppendLine(CodeWriterHelper.AddTableAttribute(_dbTableName));
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
                        case EDatabaseEngine.MSSqlServer:
                            isIdentity = (bool)dataRow["IsIdentity"];
                            break;
                        case EDatabaseEngine.SqLite:
                            isIdentity = (bool)dataRow["IsAutoIncrement"];
                            break;
                        case EDatabaseEngine.OleDb:
                            isIdentity = (bool)dataRow["IsAutoIncrement"];
                            break;
                        case EDatabaseEngine.MySql:
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

                    builder.AppendLine(CodeWriterHelper.AddColumnAttribute(isIdentity, isPrimary,(propertyName != columnName) ? columnName : null));
                    builder.AppendLine(CodeWriterHelper.AddPublicProperty(dataType, propertyName));
                }
            }

            builder.AppendLine(CodeWriterHelper.EndClass());
            builder.AppendLine(CodeWriterHelper.EndNamespace());

            return builder.ToString();
        }

        private string AddSummary()
        {
            string ret = "\t\t/// <summary>\r\n";
            ret += "\t\t/// This class was created automatically with SysWork.EntityManager.\r\n";
            ret += "\t\t/// Please check the DbTypes and the field names.\r\n";
            ret += "\t\t/// </summary>\r\n";

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