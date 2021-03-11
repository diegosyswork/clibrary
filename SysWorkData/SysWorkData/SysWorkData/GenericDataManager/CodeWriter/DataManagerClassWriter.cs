using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SysWork.Data.Common.ValueObjects;
using SysWork.Data.GenericDataManager.CodeWriter.Properties;

namespace SysWork.Data.GenericDataManager.CodeWriter
{
    // TODO:  Agregar la  posibilidad de crear: Clasico Singleton, DbContext Singleton, Clasico Instanciable, DbContext Instanciable, agregado el 08/03/20021
    public enum EDatamagerStyle
    {
        Classic,
        DbContext
    }

    /// <summary>
    /// Write a DataManager Class
    /// </summary>
    public class DataManagerClassWriter
    {
        private string _connectionString;
        private EDatabaseEngine _databaseEngine;

        private EDatamagerStyle _datamagerStyle;

        private string _nameSpace;
        private List<DbObjectWriterProperty> _repositories;
        private List<DbObjectWriterProperty> _viewManagers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManagerClassWriter"/> class. 
        /// The databaseEngine is MSSqlServer
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="Repositories"></param>
        /// <param name="ViewManagers"></param>
        public DataManagerClassWriter(string ConnectionString, string NameSpace, List<DbObjectWriterProperty> Repositories,List<DbObjectWriterProperty> ViewManagers, EDatamagerStyle datamagerStyle)
        {
            DataManagerClassWriterConstructorResolver(EDatabaseEngine.MSSqlServer, ConnectionString, NameSpace, Repositories, ViewManagers, datamagerStyle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManagerClassWriter"/> class.
        /// </summary>
        /// <param name="databaseEngine">The database engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="NameSpace">The name space.</param>
        /// <param name="Repositories"></param>
        /// <param name="ViewManagers"></param>
        public DataManagerClassWriter(EDatabaseEngine databaseEngine, string ConnectionString, string NameSpace, List<DbObjectWriterProperty> Repositories, List<DbObjectWriterProperty> ViewManagers, EDatamagerStyle datamagerStyle)
        {
            DataManagerClassWriterConstructorResolver(databaseEngine, ConnectionString, NameSpace, Repositories, ViewManagers, datamagerStyle);
        }

        private void DataManagerClassWriterConstructorResolver(EDatabaseEngine databaseEngine, string ConnectionString, string NameSpace, List<DbObjectWriterProperty> Repositories, List<DbObjectWriterProperty> ViewManagers, EDatamagerStyle datamagerStyle)
        {
            _connectionString = ConnectionString;
            _databaseEngine = databaseEngine;
            _datamagerStyle = datamagerStyle;
            _nameSpace = NameSpace;
            _repositories = Repositories;
            _viewManagers = ViewManagers;
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
            builder.AppendLine(DataManagerCodeWriterHelper.AddUsing("SysWork.Data.GenericDataManager"));
            builder.AppendLine(DataManagerCodeWriterHelper.AddUsing("SysWork.Data.GenericDataManager.Intefaces"));

            if(_repositories!=null && _repositories.Count>0)
                builder.AppendLine(DataManagerCodeWriterHelper.AddUsing(_nameSpace + ".Repositories"));

            if (_viewManagers != null && _viewManagers.Count > 0)
                builder.AppendLine(DataManagerCodeWriterHelper.AddUsing(_nameSpace + ".ViewManagers"));

            builder.AppendLine(DataManagerCodeWriterHelper.StartNamespace(_nameSpace));
            builder.AppendLine(AddSummary());
            if (_datamagerStyle == EDatamagerStyle.Classic)
                builder.AppendLine(DataManagerCodeWriterHelper.StartClass("DataManager", "BaseDataManager<DataManager>, IDataManager"));
            else
                builder.AppendLine(DataManagerCodeWriterHelper.StartClass("Db", "BaseDataManager<Db>, IDataManager"));

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
            if (_viewManagers != null)
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
            //Ver. 1.0   ret += $"\t\tpublic {repositoryName} {repositoryName} {{get; private set;}}" + Environment.NewLine;
            var repositoryVariable = GetRepositoryVariable(repository.ObjectName);
            ret += $"\t\tprivate Lazy<{repository.ObjectName}> {repositoryVariable};" + Environment.NewLine;

            if (_datamagerStyle == EDatamagerStyle.Classic)
                ret += $"\t\tpublic {repository.ObjectName} {repository.PublicPropertyName} {{get => {repositoryVariable}.Value;}}" + Environment.NewLine + Environment.NewLine;
            else
                ret += $"\t\tpublic static {repository.ObjectName} {repository.PublicPropertyName} {{get => GetInstance().{repositoryVariable}.Value;}}" + Environment.NewLine + Environment.NewLine;

            return ret;
        }

        private string AddViewManager(DbObjectWriterProperty viewManager)
        {
            string ret = "";

            //Ver 1.0 ret += $"\t\tpublic {viewManager} {viewManager} {{get; private set;}}" + Environment.NewLine;
            var viewManagerVariable = GetRepositoryVariable(viewManager.ObjectName);

            ret += $"\t\tprivate Lazy<{viewManager.ObjectName}> {viewManagerVariable};" + Environment.NewLine;
            if (_datamagerStyle == EDatamagerStyle.Classic)
                ret += $"\t\tpublic {viewManager} {viewManager} {{get => GetInstance().{viewManagerVariable}.Value;}}" + Environment.NewLine;
            else
                ret += $"\t\tpublic static {viewManager.ObjectName} {viewManager.PublicPropertyName} {{get => GetInstance().{viewManagerVariable}.Value;}}" + Environment.NewLine + Environment.NewLine;

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
                    ret += AddRepositoryInstance(repository.ObjectName);
            }

            if (_viewManagers != null)
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

            //Ver 1.0 ret += $"\t\t\t{repositoryName}  = new {repositoryName}(ConnectionString,DatabaseEngine);" + Environment.NewLine;
            ret += $"\t\t\t{GetRepositoryVariable(repositoryName)}  = new Lazy<{repositoryName}>(()=>new {repositoryName}(ConnectionString,DatabaseEngine));" + Environment.NewLine;
            return ret;
        }
        private string AddViewManagerInstance(string viewManagerName)
        {
            string ret = "";

            //Ver 1.0 ret += $"\t\t\t{viewManagerName}  = new {viewManagerName}(ConnectionString,DatabaseEngine);" + Environment.NewLine;
            ret += $"\t\t\t{GetViewManagerVariable(viewManagerName)}  = new Lazy<{viewManagerName}>(()=>new {viewManagerName}(ConnectionString,DatabaseEngine));" + Environment.NewLine;

            return ret;
        }

        private string AddPrivateConstructor()
        {
            string ret = "";

            if (_datamagerStyle == EDatamagerStyle.Classic)
                ret += "\t\tprivate DataManager()" + Environment.NewLine;
            else
                ret += "\t\tprivate Db()" + Environment.NewLine;

            ret += "\t\t{" + Environment.NewLine;
            ret += "\t\t"  + Environment.NewLine;
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
