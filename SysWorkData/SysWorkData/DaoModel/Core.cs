using SysWork.Data.DaoModel.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SysWork.Data.DaoModel.LambdaSqlBuilder;
using System.Data.Common;
using SysWork.Data.Common;
using SysWork.Data.Common.ObjectResolver;
using System.Data.OleDb;
using SysWork.Data.Extensions.OleDbCommandExtensions;

namespace SysWork.Data.DaoModel
{
    internal class ColumnDbInfo
    {
        internal DbType DbType { get; set; }
        internal Int32? MaxLenght { get; set; }
    }

    public interface IBaseDao<T>
    {
        long Add(T entity);
        bool AddRange(IList<T> entities);
        bool Update(T entity);
        bool UpdateRange(IList<T> entities);
        bool DeleteById(long id);
        T GetById(object id);
        IList<T> GetAll();
        IList<T> Find(IEnumerable<object> ids);
    }
    /// <summary>
    /// Author : Diego Martinez
    /// Email : dmartinez@syswork.com.ar
    /// Date : 07/12/2017
    /// Description : Clase abstracta para implementar CRUD con las bases de datos.
    ///               Debe definirse el motor de base de datos. Por default es MSSqlServer.
    /// 
    /// Update : 17/08/2017
    /// ======   ==========
    ///         Los metodos, permiten recibir una conexion externa, y soportan transacciones explicitas.
    /// 

    public abstract class BaseDao<T>: IBaseDao<T> where T : class, new() 
    {
        private DbConnection _persistentConnection;

        public string ConnectionString { get; private set; }

        public string TableName { get; private set; }

        private Hashtable ColumnListWithDbInfo = new Hashtable();

        private EDataBaseEngine _dataBaseEngine;
        
        /// <summary>
        /// Contiene la lista de propiedades de la entidad vinculadas a la DB.
        /// </summary>
        public IList<PropertyInfo> ListObjectPropertyInfo { get; private set; }

        /// <summary>
        /// Devuelve las columnas necesarias para hacer un Insert (No incluye las columnas tipo Identity)
        /// </summary>
        public String ColumnsForInsert { get; private set; }

        /// <summary>
        /// Devuelve las columnas que deberian utilizarse en un SELECT.
        /// </summary>
        public String ColumnsForSelect { get; private set; }

