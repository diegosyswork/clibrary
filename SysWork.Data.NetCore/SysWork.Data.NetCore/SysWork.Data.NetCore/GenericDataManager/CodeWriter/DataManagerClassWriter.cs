using System;
using System.Collections.Generic;
using System.Text;
using SysWork.Data.NetCore.Common.Syntax;
using SysWork.Data.NetCore.Common.ValueObjects;

namespace SysWork.Data.NetCore.GenericDataManager.CodeWriter
{
    /// <summary>
    /// Write a DataManager Class
    /// </summary>
    public class DataManagerClassWriter
    {
        private string _connectionString;
        private EDataBaseEngine _databaseEngine;

        private string _nameSpace;
        private List<string> _repositories;
        private List<string> _viewManagers;

        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManagerClassWriter"/> class. 
        /// The databaseEngine is MSSqlServer
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="Repositories"></param>
        /// <param name="ViewManagers"></param>
        public DataManagerClassWriter(string ConnectionString, string NameSpace, List<string> Repositories,List<string> ViewManagers)
        {
            DataManagerClassWriterConstructorResolver(EDataBaseEngine.MSSqlServer, ConnectionString, NameSpace, Repositories, ViewManagers);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManagerClassWriter"/> class.
        /// </summary>
        /// <param name="databaseEngine">The database engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="Repositories"></param>
        /// <param name="ViewManagers"></param>
        public DataManagerClassWriter(EDataBaseEngine databaseEngine, string ConnectionString, string NameSpace, List<string> Repositories, List<string> ViewManagers)
        {
            DataManagerClassWriterConstructorResolver(databaseEngine, ConnectionString, NameSpace, Repositories, ViewManagers);
        }

        private void DataManagerClassWriterConstructorResolver(EDataBaseEngine databaseEngine, string ConnectionString, string NameSpace, List<string> Repositories, List<string> ViewManagers)
        {
            _connectionString = ConnectionString;
            _databaseEngine = databaseEngine;
            _nameSpace = NameSpace;
            _repositories = Repositories;
            _viewManagers = ViewManagers;
            _syntaxProvider = new SyntaxProvider(_databaseEngine);
        }

        private string GetRepositoryVariable(string repositoryName)
        {
            return "_" + repositoryName[0].ToString().ToLower() + repositoryName.Substring(1);
        }
        private string GetViewManagerVariable(string viewManagerName)
        {
            return "_" + viewManagerName[0].ToString().ToLower() + viewManagerName.Substring(1);
        }

        private string GetTextClass()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(DataManagerCodeWriterHelper.AddUsing("System"));
            builder.AppendLine(DataManagerCodeWriterHelper.AddUsing("SysWork.Data.NetCore.GenericDataManager"));
            builder.AppendLine(DataManagerCodeWriterHelper.AddUsing("SysWork.Data.NetCore.GenericDataManager.Intefaces"));

            if(_repositories!=null && _repositories.Count>0)
                builder.AppendLine(DataManagerCodeWriterHelper.AddUsing(_nameSpace + ".Repositories"));

            if (_viewManagers != null && _viewManagers.Count > 0)
                builder.AppendLine(DataManagerCodeWriterHelper.AddUsing(_nameSpace + ".ViewManagers"));

            builder.AppendLine(DataManagerCodeWriterHelper.StartNamespace(_nameSpace));
            builder.AppendLine(AddSummary());
            builder.AppendLine(DataManagerCodeWriterHelper.StartClass("DataManager", "BaseDataManager<DataManager>, IDataManager"));
            builder.AppendLine(AddRepositories());
            builder.AppendLine(AddViewManagers());
            builder.AppendLine(AddPrivateConstructor());
            builder.AppendLine(AddInitDataObjects());
            builder.AppendLine(DataManagerCodeWriterHelper.EndClass());
            builder.AppendLine(DataManagerCodeWriterHelper.EndNamespace());

            return builder.ToString();
        }
        private string AddRepositories()
        {
            string ret = "";
            if (_repositories != null)
            {
                ret += "\t\t//Repositories" + Environment.NewLine;
                foreach (var repository in _repositories)
                    ret += AddRepository(repository);
            }

            return ret;
        }
        private string AddViewManagers()
        {
            string ret = "";
            if (_viewManagers != null)
            {
                ret += "\t\t//ViewManagers" + Environment.NewLine;
                foreach (var viewManager in _viewManagers)
                    ret += AddViewManager(viewManager);
            }

            return ret;
        }

