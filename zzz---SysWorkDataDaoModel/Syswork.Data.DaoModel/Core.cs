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
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;

namespace SysWork.Data.DaoModel
{
    /// <summary>
    /// Author : Diego Martinez
    /// Email : dmartinez@syswork.com.ar
    /// Date : 07/12/2017
    /// Description : Entity/model classes Genericos
    /// 
    /// UPDATES:
    /// =======
    /// 08/12/2017: Por un tema de Performance, hago que se lea en los constructores
    ///             GetPropertyInfoList(), de esta manera solamenta hago una lectura
    ///             via reflexion de las propiedades del objeto, al crear el DAO,  y
    ///             no en cada Operacion.
    ///             
    /// 17/01/2018: Se crea la excepcion DaoModelException, para poder colectar mas 
    ///             informacion, se utiliza en Add,AddRange,Update,UpdateRange,GetById,DeleteById
    ///             
    /// 03/08/2018: Se Abstrae del motor de base de datos, utlizando las Interfaces IDbConnection
    ///             IDbCommand, IDataReader, IDataParameter.
    ///             
    ///             El motor de base de datos predeterminado es SqlServer.
    ///             
    /// </summary>
    /// <typeparam name="T"> Clase e Instanciable T</typeparam>
    internal class ColumnDbInfo
    {
        internal DbType DbType { get; set; }
        internal Int32? MaxLenght { get; set; }
    }

    /// <summary>
    /// Usar con clases Entity/Model solamente
    /// </summary>
    /// <typeparam name="T"></typeparam>

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

    public abstract class BaseDao<T>: IBaseDao<T> where T : class, new() 
    {
        public string ConnectionString { get; private set; }

        public string TableName { get; private set; }

        private Hashtable ColumnListWithDbInfo = new Hashtable();

        private EDataBaseEngine _dataBaseEngine;
        
        /// <summary>
        /// Contiene la lista de propiedades de la entidad vinculadas a la DB.
        /// </summary>
        public IList<PropertyInfo> ListObjectPropertyInfo { get; private set; }

        /// <summary>
        /// Devuelve las columnas necesarias para hacer un Insert (No incluye las columnas Identity)
        /// </summary>
        public String ColumnsForInsert { get; private set; }

        /// <summary>
        /// Devuelve las columnas de la entidad.
        /// </summary>
        public String ColumnsForSelect { get; private set; }
        
        /// <summary>
        /// Crea una nueva instancia de BaseDao, el motor a utilizar es SqlServer
        /// </summary>
        /// <param name="ConnectionString">Representa la cadena de conexion</param>
        public BaseDao(string ConnectionString)
        {
            BaseDaoConstructorResolver(ConnectionString, EDataBaseEngine.SqlServer);
        }