        /// <summary>
        /// Crea una nueva instancia de BaseDao, el motor a utilizar es MSSqlServer
        /// </summary>
        /// <param name="ConnectionString">Cadena de conexion valida</param>
        public BaseDao(string ConnectionString)
        {
            BaseDaoConstructorResolver(ConnectionString, EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Crea una nueva instancia de BaseDao, especificando el motor de base de datos.
        /// </summary>
        /// <param name="ConnectionString">Cadena de conexion valida</param>
        /// <param name="dataBaseEngine">El motor de base de datos a utilizar</param>
        public BaseDao(string ConnectionString, EDataBaseEngine dataBaseEngine)
        {
            BaseDaoConstructorResolver(ConnectionString, dataBaseEngine);
        }

        private void BaseDaoConstructorResolver(string ConnectionString, EDataBaseEngine dataBaseEngine)
        {
            _dataBaseEngine = dataBaseEngine;

            T entity = new T();
            ListObjectPropertyInfo = GetPropertyInfoList(entity);

            TableName = GetTableNameFromEntity(entity.GetType());

            if ((ListObjectPropertyInfo == null) || (ListObjectPropertyInfo.Count == 0))
            {
                throw new Exception(string.Format("La entidad {0}, no tiene atributos vinculados a la tabla: {1}, Utilice el decorador DbColumn para vincular las propiedades a los campos de la tabla", entity.GetType().Name, TableName));
            }

            this.ConnectionString = ConnectionString;

            GetDbColumnsAndAtributes();

        }
        private static string GetTableNameFromEntity(Type type)
        {
            var column = type.GetCustomAttributes(false).OfType<DbTableAttribute>().FirstOrDefault();
            if (column != null)
                return column.Name;
            else
                return type.Name;
        }

        public long Add(T entity)
        {
            return Add(entity, null, null);
        }

        public long Add(T entity, out string errMessage)
        {
            errMessage = "";
            long identity = 0;

            try
            {
                identity = Add(entity);
            }
            catch (DaoModelException daoModelException)
            {
                errMessage = daoModelException.OriginalException.Message;
                identity = -1;
            }
            catch (Exception exception)
            {
                errMessage = exception.Message;
                identity = -1;
            }
            return identity;
        }
        public long Add(T entity, IDbConnection extConnection)
        {
            return Add(entity, extConnection, null);
        }

        public long Add(T entity, IDbConnection extConnection,IDbTransaction dbTransaction)
        {
            long identity = 0;
            bool hasIdentity = false;

            if (extConnection == null && dbTransaction != null)
                throw new ArgumentException("Se especifico una dbTransaction pero no una dbConnection externa");

            IDbConnection dbConnection = extConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            StringBuilder parameterList = new StringBuilder();
            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttibute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (!customAttibute.IsIdentity)
                {
                    string parameterName = "@param_" + i.Name;

                    parameterList.Append(string.Format("{0},", parameterName));

                    ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[i.Name];

                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                }
                else
                {
                    hasIdentity = true;
                }
            }

            if (parameterList.ToString() != string.Empty)
            {
                parameterList.Remove(parameterList.Length - 1, 1);

                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append(string.Format("INSERT INTO [{0}] ( {1} ) VALUES ( {2} ) {3}  ", TableName, ColumnsForInsert, parameterList, GetSubQueryGetIdentity()));
                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = insertQuery.ToString();

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    if (hasIdentity)
                    {
                        if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        {
                            // Se ejecuta la insercion y luego se pregunta por el Identity;
                            dbCommand.ExecuteNonQuery();

                            dbCommand.CommandText = "Select @@Identity";
                            identity = ParseToLong(dbCommand.ExecuteScalar());
                        }
                        else
                        {
                            identity = ParseToLong(dbCommand.ExecuteScalar());
                        }
                    }
                    else
                    {
                        dbCommand.ExecuteNonQuery();
                        identity = 0;
                    }

                    if (extConnection == null)
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }

                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
            }

            return identity;
        }

