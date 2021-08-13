using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericDataManager.CodeWriter.Properties;

namespace SysWork.Data.GenericDataManager.CodeWriter
{
    /// <summary>
    /// Write a DataManager Class
    /// </summary>
    public class DataManagerInstantiableClassWriter
    {
        private string _connectionString;
        public string ConnectionString { get { return _connectionString; } }
        
        private EDatabaseEngine _databaseEngine;
        public EDatabaseEngine DatabaseEngine { get { return _databaseEngine; } }

        private string _nameSpace;
        private List<DbObjectWriterProperty> _repositories;
        private List<DbObjectWriterProperty> _viewManagers;

        private bool _useLazyLoad;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManagerClassWriter"/> class. 
        /// The databaseEngine is MSSqlServer
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="Repositories"></param>
        /// <param name="ViewManagers"></param>
        public DataManagerInstantiableClassWriter(string ConnectionString, string NameSpace, List<DbObjectWriterProperty> Repositories,List<DbObjectWriterProperty> ViewManagers, bool useLazyLoad)
        {
            DataManagerInstantiableClassWriterConstructorResolver(DefaultValues.DefaultDatabaseEngine, ConnectionString, NameSpace, Repositories, ViewManagers, useLazyLoad);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManagerClassWriter"/> class.
        /// </summary>
        /// <param name="databaseEngine">The database engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="Repositories"></param>
        /// <param name="ViewManagers"></param>
        public DataManagerInstantiableClassWriter(EDatabaseEngine databaseEngine, string connectionString, string NameSpace, List<DbObjectWriterProperty> Repositories, List<DbObjectWriterProperty> ViewManagers, bool useLazyLoad)
        {
            DataManagerInstantiableClassWriterConstructorResolver(databaseEngine, connectionString, NameSpace, Repositories, ViewManagers, useLazyLoad);
        }

        private void DataManagerInstantiableClassWriterConstructorResolver(EDatabaseEngine databaseEngine, string connectionString, string NameSpace, List<DbObjectWriterProperty> Repositories, List<DbObjectWriterProperty> ViewManagers, bool useLazyLoad)
        {
            _connectionString = connectionString;
            _databaseEngine = databaseEngine;
            _nameSpace = NameSpace;
            _repositories = Repositories;
            _viewManagers = ViewManagers;
            _useLazyLoad = useLazyLoad;
        }

        private string GetPrivateRepositoryVariableName(string repositoryName)
        {
            return "_" + repositoryName[0].ToString().ToLower() + repositoryName.Substring(1);
        }
        private string GetPrivateViewManagerVariableName(string viewManagerName)
        {
            return "_" + viewManagerName[0].ToString().ToLower() + viewManagerName.Substring(1);
        }

        private string GetTextClass()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(DataManagerCodeWriterHelper.AddUsing("System"));
            builder.AppendLine(DataManagerCodeWriterHelper.AddUsing("SysWork.Data.GenericDataManager"));
            builder.AppendLine(DataManagerCodeWriterHelper.AddUsing("SysWork.Data.Common.ValueObjects"));
            
            if (_repositories!=null && _repositories.Count>0)
                builder.AppendLine(DataManagerCodeWriterHelper.AddUsing(_nameSpace + ".Repositories"));

            if (_viewManagers != null && _viewManagers.Count > 0)
                builder.AppendLine(DataManagerCodeWriterHelper.AddUsing(_nameSpace + ".ViewManagers"));

            builder.AppendLine(DataManagerCodeWriterHelper.StartNamespace(_nameSpace));
            builder.AppendLine(AddSummary());
            builder.AppendLine(DataManagerCodeWriterHelper.StartClass("DataManager", "BaseInstantiableDataManager"));

            builder.AppendLine(AddRepositories());
            builder.AppendLine(AddViewManagers());
            builder.AppendLine(AddPublicConstructors());
            
            builder.AppendLine(AddInitDataObjects());
            builder.AppendLine(DataManagerCodeWriterHelper.EndClass());
            builder.AppendLine(DataManagerCodeWriterHelper.EndNamespace());

            return builder.ToString();
        }
        private string AddRepositories()
        {
            string ret = "";
            if (_repositories != null && _repositories.Count > 0)
            {
                _repositories = _repositories.OrderBy(r => r.PublicPropertyName).ToList();

                ret += "\t\t//Repositories" + Environment.NewLine;
                foreach (var repository in _repositories)
                    ret += AddRepository(repository);
            }

            return ret;
        }
        private string AddViewManagers()
        {
            string ret = "";
            if (_viewManagers != null && _viewManagers.Count > 0)
            {
                _viewManagers = _viewManagers.OrderBy(r => r.PublicPropertyName).ToList();

                ret += "\t\t//ViewManagers" + Environment.NewLine;
                foreach (var viewManager in _viewManagers)
                    ret += AddViewManager(viewManager);
            }

            return ret;
        }