        /// <summary>
        /// Crea una nueva instancia de BaseDao, el motor a utilizar es SqlServer
        /// </summary>
        /// <param name="ConnectionString">Representa la cadena de conexion</param>
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
        /// <summary>
        /// Inserta una Entidad en una Tabla.
        /// Captura excepciones y en caso de ocurrir devuelve el mensaje de la misma en errMessage
        /// </summary>
        /// <returns>Devuelve el Id Generado en caso que la tabla tega una columna Identity, 0 en caso que no, y -1 si hubo un error</returns>
        /// <param name="entity"></param>
        /// 
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
        /// <summary>
        /// Inserta una Entidad en una Tabla.
        /// En caso de ocurrir una Excepcion, la captura, y lanza una DaoModelExpception
        /// la cual contiene informacion extra como sel el SqlCommand que la causo y la 
        /// excepcion original.
        /// 
        /// </summary>
        /// <returns>Devuelve el Id Generado en caso que la tabla tega una columna Identity, 0 en caso que no, y -1 si hubo un error</returns>
        /// <param name="entity"></param>
        /// 
        public long Add(T entity)
        {
            long identity = 0;
            bool hasIdentity = false;
            
            StringBuilder parameterList = new StringBuilder();

            IDbCommand dbCommand = GetIDbCommand();

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
                    using (IDbConnection dbConnection = GetIDbConnection())
                    {
                        dbConnection.Open();

                        dbCommand.CommandText = insertQuery.ToString();
                        dbCommand.Connection = dbConnection;

                        if (hasIdentity)
                        {
                            if (_dataBaseEngine == EDataBaseEngine.OleDb)
                            {
                                dbCommand.ExecuteNonQuery();
                                ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                                dbCommand.CommandText = "Select @@Identity";
                                var resultado = dbCommand.ExecuteScalar();
                                if (resultado.GetType() == typeof(System.Int64))
                                    identity = (long)resultado;
                                if (resultado.GetType() == typeof(System.Int32))
                                    identity = (long)(Int32)resultado;
                                else if (resultado.GetType() == typeof(System.Decimal))
                                    identity = (long)(Decimal)resultado;
                            }
                            else
                            {
                                var resultado = dbCommand.ExecuteScalar();
                                if (resultado.GetType() == typeof(System.Int64))
                                    identity = (long)resultado;
                                else if (resultado.GetType() == typeof(System.Decimal))
                                    identity = (long)(Decimal)resultado;
                            }
                        }
                        else
                        {
                            dbCommand.ExecuteNonQuery();
                            identity = 0;
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
            }

            return identity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetSubQueryGetIdentity()
        {
            if (_dataBaseEngine == EDataBaseEngine.SqlServer)
                return " SELECT SCOPE_IDENTITY()";
            else if (_dataBaseEngine == EDataBaseEngine.OleDb)
                // Hay que Ejecutar por separado otra consulta solicitando @@Identity
                return "";
            else if (_dataBaseEngine == EDataBaseEngine.SqLite)
                return " ; SELECT last_insert_rowid()";

            throw new ArgumentException("Es necesario revisar el metodo GetSubQueryScalar()");
        }

        /// <summary>
        /// Inserta Multiples Entidades en una tabla
        /// Captura excepciones y en caso de ocurrir devuelve el mensaje de la misma en errMessage
        /// </summary>
        /// <param name="entities"></param>
        public bool AddRange(IList<T> entities, out string errMessage)
        {
            errMessage = "";
            bool result = false;

            try
            {
                result = AddRange(entities);
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

        /// <summary>
        /// Inserta Multiples Entidades en una tabla.
        /// En caso de ocurrir una Excepcion, la captura, y lanza una DaoModelExpception
        /// la cual contiene informacion extra como ser el IdbCommand que la causo y la 
        /// excepcion original.
        /// </summary>
        /// <param name="entities"></param>
        public bool AddRange(IList<T> entities)
        {
            if (_dataBaseEngine == EDataBaseEngine.OleDb)
                return AddRangeOleDb(entities);

            StringBuilder insertRangeQuery = new StringBuilder();
            int paramNro = 0;

            IDbCommand dbCommand = GetIDbCommand();

            foreach (T entity in entities)
            {
                StringBuilder parameterList = new StringBuilder();

                foreach (PropertyInfo i in ListObjectPropertyInfo)
                {
                    var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;

                    if (!customAttribute.IsIdentity)
                    {
                        string parameterName = "@param_" + i.Name + "_" + paramNro;

                        parameterList.Append(string.Format("{0},", parameterName));

                        ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[i.Name];

                        dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));

                    }
                }

                if (parameterList.ToString() != string.Empty)
                {
                    parameterList.Remove(parameterList.Length - 1, 1);
                    insertRangeQuery.AppendLine(string.Format("INSERT INTO [{0}] ( {1} ) VALUES ( {2} );", TableName, ColumnsForInsert, parameterList));
                }

                paramNro++;
            }

            try
            {
                using (IDbConnection dbConnection = GetIDbConnection())
                {
                    dbConnection.Open();
                    dbCommand.CommandText = insertRangeQuery.ToString();
                    dbCommand.Connection = dbConnection;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    dbCommand.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }

            return true;
        }
        /// <summary>
        /// 
        /// Puesto que OleDb no permite insertar mas de una sentencia dentro del CommandText
        /// inserto de a uno los registros.
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private bool AddRangeOleDb(IList<T> entities)
        {
            using (OleDbConnection dbConnection = (OleDbConnection)GetIDbConnection())
            {
                try
                {
                    dbConnection.Open();
                }
                catch (Exception connectionException)
                {
                    throw new DaoModelException(connectionException);
                }

                foreach (T entity in entities)
                {
                    string parameterList = "";

                    OleDbCommand dbCommand = (OleDbCommand)GetIDbCommand();
                    dbCommand.Connection = dbConnection;
                    dbCommand.Parameters.Clear();

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
                        dbCommand.ConvertNamedParametersToPositionalParameters();

                        dbCommand.ExecuteNonQuery();
                    }
                    catch (Exception commandException)
                    {
                        throw new DaoModelException(commandException, dbCommand);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Actualiza una Entidad en una Tabla.
        /// Captura excepciones y en caso de ocurrir devuelve el mensaje de la misma en errMessage
        /// </summary>
        /// <param name="entity"></param>
        public bool Update(T entity, out string errMessage)
        {
            errMessage = "";
            bool result = false;

            try
            {
                result = Update(entity);
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

        /// <summary>
        /// Actualiza una Entidad
        /// En caso de ocurrir una Excepcion, la captura, y lanza una DaoModelExpception
        /// la cual contiene informacion extra como sel el SqlCommand que la causo y la 
        /// excepcion original.
        /// </summary>
        /// <param name="entity"></param>
        public bool Update(T entity)
        {
            StringBuilder parameterList = new StringBuilder();
            StringBuilder where = new StringBuilder();

            IDbCommand dbCommand = GetIDbCommand();

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
                    using (IDbConnection dbConnection = GetIDbConnection())
                    {
                        dbConnection.Open();

                        dbCommand.CommandText = updateQuery.ToString();
                        dbCommand.Connection = dbConnection;

                        if (_dataBaseEngine == EDataBaseEngine.OleDb)
                            ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
            }

            return true;
        }

        /// <summary>
        /// Actualiza multiples entidadades en una sola Query.
        /// Captura excepciones y en caso de ocurrir devuelve el mensaje de la misma en errMessage
        /// </summary>
        /// <param name="entities"></param>
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

        /// <summary>
        /// Actualiza multiples entidadades en una sola Query
        /// En caso de ocurrir una Excepcion, la captura, y lanza una DaoModelExpception
        /// la cual contiene informacion extra como sel el SqlCommand que la causo y la 
        /// excepcion original.
        /// </summary>
        /// <param name="entities"></param>
        public bool UpdateRange(IList<T> entities)
        {
            if (_dataBaseEngine == EDataBaseEngine.OleDb)
                return UpdateRangeOleDb(entities);

            StringBuilder updateRangeQuery = new StringBuilder();
            int paramNro = 0;

            IDbCommand dbCommand = GetIDbCommand();

            foreach (T entity in entities)
            {
                StringBuilder parameterList = new StringBuilder();
                StringBuilder where = new StringBuilder();

                string parameterName;

                foreach (PropertyInfo i in ListObjectPropertyInfo)
                {
                    var customAttibute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;

                    parameterName = "@param_" + i.Name + "_" + paramNro;

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

                paramNro++;
            }
            try
            {
                using (IDbConnection dbConnection = GetIDbConnection())
                {
                    dbConnection.Open();
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = updateRangeQuery.ToString();
                    System.Diagnostics.Debug.Print(dbCommand.CommandText);

                    dbCommand.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }

            return true;
        }

        private bool UpdateRangeOleDb(IList<T> entities)
        {
            using (OleDbConnection dbConnection = (OleDbConnection)GetIDbConnection())
            {
                try
                {
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

                    OleDbCommand dbCommand = (OleDbCommand)GetIDbCommand();

                    string parameterName;

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
                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = updateRangeQuery.ToString();
                        dbCommand.ConvertNamedParametersToPositionalParameters();

                        dbCommand.ExecuteNonQuery();
                    }
                    catch(Exception exceptionCommand)
                    {
                        throw new DaoModelException(exceptionCommand, dbCommand);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Busca un Item por el campo que sea Identity
        /// </summary>
        /// <param name="id"></param>
        public T GetById(object id)
        {
            T entity = new T();

            StringBuilder clause = new StringBuilder();

            IDbCommand dbCommand = GetIDbCommand();

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
                    using (IDbConnection dbConnection = GetIDbConnection())
                    {
                        dbConnection.Open();
                        dbCommand.Connection = dbConnection;

                        if (_dataBaseEngine == EDataBaseEngine.OleDb)
                            ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                        var _entities = new IDataReaderToEntity().Map<T>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);

                        if (_entities != null && _entities.Count > 0)
                            entity = _entities[0];
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
            }

            return entity;
        }

        /// <summary>
        /// Devuelve todos los registros
        /// </summary>
        /// <param name="cmdText"></param>
        public IList<T> GetAll()
        {
            IList<T> collection = new List<T>();

            IDbCommand dbCommand = GetIDbCommand();

            dbCommand.CommandText = string.Format("SELECT {0} FROM [{1}]", ColumnsForSelect, TableName);

            try
            {
                using (IDbConnection dbConnection = GetIDbConnection())
                {
                    dbConnection.Open();
                    dbCommand.Connection = dbConnection;

                    collection = new IDataReaderToEntity().Map<T>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }

            return collection;
        }

        /// <summary>
        /// Devuelve los registros que coincidan con el criterio especificado por la Expresion LAMBDA
        /// </summary>
        /// <param name="lambdaExpressionFilter">Una expresion LAMBDA</param>

        public IList<T> GetListByLambdaExpressionFilter(Expression<Func<T, bool>> lambdaExpressionFilter)
        {
            IList<T> collection = new List<T>();
            var query = new SqlLam<T>(lambdaExpressionFilter);

            IDbCommand dbCommand = GetIDbCommand();
            try
            {
                using (IDbConnection dbConnection = GetIDbConnection())
                {
                    dbConnection.Open();
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = query.QueryString;

                    foreach (var parameters in query.QueryParameters)
                    {
                        dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));
                    }

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();


                    collection = new IDataReaderToEntity().Map<T>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }

            return collection;
        }
        /// <summary>
        /// Devuelve una lista en base a una lista de Ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<T> Find(IEnumerable<object> ids)
        {
            IList<T> entities = new List<T>();
            StringBuilder clause = new StringBuilder();

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

                IDbCommand dbCommand = GetIDbCommand();
                using (IDbConnection dbConnection = GetIDbConnection())
                {
                    dbConnection.Open();
                    dbCommand.CommandText = findQuery.ToString();
                    dbCommand.Connection = dbConnection;

                    entities = new IDataReaderToEntity().Map<T>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);
                }
            }

            return entities;
        }
        /// <summary>
        /// Elimina un Registro por su Id.
        /// Captura excepciones y en caso de ocurrir devuelve el mensaje de la misma en errMessage
        /// </summary>
        /// <param name="entities"></param>
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
        /// <summary>
        /// Elimina un Registro por su Id.
        /// En caso de ocurrir una Excepcion, la captura, y lanza una DaoModelExpception
        /// la cual contiene informacion extra como sel el SqlCommand que la causo y la 
        /// excepcion original.
        /// </summary>
        /// <param name="entities"></param>

        public bool DeleteById(long Id)
        {

            T entity = new T();

            StringBuilder where = new StringBuilder();
            IDbCommand dbCommand = GetIDbCommand();

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
                    using (IDbConnection dbConnection = GetIDbConnection())
                    {
                        dbConnection.Open();

                        dbCommand.CommandText = deleteQuery.ToString();
                        dbCommand.Connection = dbConnection;

                        if (_dataBaseEngine == EDataBaseEngine.OleDb)
                            ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
            }

            return true;
        }

        protected IDbConnection GetIDbConnection()
        {
            return DataObjectResolver.GetIDbConnection(_dataBaseEngine, ConnectionString);
        }
        protected DbConnection GetDbConnection()
        {
            return DataObjectResolver.GetDbConnection(_dataBaseEngine, ConnectionString);
        }
        protected IDbCommand GetIDbCommand()
        {
            return DataObjectResolver.GetIDbCommand(_dataBaseEngine);
        }
        protected IDbDataParameter GetIDataParameter()
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
        
        public IDbDataParameter CreateIDbDataParameter(string parameterName, object value, Int32? size = null)
        {
            IDbDataParameter dataParameter = GetIDataParameter();

            dataParameter.ParameterName = parameterName;
            dataParameter.Value = value == null ? (Object)DBNull.Value : value;

            if (size != null)
                dataParameter.Size = (int)size;
            return dataParameter;
        }
        private IDbDataParameter CreateIDbDataParameter(string parameterName, DbType DbType, object value, Int32? size = null)
        {
            IDbDataParameter dataParameter = GetIDataParameter();

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
    }
    /// <summary>
    /// Author : Diego Martinez
    /// Email : dmartinez@syswork.com.ar
    /// Date : 07/12/2017
    /// Description : Mapper , dado un IDataReader y el Tipo de Objeto 
    ///               devuelve una coleccion de los que existan
    /// 
    /// UPDATES:
    /// =======
    /// 08/12/2017: Por un tema de Performance, puede recibir como parametro una lista de las
    ///             propiedades del objeto, para no tener que usar Reflexion en cada una de las
    ///             llamadas
    ///             
    /// </summary>
    public class IDataReaderToEntity
    {
        /// <summary>
        /// Mapea una entidad desde un SqlDataReader
        /// </summary>
        /// <param name="reader">Representa la cadena de conexion</param>
        /// <typeparam name="T">Entidad a mapear,solo los atributos que posean el decorador DbColumn seran tenidos en cuenta</typeparam>
        public IList<T> Map<T>(IDataReader reader) where T : class, new()
        {
            return Map<T>(reader, null);
        }

        /// <summary>
        /// **OPTIMIZADO** Mapea una entidad desde un IDataReader, Ideal para Bucles o multiples iteraciones, donde ya tenemos info del Objeto etc.
        /// </summary>
        /// <param name="reader">Representa la cadena de conexion</param>
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