        private string GetSubQueryGetIdentity()
        {
            if (_dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return " SELECT SCOPE_IDENTITY()";
            else if (_dataBaseEngine == EDataBaseEngine.OleDb)
                // Hay que Ejecutar por separado otra consulta solicitando @@Identity
                return "";
            else if (_dataBaseEngine == EDataBaseEngine.SqLite)
                return " ; SELECT last_insert_rowid()";

            throw new ArgumentException("Es necesario revisar el metodo GetSubQueryScalar()");
        }
        public bool AddRange(IList<T> entities)
        {
            return AddRange(entities, null, null,out long recordsAffected);
        }
        public bool AddRange(IList<T> entities, out string errMessage)
        {
            errMessage = "";
            bool result = false;

            try
            {
                result = AddRange(entities,null);
            }
            catch (DaoModelException daoModelException)
            {
                errMessage = daoModelException.OriginalException.Message;
                result = false;
            }
            catch (Exception exception)
            {
                errMessage = exception.Message;
                result = false;
            }

            return result;
        }
        public bool AddRange(IList<T> entities, IDbConnection extConnection)
        {
            return AddRange(entities, extConnection,null,out long recordsaffected);
        }
        public bool AddRange(IList<T> entities, IDbConnection extConnection, IDbTransaction dbTransaction)
        {
            return AddRange(entities, extConnection, dbTransaction, out long recordsAffected);
        }
        public bool AddRange(IList<T> entities,IDbConnection extConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;
            if (extConnection == null && dbTransaction != null)
                throw new ArgumentException("Se especifico una dbTransaction pero no una dbConnection externa");

            IDbConnection dbConnection = extConnection ?? GetIDbConnection();
            IDbCommand dbCommand;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
            }
            catch (Exception connectionException)
            {
                throw new DaoModelException(connectionException);
            }

            foreach (T entity in entities)
            {
                string parameterList = "";
                dbCommand = dbConnection.CreateCommand();

                string insertRangeQuery="";
                foreach (PropertyInfo i in ListObjectPropertyInfo)
                {
                    var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                    if (!customAttribute.IsIdentity)
                    {
                        string parameterName = "@param_" + i.Name ;
                        parameterList += string.Format("{0},", parameterName);

                        ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[i.Name];
                        dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                    }
                }

                if (parameterList != string.Empty)
                {
                    parameterList = parameterList.Substring(0, parameterList.Length - 1);
                    insertRangeQuery = (string.Format("INSERT INTO [{0}] ( {1} ) VALUES ( {2} );", TableName, ColumnsForInsert, parameterList));
                }

                try
                {
                    dbCommand.CommandText = insertRangeQuery;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    if (dbTransaction!=null)
                        dbCommand.Transaction = (OleDbTransaction)dbTransaction;

                    recordsAffected += dbCommand.ExecuteNonQuery();

                }
                catch (Exception commandException)
                {
                    throw new DaoModelException(commandException, dbCommand);
                }
            }

            if (extConnection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return true;
        }

        public bool Update(T entity)
        {
            return Update(entity, null, null);
        }

        public bool Update(T entity, out string errMessage)
        {
            errMessage = "";
            bool result = false;

            try
            {
                result = Update(entity,null,null);
            }
            catch (DaoModelException daoModelException)
            {
                errMessage = daoModelException.OriginalException.Message;
                result = false;
            }
            catch (Exception exception)
            {
                errMessage = exception.Message;
                result = false;
            }

            return result;
        }
        public bool Update(T entity, IDbConnection extConnection)
        {
            return Update(entity, extConnection, null);
        }
        public bool Update(T entity, IDbConnection extConnection, IDbTransaction dbTransaction)
        {
            return Update(entity, extConnection, dbTransaction, out long recordsAffected);
        }
        public bool Update(T entity, IDbConnection extConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;

            if (extConnection == null && dbTransaction != null)
                throw new ArgumentException("Se especifico una dbTransaction pero no una dbConnection externa");

            IDbConnection dbConnection = extConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            StringBuilder parameterList = new StringBuilder();
            StringBuilder where = new StringBuilder();

            bool hasPrimary = false;

            string parameterName;
            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                parameterName = "@param_" + i.Name;

                if (!customAttribute.IsPrimary)
                {
                    parameterList.Append(string.Format("[{0}] = {1},", i.Name, parameterName));
                }
                else
                {
                    hasPrimary = true;

                    if (where.ToString() != String.Empty)
                        where.Append(" AND ");

                    where.Append(string.Format("([{0}] = {1})", i.Name, parameterName));
                }

                ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[i.Name];
                dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
            }

            if (!hasPrimary)
            {
                throw new ArgumentException("No hay una clave primaria definida en la entidad, al menos un atributo debe tener la propiedad IsPrimary = True");
            }

            if (parameterList.ToString() != string.Empty)
            {
                parameterList.Remove(parameterList.Length - 1, 1);
                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append(string.Format("UPDATE  [{0}] SET {1} WHERE {2}", TableName, parameterList, where));

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = updateQuery.ToString();

                    if (dbTransaction != null)
                        dbCommand.Transaction= dbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected = dbCommand.ExecuteNonQuery();
                    
                    if (extConnection == null)
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
            }
            return true;
        }
        public bool UpdateRange(IList<T> entities)
        {
            return UpdateRange(entities, null, null);
        }

        public bool UpdateRange(IList<T> entities, out string errMessage)
        {
            bool result = false;
            errMessage = "";

            try
            {
                result = UpdateRange(entities);
            }
            catch (DaoModelException daoModelException)
            {
                errMessage = daoModelException.OriginalException.Message;
                result = false;
            }
            catch (Exception exception)
            {
                errMessage = exception.Message;
                result = false;
            }

            return result;
        }