        private string AddRepository(string repositoryName)
        {
            string ret = "";

            //Ver. 1.0   ret += $"\t\tpublic {repositoryName} {repositoryName} {{get; private set;}}" + Environment.NewLine;

            ret += $"\t\tprivate Lazy<{repositoryName}> {GetRepositoryVariable(repositoryName)};" + Environment.NewLine;
            ret += $"\t\tpublic {repositoryName} {repositoryName} {{get => {GetRepositoryVariable(repositoryName)}.Value;}}" + Environment.NewLine + Environment.NewLine;

            return ret;
        }

        private string AddViewManager(string viewManagerName)
        {
            string ret = "";

            //Ver 1.0 ret += $"\t\tpublic {viewManager} {viewManager} {{get; private set;}}" + Environment.NewLine;
            ret += $"\t\tprivate Lazy<{viewManagerName}> {GetViewManagerVariable(viewManagerName)};" + Environment.NewLine;
            ret += $"\t\tpublic {viewManagerName} {viewManagerName} {{get => {GetViewManagerVariable(viewManagerName)}.Value;}}" + Environment.NewLine;
            return ret;
        }

        private string AddInitDataObjects()
        {
            string ret = "";

            ret += "\t\tpublic void InitDataObjects()" + Environment.NewLine;
            ret += "\t\t{" + Environment.NewLine;


            if (_repositories != null)
            {
                ret += "\t\t\t//Repositories" + Environment.NewLine;
                foreach (var repository in _repositories)
                    ret += AddRepositoryInstance(repository);
            }


            if (_viewManagers != null)
            {
                ret += "\r\n\t\t\t//ViewManagers" + Environment.NewLine;
                foreach (var viewManager in _viewManagers)
                    ret += AddViewManagerInstance(viewManager);
            }

            ret += "\t\t}" + Environment.NewLine;

            return ret;
        }


        private string AddRepositoryInstance(string repositoryName)
        {
            string ret = "";

            //Ver 1.0 ret += $"\t\t\t{repositoryName}  = new {repositoryName}(ConnectionString,DataBaseEngine);" + Environment.NewLine;
            ret += $"\t\t\t{GetRepositoryVariable(repositoryName)}  = new Lazy<{repositoryName}>(()=>new {repositoryName}(ConnectionString,DataBaseEngine));" + Environment.NewLine;
            return ret;
        }
        private string AddViewManagerInstance(string viewManagerName)
        {
            string ret = "";

            //Ver 1.0 ret += $"\t\t\t{viewManagerName}  = new {viewManagerName}(ConnectionString,DataBaseEngine);" + Environment.NewLine;
            ret += $"\t\t\t{GetViewManagerVariable(viewManagerName)}  = new Lazy<{viewManagerName}>(()=>new {viewManagerName}(ConnectionString,DataBaseEngine));" + Environment.NewLine;

            return ret;
        }

        private string AddPrivateConstructor()
        {
            string ret = "";

            ret += "\t\tprivate DataManager()" + Environment.NewLine;
            ret += "\t\t{" + Environment.NewLine;
            ret += "\t\t" + Environment.NewLine;
            ret += "\t\t}" + Environment.NewLine;

            return ret;
        }

        private string AddSummary()
        {

            string ret = "\t/// <summary>\r\n";
            ret += "\t/// This class was created automatically with the DataManagerClassWriter.\r\n";
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
