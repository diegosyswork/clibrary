using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.NetCore.Common;
using SysWork.Data.NetCore.Common.DataObjectProvider;
using SysWork.Data.NetCore.Common.Dictionaries;
using SysWork.Data.NetCore.Common.Syntax;
using SysWork.Data.NetCore.Common.ValueObjects;

namespace SysWork.Data.NetCore.GenericViewManager.CodeWriter
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
            builder.AppendLine(CodeWriterViewHelper.AddUsing("SysWork.Data.NetCore.Common.Attributes"));
            builder.AppendLine(CodeWriterViewHelper.StartNamespace(_nameSpace + ".Entities"));
            builder.AppendLine(CodeWriterViewHelper.AddDbViewAttribute(_dbViewName));
            builder.AppendLine(CodeWriterViewHelper.StartClass(_className));
            builder.AppendLine(AddSummary());

            DataTable columns;
            using (DbConnection dbConnection = StaticDbObjectProvider.GetDbConnection(_databaseEngine, _connectionString))
            {
                dbConnection.Open();

                if (_databaseEngine == EDataBaseEngine.OleDb || _databaseEngine == EDataBaseEngine.MySql)
                    columns = dbConnection.GetSchema("Columns", new[] { null, null, _dbViewName, null });
                else
                    columns = dbConnection.GetSchema("Columns", new[] { dbConnection.Database, null, _dbViewName, null });

                columns.DefaultView.Sort = "ORDINAL_POSITION ASC";
                columns = columns.DefaultView.ToTable();

                foreach (DataRow dataRow in columns.Rows)
                {
                    string columnName = dataRow["COLUMN_NAME"].ToString();
                    string propertyName = columnName;
                    propertyName = propertyName.Replace(" ", "").Replace("-", "").Replace("/", "_");

                    var dbType = dataRow["DATA_TYPE"];
                    var nullable = dataRow["IS_NULLABLE"].ToString().ToLower();
                    var isNullable = (!nullable.Equals("no")) && (!nullable.Equals("false"));

                    var dataType = "";
                    if (_databaseEngine== EDataBaseEngine.OleDb)
                    {
                        if (DbTypeDictionary.DbColumnTypeToDbTypeEnum.TryGetValue(((OleDbType)dbType).ToString(), out DbType dBTypeValue))
                            dataType = dBTypeValue.ToString();
                        else
                            throw new ArgumentOutOfRangeException("The datatype is not recognited. See the DbColumnTypeToDbTypeEnum Dictionary");
                    }
                    else
                    {
                        if (DbTypeDictionary.DbColumnTypeToDbTypeEnum.TryGetValue(dbType.ToString(), out DbType dBTypeValue))
                            dataType = dBTypeValue.ToString();
                        else
                            throw new ArgumentOutOfRangeException("The datatype is not recognited. See the DbColumnTypeToDbTypeEnum Dictionary");
                    }

                    dataType = CodeWriterViewHelper.GetDataType(dataType, isNullable);

                    builder.AppendLine(CodeWriterViewHelper.AddDbColumnAttribute((propertyName != columnName) ? columnName : null));
                    builder.AppendLine(CodeWriterViewHelper.AddPublicProperty(dataType, propertyName));
                }

                dbConnection.Close();

            }
            builder.AppendLine(CodeWriterViewHelper.EndClass());
            builder.AppendLine(CodeWriterViewHelper.EndNamespace());

            return builder.ToString();
        }

        private string AddSummary()
        {
            string ret = "\t\t/// <summary>\r\n";
            ret += "\t\t/// This class was created automatically with the class EntityClassFromView.\r\n";
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