        private bool UpdateRange(IList<T> entities, IDbConnection extConnection)
        {
            return UpdateRange(entities, extConnection, null,out long recodsAffected);
        }
        private bool UpdateRange(IList<T> entities, IDbConnection extConnection, IDbTransaction dbTransaction)
        {
            return UpdateRange(entities, extConnection, dbTransaction, out long recordsAffected);
        }

        private bool UpdateRange(IList<T> entities,IDbConnection extConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;

            if (extConnection == null && dbTransaction != null)
                throw new ArgumentException("Se especifico una dbTransaction pero no una dbConnection externa");

            IDbConnection dbConnection = extConnection ?? GetIDbConnection();
            IDbCommand dbCommand;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
            }
            catch (Exception exceptionDb)
            {
                throw new DaoModelException(exceptionDb);
            }

            foreach (T entity in entities)
            {
                StringBuilder updateRangeQuery = new StringBuilder();
                StringBuilder parameterList = new StringBuilder();
                StringBuilder where = new StringBuilder();

                string parameterName;
                dbCommand = dbConnection.CreateCommand();

                foreach (PropertyInfo i in ListObjectPropertyInfo)
                {
                    var customAttibute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;

                    parameterName = "@param_" + i.Name;

                    if (!customAttibute.IsPrimary)
                    {
                        parameterList.Append(string.Format("[{0}] = {1},", i.Name, parameterName));
                    }
                    else
                    {
                        if (where.ToString() != String.Empty)
                            where.Append(" AND ");

                        where.Append(string.Format("([{0}] = {1})", i.Name, parameterName));
                    }
                    ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                }

                if (parameterList.ToString() != string.Empty)
                {
                    parameterList.Remove(parameterList.Length - 1, 1);
                    updateRangeQuery.AppendLine(string.Format("UPDATE [{0}] SET {1} WHERE {2}; ", TableName, parameterList, where));
                }

                try
                {
                    dbCommand.CommandText = updateRangeQuery.ToString();

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected += dbCommand.ExecuteNonQuery();
                }
                catch(Exception exceptionCommand)
                {
                    throw new DaoModelException(exceptionCommand, dbCommand);
                }
            }

            if (extConnection == null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return true;
        }
        public bool DeleteById(long Id)
        {
            return DeleteById(Id, null, null);
        }

        public bool DeleteById(long Id, out string errMessage)
        {
            bool result = false;
            errMessage = "";

            try
            {
                result = DeleteById(Id);
            }
            catch (DaoModelException daoModelException)
            {
                errMessage = daoModelException.OriginalException.Message;
                result = false;
            }
            catch (Exception exception)
            {
                errMessage = exception.Message;
                result = false;
            }

            return result;
        }
        public bool DeleteById(long Id, IDbConnection extConnection)
        {
            return DeleteById(Id, extConnection, null);
        }
        public bool DeleteById(long Id, IDbConnection extConnection, IDbTransaction dbTransaction)
        {
            return DeleteById(Id, extConnection, dbTransaction, out long recordsAffected);
        }

