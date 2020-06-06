using System;
using System.Text;
using SysWork.Data.Common.Syntax;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.GenericViewManager.CodeWriter
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewManagerClassFromView
    {
        private string _connectionString;
        private EDataBaseEngine _databaseEngine;

        private string _nameSpace;
        private string _entityName;
        private string _className;

        private string _dbViewName;
        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewManagerClassFromView"/> class.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="EntityName">Name of the entity.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="DbViewName">Name of the database view.</param>
        public ViewManagerClassFromView(string ConnectionString, string EntityName, string NameSpace, string DbViewName)
        {
            ViewManagerClassFromViewConstructorResolver(EDataBaseEngine.MSSqlServer, ConnectionString, EntityName, NameSpace, DbViewName);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewManagerClassFromView"/> class.
        /// </summary>
        /// <param name="databaseEngine">The database engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="EntityName">Name of the entity.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="DbTableName">Name of the database table.</param>
        public ViewManagerClassFromView(EDataBaseEngine databaseEngine, string ConnectionString, string EntityName, string NameSpace, string DbTableName)
        {
            ViewManagerClassFromViewConstructorResolver(databaseEngine, ConnectionString, EntityName, NameSpace, DbTableName);
        }

        private void ViewManagerClassFromViewConstructorResolver(EDataBaseEngine databaseEngine, string ConnectionString, string EntityName, string NameSpace, string DbTableName)
        {
            _connectionString = ConnectionString;
            _databaseEngine = databaseEngine;
            _nameSpace = NameSpace;
            _entityName = EntityName;
            _className = EntityName + "ViewManager";
            _dbViewName = DbTableName;
            _syntaxProvider = new SyntaxProvider(_databaseEngine);
        }

        private string GetTextClass()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(CodeWriterViewHelper.AddUsing("System"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("System.Collections.Generic"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("System.Linq"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("System.Text"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("SysWork.Data.Common"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("SysWork.Data.GenericViewManager"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing("SysWork.Data.Common.Attributes"));
            builder.AppendLine(CodeWriterViewHelper.AddUsing(_nameSpace + ".Entities"));
            builder.AppendLine(CodeWriterViewHelper.StartNamespace(_nameSpace + ".ViewManagers"));
            builder.AppendLine(AddSummary());
            builder.AppendLine(CodeWriterViewHelper.StartClass(_className, string.Format("BaseViewManager<{0}>", _entityName)));
            builder.AppendLine(AddConstructor());
            builder.AppendLine(CodeWriterViewHelper.EndClass());
            builder.AppendLine(CodeWriterViewHelper.EndNamespace());

            return builder.ToString();
        }


        private string AddConstructor()
        {
            string ret = "";

            ret += "\t\tpublic " + _entityName + "BaseViewManager (string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString,dataBaseEngine)" + Environment.NewLine;
            ret += "\t\t{" + Environment.NewLine;
            ret += "\t\t" + Environment.NewLine;
            ret += "\t\t}" + Environment.NewLine;

            return ret;
        }

        private string AddSummary()
        {

            string ret = "\t/// <summary>\r\n";
            ret += "\t/// This class was created automatically with the BaseViewManagerClassFromView class.\r\n";
            ret += "\t/// Inherited from GenericRepository which allows you to perform the following actions: \r\n";
            ret += "\t/// GetById, \r\n";
            ret += "\t/// GetByLambdaExpressionFilter, \r\n";
            ret += "\t/// GetByGenericWhereFilter, \r\n";
            ret += "\t/// GetAll, \r\n";
            ret += "\t/// GetListByLambdaExpressionFilter, \r\n";
            ret += "\t/// GetDataTableByLambdaExpressionFilter, \r\n";
            ret += "\t/// GetListByGenericWhereFilter, \r\n";
            ret += "\t/// GetDataTableByGenericWhereFilter, \r\n";
            ret += "\t/// Find \r\n";
            ret += "\t/// </summary>\r\n";

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
            return GetTextClass();
        }

    }
}