        private string AddRepository(DbObjectWriterProperty repository)
        {
            string ret = "";
            var repositoryVariable = GetPrivateRepositoryVariableName(repository.ObjectName);

            if (_useLazyLoad)
            {
                ret += $"\t\tprivate Lazy<{repository.ObjectName}> {repositoryVariable};" + Environment.NewLine;
                ret += $"\t\tpublic {repository.ObjectName} {repository.PublicPropertyName} {{get => {repositoryVariable}.Value;}}" + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                ret += $"\t\tprivate {repository.ObjectName} {repositoryVariable};" + Environment.NewLine;
                ret += $"\t\tpublic {repository.ObjectName} {repository.PublicPropertyName} {{get => {repositoryVariable};}}" + Environment.NewLine + Environment.NewLine;
            }

            return ret;
        }

        private string AddViewManager(DbObjectWriterProperty viewManager)
        {
            string ret = "";
            var viewManagerVariable = GetPrivateViewManagerVariableName(viewManager.ObjectName);

            if (_useLazyLoad)
            {
                ret += $"\t\tprivate Lazy<{viewManager.ObjectName}> {viewManagerVariable};" + Environment.NewLine;
                ret += $"\t\tpublic {viewManager.ObjectName} {viewManager.PublicPropertyName} {{get => {viewManagerVariable}.Value;}}" + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                ret += $"\t\tprivate {viewManager.ObjectName} {viewManagerVariable};" + Environment.NewLine;
                ret += $"\t\tpublic {viewManager.ObjectName} {viewManager.PublicPropertyName} {{get => {viewManagerVariable};}}" + Environment.NewLine + Environment.NewLine;
            }

            return ret;
        }

        private string AddInitDataObjects()
        {
            string ret = "";

            ret += "\t\tprivate void InitDataObjects()" + Environment.NewLine;
            ret += "\t\t{" + Environment.NewLine;


            if (_repositories != null && _repositories.Count > 0)
            {
                ret += "\t\t\t//Repositories" + Environment.NewLine;
                foreach (var repository in _repositories)
                    ret += AddRepositoryInstance(repository.ObjectName);
            }

            if (_viewManagers != null && _viewManagers.Count > 0)
            {
                ret += "\r\n\t\t\t//ViewManagers" + Environment.NewLine;
                foreach (var viewManager in _viewManagers)
                    ret += AddViewManagerInstance(viewManager.ObjectName);
            }

            ret += "\t\t}" + Environment.NewLine;

            return ret;
        }


        private string AddRepositoryInstance(string repositoryName)
        {
            string ret = "";
            if (_useLazyLoad)
                ret += $"\t\t\t{GetPrivateRepositoryVariableName(repositoryName)}  = new Lazy<{repositoryName}>(()=>new {repositoryName}(ConnectionString,DatabaseEngine));" + Environment.NewLine;
            else 
                ret += $"\t\t\t{GetPrivateRepositoryVariableName(repositoryName)}  = new {repositoryName} (ConnectionString,DatabaseEngine);" + Environment.NewLine;
            
            return ret;
        }
        private string AddViewManagerInstance(string viewManagerName)
        {
            string ret = "";
            if (_useLazyLoad)
                ret += $"\t\t\t{GetPrivateViewManagerVariableName(viewManagerName)}  = new Lazy<{viewManagerName}>(()=>new {viewManagerName}(ConnectionString,DatabaseEngine));" + Environment.NewLine;
            else
                ret += $"\t\t\t{GetPrivateViewManagerVariableName(viewManagerName)}  = new {viewManagerName} (ConnectionString,DatabaseEngine);" + Environment.NewLine;

            return ret;
        }

        private string AddPublicConstructors()
        {
            string ret = "";
                
            ret += "\t\tpublic DataManager(EDatabaseEngine databaseEngine, string connectionString) : base(databaseEngine, connectionString)" + Environment.NewLine;
            ret += "\t\t{" + Environment.NewLine;
            ret += "\t\t\t"  + "InitDataObjects();";
            ret += "\t\t}" + Environment.NewLine;
            ret += "\t\tpublic DataManager(string connectionString) : base(DefaultValues.DefaultDatabaseEngine, connectionString)" + Environment.NewLine;
            ret += "\t\t{" + Environment.NewLine;
            ret += "\t\t\t" + "InitDataObjects();";
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