        public bool DeleteById(long Id, IDbConnection extConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;
            if (extConnection == null && dbTransaction != null)
                throw new ArgumentException("Se especifico una dbTransaction pero no una dbConnection externa");

            IDbConnection dbConnection = extConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            T entity = new T();
            StringBuilder where = new StringBuilder();

            string parameterName;
            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;

                parameterName = "@param_" + i.Name;

                if (customAttribute.IsPrimary)
                {
                    if (where.ToString() != String.Empty)
                        where.Append(" AND ");

                    where.Append(string.Format("([{0}] = {1})", i.Name, parameterName));

                    ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, Id, cdbi.MaxLenght));

                }
            }

            if (where.ToString() != string.Empty)
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(string.Format("DELETE FROM [{0}] WHERE {1}", TableName, where));

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = deleteQuery.ToString();

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected = dbCommand.ExecuteNonQuery();

                    if (extConnection == null)
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
            }

            return true;
        }

        public T GetById(object id)
        {
            return GetById(id, null);
        }

        public T GetById(object id,IDbConnection extConnection)
        {
            T entity = new T();

            StringBuilder clause = new StringBuilder();

            IDbConnection dbConnection = extConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            foreach (var pi in ListObjectPropertyInfo)
            {
                var pk = pi.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (pk != null && pk.IsPrimary)
                {
                    string parameterName = "@param_" + pi.Name;
                    clause.Append(string.Format("[{0}]={1}", pi.Name, id));

                    ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[pi.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, id, cdbi.MaxLenght));

                    break;
                }
            }

            entity = null;

            if (clause.ToString() != string.Empty)
            {
                StringBuilder getQuery = new StringBuilder();
                getQuery.Append(string.Format("SELECT {0} FROM [{1}] WHERE {2}", ColumnsForSelect, TableName, clause));

                dbCommand.CommandText = getQuery.ToString();

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    var _entities = new IDataReaderToEntity().Map<T>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);

                    if (_entities != null && _entities.Count > 0)
                        entity = _entities[0];

                    if (extConnection == null)
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
            }

            return entity;
        }

        public IList<T> GetAll()
        {
            return GetAll(null);
        }
        public IList<T> GetAll(IDbConnection extConnection)
        {
            IList<T> collection = new List<T>();

            IDbConnection dbConnection = extConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = string.Format("SELECT {0} FROM [{1}]", ColumnsForSelect, TableName);

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                collection = new IDataReaderToEntity().Map<T>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);

                if (extConnection == null)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }

            return collection;
        }

        public IList<T> GetListByLambdaExpressionFilter(Expression<Func<T, bool>> lambdaExpressionFilter)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, null);
        }
        public IList<T> GetListByLambdaExpressionFilter(Expression<Func<T, bool>> lambdaExpressionFilter, IDbConnection extConnection)
        {
            IList<T> resultado = new List<T>();
            var query = new SqlLam<T>(lambdaExpressionFilter);

            IDbConnection dbConnection = extConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                if(dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dbCommand.CommandText = query.QueryString;

                foreach (var parameters in query.QueryParameters)
                {
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));
                }

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                resultado = new IDataReaderToEntity().Map<T>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);

                if (extConnection == null)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }

            return resultado;
        }

        public IList<T> Find(IEnumerable<object> ids)
        {
            return Find(ids, null);
        }

        public IList<T> Find(IEnumerable<object> ids, IDbConnection extConnection)
        {
            IList<T> entities = new List<T>();
            StringBuilder clause = new StringBuilder();

            IDbConnection dbConnection = extConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            foreach (var pi in ListObjectPropertyInfo)
            {
                var pk = pi.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (pk != null && pk.IsPrimary)
                {
                    string _ids = string.Empty;
                    foreach (var id in ids)
                    {
                        if (_ids != string.Empty)
                            _ids = _ids + ",";

                        _ids = _ids + id.ToString();
                    }

                    clause.Append(string.Format("[{0}] IN ({1})", pi.Name, _ids));
                    break;
                }
            }

            if (clause.ToString() != string.Empty)
            {
                StringBuilder findQuery = new StringBuilder();
                findQuery.Append(string.Format("SELECT {0} FROM [{1}] WHERE {2}", ColumnsForSelect, TableName, clause));

                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dbCommand.CommandText = findQuery.ToString();

                entities = new IDataReaderToEntity().Map<T>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);

                if (extConnection == null)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }

            return entities;
        }
        /// <summary>
        /// Devuelve un *IDbConnection*, dependiendo del motor de base de datos del DAO
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetIDbConnection()
        {
            return DataObjectResolver.GetIDbConnection(_dataBaseEngine, ConnectionString);
        }
        /// <summary>
        /// Devuelve un *DbConnection*, dependiendo del motor de base de datos del DAO
        /// </summary>
        /// <returns></returns>
        protected DbConnection GetDbConnection()
        {
            return DataObjectResolver.GetDbConnection(_dataBaseEngine, ConnectionString);
        }
        /// <summary>
        /// A no ser que el Usuario Cierre esta DbConnection, la misma permanecerá activa.
        /// </summary>
        /// <returns></returns>
        protected DbConnection GetPersistentDbConnection()
        {
            if (_persistentConnection == null)
                _persistentConnection = DataObjectResolver.GetDbConnection(_dataBaseEngine, ConnectionString);

            return _persistentConnection;
        }
        /// <summary>
        /// Devuelve un *IDbCommand*, dependiendo del motor de base de datos del DAO
        /// </summary>
        /// <returns></returns>
        protected IDbCommand GetIDbCommand()
        {
            return DataObjectResolver.GetIDbCommand(_dataBaseEngine);
        }
        /// <summary>
        /// Devuelve un IDbParameter, dependiendo del motor de base de datos del DAO
        /// </summary>
        /// <returns></returns>
        protected IDbDataParameter GetIDbDataParameter()
        {
            return DataObjectResolver.GetIDbDataParameter(_dataBaseEngine);
        }

        /// <summary>
        /// Obtiene la lista de Columnas para INSERT (No incluye las columnas con atributo IsIdentity) 
        /// Obtiene la lista de Columnas para SELECT.
        /// Obtiene atributos de las columnas en la base de datos (Tipo y Longitud) 
        /// </summary>
        private void GetDbColumnsAndAtributes()
        {
            T entity = new T();

            StringBuilder columnsInsert = new StringBuilder();
            StringBuilder columnsSelect = new StringBuilder();

            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;

                if (!customAttribute.IsIdentity)
                    columnsInsert.Append(string.Format("[{0}],", i.Name));

                columnsSelect.Append(string.Format("[{0}],", i.Name));

                ColumnListWithDbInfo.Add(i.Name, GetColumnDbInfo(i.Name));
            }

            if (columnsInsert.Length > 0)
                columnsInsert.Remove(columnsInsert.Length - 1, 1);

            if (columnsSelect.Length > 0)
                columnsSelect.Remove(columnsSelect.Length - 1, 1);

            ColumnsForInsert = columnsInsert.ToString();
            ColumnsForSelect = columnsSelect.ToString();
        }

        private static readonly Dictionary<string, DbType> s_sqlTypeToDbType = new Dictionary<string, DbType>(StringComparer.OrdinalIgnoreCase)
        {
            { "smallint", DbType.Int16 },
            { "int", DbType.Int32 },
            { "bigint", DbType.Int64 },
            { "integer", DbType.Int64 },
            { "long", DbType.Int64 },
            { "bit", DbType.Boolean },
            { "bool", DbType.Boolean },
            { "boolean", DbType.Boolean },
            { "blob", DbType.Binary },
            { "binary", DbType.Binary },
            { "image", DbType.Binary },
            { "datetime", DbType.DateTime },
            { "date", DbType.Date },
            { "double", DbType.Double },
            { "float", DbType.Double },
            { "real", DbType.Double },
            { "guid", DbType.Guid },
            { "money", DbType.Decimal},
            { "currency", DbType.Decimal},
            { "smallmoney", DbType.Decimal},
            { "decimal", DbType.Decimal},
            { "numeric", DbType.Decimal},
            { "unsignedtinyint", DbType.UInt16},
            { "single", DbType.Single},
            { "string", DbType.String },
            { "varchar", DbType.String },
            { "nvarchar", DbType.String },
            { "nchar", DbType.String },
            { "char", DbType.AnsiStringFixedLength},
            { "ntext", DbType.String },
            { "text", DbType.String },
            { "WChar", DbType.String },
            { "varwchar", DbType.String },
            { "longvarwchar", DbType.String },
        };
        private ColumnDbInfo GetColumnDbInfo(string columnName)
        {
            ColumnDbInfo columnData = new ColumnDbInfo();

            using (DbConnection dbConnection = GetDbConnection())
            {
                dbConnection.Open();

                DataTable columnProperty;
                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    columnProperty = dbConnection.GetSchema("Columns", new[] { null, null, TableName, columnName });
                else
                    columnProperty = dbConnection.GetSchema("Columns", new[] { dbConnection.Database, null, TableName,columnName});

                DataRow dataRow  = columnProperty.Rows[0];

                columnData.MaxLenght = null;
                if (Int32.TryParse(dataRow["CHARACTER_MAXIMUM_LENGTH"].ToString(), out Int32 maxLength))
                    columnData.MaxLenght= maxLength;

                var dataType = dataRow["DATA_TYPE"];

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                {
                    if (s_sqlTypeToDbType.TryGetValue(((OleDbType)dataType).ToString(), out DbType dBTypeValue))
                        columnData.DbType = dBTypeValue;
                    else
                        throw new Exception("Tipo de datos no reconocido");
                }
                else
                {
                    if (s_sqlTypeToDbType.TryGetValue(dataType.ToString(), out DbType dBTypeValue))
                        columnData.DbType = dBTypeValue;
                    else
                        throw new Exception("Tipo de datos no reconocido");
                }

                dbConnection.Close();

                return columnData;
            }
        }
        /// <summary>
        /// Permite crear un IDbParamenter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected IDbDataParameter CreateIDbDataParameter(string parameterName, object value, Int32? size = null)
        {
            IDbDataParameter dataParameter = GetIDbDataParameter();

            dataParameter.ParameterName = parameterName;
            dataParameter.Value = value == null ? (Object)DBNull.Value : value;

            if (size != null)
                dataParameter.Size = (int)size;
            return dataParameter;
        }
        /// <summary>
        /// Permite crear un IDbParamenter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="DbType"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected IDbDataParameter CreateIDbDataParameter(string parameterName, DbType DbType, object value, Int32? size = null)
        {
            IDbDataParameter dataParameter = GetIDbDataParameter();

            dataParameter.ParameterName = parameterName;
            dataParameter.DbType = DbType;
            dataParameter.Value = value == null ? (Object)DBNull.Value : value;

            if (size != null && size !=0)
                dataParameter.Size = (int)size;

            return dataParameter;
        }

        private IList<PropertyInfo> GetPropertyInfoList(T entity)
        {
            return entity.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();
        }

        public string GetNextCode(string field, int lenght)
        {
            string newCode = new string('0', lenght);
            IDbCommand dbCommand = GetIDbCommand();
            try
            {
                using (IDbConnection dbConnection = GetIDbConnection())
                {
                    string query = "SELECT ISNULL(MAX({0}),0) AS MAXIMO FROM {1} WHERE (ISNUMERIC({2}) = 1) AND LEN({3}) = {4}";
                    query = string.Format(query, field, TableName, field, field, lenght);

                    dbConnection.Open();

                    dbCommand.CommandText = query;
                    dbCommand.Connection = dbConnection;

                    var value = dbCommand.ExecuteScalar();
                    var numberValue = Int32.Parse(value.ToString()) + 1;
                    newCode = string.Format("{0:" + new string('0', lenght) + "}", numberValue);

                    dbCommand.Dispose();
                    dbConnection.Close();
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }

            return newCode;
        }
        private long ParseToLong(object result)
        {
            if (result.GetType() == typeof(System.Int64))
                return (long)result;
            if (result.GetType() == typeof(System.Int32))
                return (long)(Int32)result;
            else if (result.GetType() == typeof(System.Decimal))
                return (long)(Decimal)result;
            else
                throw new ArgumentOutOfRangeException("El resultado obtenido no puede Parsearse");
        }

    }
    /// <summary>
    /// Author : Diego Martinez
    /// Email : dmartinez@syswork.com.ar
    /// Date : 07/12/2017
    /// Description : Mapper , dado un IDataReader y el Tipo de Entidad devuelve una coleccion de dichas entidades.
    public class IDataReaderToEntity
    {
        /// <summary>
        /// Mapea una entidad desde un IDataReader
        /// </summary>
        /// <param name="reader">IDataReader a mapear</param>
        /// <typeparam name="T">Entidad a mapear,solo los atributos que posean el decorador DbColumn seran tenidos en cuenta</typeparam>
        public IList<T> Map<T>(IDataReader reader) where T : class, new()
        {
            return Map<T>(reader, null);
        }

        /// <summary>
        /// **OPTIMIZADO** Mapea una entidad desde un IDataReader, Ideal para Bucles o multiples iteraciones, donde ya tenemos info del Objeto etc.
        /// </summary>
        /// <param name="reader">IDataReader a mapear</param>
        /// <param name="listObjectPropertyInfo">En el caso que se posea la lista de atributos de la clase que esten vinculados a la DB, se pasan como parametro, evitando volver a buscarlos</param>
        /// <typeparam name="T">Entidad a mapear,solo los atributos que posean el decorador DbColumn seran tenidos en cuenta</typeparam>
        public IList<T> Map<T>(IDataReader reader, IList<PropertyInfo> listObjectPropertyInfo) where T : class, new()
        {
            T obj = new T();

            IList<PropertyInfo> _propertyInfo;

            if (listObjectPropertyInfo != null)
                _propertyInfo = listObjectPropertyInfo;
            else
                _propertyInfo = obj.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();

            IList<T> collection = new List<T>();

            while (reader.Read())
            {
                obj = new T();

                foreach (PropertyInfo i in _propertyInfo)
                {
                    try
                    {
                        var custumAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute));

                        if (((DbColumnAttribute)custumAttribute).Convert == true)
                        {
                            if (reader[i.Name] != DBNull.Value)
                                i.SetValue(obj, Convert.ChangeType(reader[i.Name], i.PropertyType));
                        }
                        else
                        {
                            if (reader[i.Name] != DBNull.Value)
                            {
                                var value = reader[i.Name];
                                var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                                var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                                i.SetValue(obj, safeValue);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }
                collection.Add(obj);
            }
            return collection;
        }

        /// <summary>
        ///  Dado un IDataReader y el Tipo de Entidad devuelve una entidad mapeada.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="listObjectPropertyInfo"></param>
        /// <returns></returns>
        public T MapSingle<T>(IDataReader reader, IList<PropertyInfo> listObjectPropertyInfo) where T : class, new()
        {
            T obj = new T();

            IList<PropertyInfo> _propertyInfo;

            if (listObjectPropertyInfo != null)
                _propertyInfo = listObjectPropertyInfo;
            else
                _propertyInfo = obj.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();

            obj = new T();

            foreach (PropertyInfo i in _propertyInfo)
            {
                try
                {
                    var custumAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute));

                    if (((DbColumnAttribute)custumAttribute).Convert == true)
                    {
                        if (reader[i.Name] != DBNull.Value)
                            i.SetValue(obj, Convert.ChangeType(reader[i.Name], i.PropertyType));
                    }
                    else
                    {
                        if (reader[i.Name] != DBNull.Value)
                        {
                            var value = reader[i.Name];
                            var type = Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType;
                            var safeValue = (value == null) ? null : Convert.ChangeType(value, type);

                            i.SetValue(obj, safeValue);
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            return obj;
        }
    }
    /// <summary>
    /// 
    /// Wrapper de excepciones, permite capturar la excepcion original, en caso que haya un DbCommand,
    /// el contenido del mismo y el StackTrace original.
    /// 
    /// </summary>
    public class DaoModelException : Exception
    {
        public Exception OriginalException { get; private set; }
        public IDbCommand DbCommand { get; private set; }
        public string OriginalStackTrace { get; private set; }

        public DaoModelException(Exception originalException, IDbCommand dbCommand) : base(originalException.Message)
        {
            this.OriginalException = originalException;
            this.DbCommand = dbCommand;
            this.OriginalStackTrace = originalException.StackTrace;
        }
        public DaoModelException(Exception originalException) : base(originalException.Message)
        {
            this.OriginalException = originalException;
            this.DbCommand = null;
            this.OriginalStackTrace = originalException.StackTrace;
        }
    }
}
