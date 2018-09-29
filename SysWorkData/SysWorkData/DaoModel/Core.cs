using SysWork.Data.DaoModel.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Data.OleDb;
using System.Data.Common;
using SysWork.Data.Common;
using SysWork.Data.Common.ObjectResolver;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Extensions.OleDbCommandExtensions;
using SysWork.Data.DaoModel.Interfaces;
using SysWork.Data.DaoModel.Exceptions;
using SysWork.Data.Common.DbConnectionUtilities;
using SysWork.Data.Syntax;
using SysWork.Data.Common.LambdaSqlBuilder.ValueObjects;

namespace SysWork.Data.DaoModel
{
    internal class ColumnDbInfo
    {
        internal DbType DbType { get; set; }
        internal Int32? MaxLenght { get; set; }
    }
    /// <summary>
    /// Author : Diego Martinez
    /// Email : dmartinez@syswork.com.ar
    /// Date : 07/12/2017
    /// Description : Clase abstracta para implementar operaciones CRUD.
    ///               Debe definirse el motor de base de datos. Por default es MSSqlServer.
    /// 
    /// Update : 17/08/2018
    /// ======   ==========
    ///         Los metodos, permiten recibir una conexion externa, y soportan transacciones explicitas.
    /// 
    public abstract class BaseDao<TEntity>: IBaseDao<TEntity> where TEntity : class, new() 
    {
        private EDataBaseEngine _dataBaseEngine;

        private Hashtable ColumnListWithDbInfo = new Hashtable();

        public string ConnectionString { get; private set; }

        protected DataObjectProvider _dataObjectProvider { get; private set; }
        private SyntaxProvider _syntaxProvider;

        public string TableName { get; private set; }
        
        /// <summary>
        /// 
        /// 
        /// Contiene la lista de propiedades de la entidad vinculadas a la DB.
        /// 
        /// 
        /// </summary>
        public IList<PropertyInfo> ListObjectPropertyInfo { get; private set; }

        /// <summary>
        /// 
        /// 
        /// Devuelve las columnas necesarias para hacer un Insert (No incluye las columnas tipo Identity)
        /// 
        /// 
        /// </summary>
        public String ColumnsForInsert { get; private set; }

        /// <summary>
        /// 
        /// 
        /// Devuelve las columnas que deberian utilizarse en un SELECT.
        /// 
        /// 
        /// </summary>
        public String ColumnsForSelect { get; private set; }

        /// <summary>
        /// 
        /// 
        /// Crea una nueva instancia de BaseDao, el motor a utilizar es MSSqlServer
        /// 
        /// 
        /// </summary>
        /// <param name="ConnectionString">Cadena de conexion valida</param>
        public BaseDao(string ConnectionString)
        {
            BaseDaoConstructorResolver(ConnectionString, EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// 
        /// 
        /// Crea una nueva instancia de BaseDao, informando el motor de base de 
        /// datos a utilizar.
        /// 
        /// 
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
            _dataObjectProvider = new DataObjectProvider(_dataBaseEngine);
            _syntaxProvider = new SyntaxProvider(_dataBaseEngine);

            SetSqlLamAdapter();

            TEntity entity = new TEntity();
            ListObjectPropertyInfo = GetPropertyInfoList(entity);

            TableName = GetTableNameFromEntity(entity.GetType());

            if ((ListObjectPropertyInfo == null) || (ListObjectPropertyInfo.Count == 0))
            {
                throw new Exception(string.Format("La entidad {0}, no tiene atributos vinculados a la tabla: {1}, Utilice el decorador DbColumn para vincular las propiedades a los campos de la tabla", entity.GetType().Name, TableName));
            }

            this.ConnectionString = ConnectionString;

            GetDbColumnsAndAtributes();
        }

        private void SetSqlLamAdapter()
        {
            if (_dataBaseEngine == EDataBaseEngine.MSSqlServer)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.SqlServer2012);
            else if (_dataBaseEngine == EDataBaseEngine.OleDb)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.SqlServer2012);
            else if (_dataBaseEngine == EDataBaseEngine.MySql)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.MySql);
            else if (_dataBaseEngine == EDataBaseEngine.SqLite)
                SqlLam<TEntity>.SetAdapter(SqlAdapter.SQLite);
            else
                throw new Exception("No se a definido un SqlLamAdapter para el tipo de Motor de Base de datos");
        }

        private string GetTableNameFromEntity(Type type)
        {
            var column = type.GetCustomAttributes(false).OfType<DbTableAttribute>().FirstOrDefault();
            if (column != null)
                return column.Name;
            else
                return type.Name;
        }

        /// <summary>
        /// 
        /// Agrega una entidad del tipo <T>
        /// 
        /// En caso de exito devuelve la identidad insertada. 
        /// En caso de no tener un campo tipo identity, devuelve 0
        /// En caso de error devuelve un -1 y el mensaje de la excepcion.
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public long Add(TEntity entity, out string errMessage)
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
        /// 
        /// Agrega una entidad del tipo <T>
        /// 
        /// En caso de exito devuelve la identidad insertada. 
        /// En caso de no tener un campo tipo identity, devuelve 0
        /// En caso de error lanza una excepcion del tipo DaoModel.
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="DaoModelException">Encapsula la excepcion original</exception>
        /// <returns></returns>
        public long Add(TEntity entity)
        {
            return Add(entity, null, null);
        }

        /// <summary>
        /// 
        /// Agrega una entidad del tipo <T>
        /// 
        /// En caso de exito devuelve la identidad insertada. 
        /// En caso de no tener un campo tipo identity, devuelve 0
        /// En caso de error lanza una excepcion del tipo DaoModel.
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="paramDbConnection">Esta conexion no se cerrará en el método</param>
        /// <exception cref="DaoModelException">Encapsula la excepcion original</exception>
        /// <returns></returns>
        public long Add(TEntity entity, IDbConnection paramDbConnection)
        {
            return Add(entity, paramDbConnection, null);
        }

        /// <summary>
        /// 
        /// Agrega una entidad del tipo <T>
        /// 
        /// En caso de exito devuelve la identidad insertada. 
        /// En caso de no tener un campo tipo identity, devuelve 0
        /// En caso de error lanza una excepcion del tipo DaoModel.
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="paramDbTransaction">Se toma como transaccion activa y se toma la conexion de la transaccion</param>
        /// <exception cref="DaoModelException">Encapsula la excepcion original</exception>
        /// <returns></returns>
        public long Add(TEntity entity, IDbTransaction paramDbTransaction)
        {
            return Add(entity, null, paramDbTransaction);
        }

        /// <summary>
        /// 
        /// Agrega una entidad del tipo <T>
        /// 
        /// En caso de exito devuelve la identidad insertada.
        /// En caso de no tener un campo tipo identity, devuelve 0
        /// En caso de error lanza una excepcion del tipo DaoModel.
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="paramDbConnection">Esta conexion no se cerrará en el método</param>
        /// <param name="paramDbTransaction">Se toma como transaccion activa </param>
        /// <exception cref="DaoModelException">Encapsula la excepcion original</exception>
        /// <returns></returns>
        public long Add(TEntity entity, IDbConnection paramDbConnection,IDbTransaction paramDbTransaction)
        {
            long identity = 0;
            bool hasIdentity = false;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
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
                insertQuery.Append(string.Format("INSERT INTO {0} ( {1} ) VALUES ( {2} ) {3}  ", _syntaxProvider.GetTableName(TableName), ColumnsForInsert, parameterList, _syntaxProvider.GetSubQueryGetIdentity()));
                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = insertQuery.ToString();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

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

                }
                catch (Exception exception)
                {
                   throw new DaoModelException(exception, dbCommand);
                }
                finally
                {
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                }
            }

            return identity;
        }

        /// <summary>
        /// 
        /// Recibe una lista de entidades y las inserta.
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="errMessage">Parametro de salida, en caso de error muestra el mensaje de la excepcion</param>
        /// <returns>Devuelve true, si no ocurrieron errores.</returns>
        public bool AddRange(IList<TEntity> entities, out string errMessage)
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
        /// 
        /// Recibe una lista de entidades y las inserta.
        /// En caso de error lanza una excepcion del tipo DaoModel.
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="DaoModelException">Encapsula la excepcion original</exception>
        /// <returns>Devuelve true, si no ocurrieron errores</returns>
        public bool AddRange(IList<TEntity> entities)
        {
            return AddRange(entities, null,null);
        }

        /// <summary>
        /// 
        /// Recibe una lista de entidades y las inserta.
        /// En caso de error lanza una excepcion del tipo DaoModel.
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="paramDbConnection">Esta conexion no se cerrará en el método</param>
        /// <exception cref="DaoModelException">Encapsula la excepcion original</exception>
        /// <returns>Devuelve true, si no ocurrieron errores</returns>
        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection)
        {
            return AddRange(entities, paramDbConnection, null);
        }

        /// <summary>
        /// Recibe una lista de entidades y las inserta
        /// 
        /// Devuelve true, si no ocurrieron errores
        /// En caso de error lanza una excepcion del tipo DaoModel.
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="paramDbTransaction">Se toma como transaccion activa y se toma la conexion de la transaccion</param>
        /// <exception cref="DaoModelException">Encapsula la excepcion original</exception>
        /// <returns></returns>
        public bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction)
        {
            return AddRange(entities, null, paramDbTransaction);
        }

        /// <summary>
        /// Recibe una lista de entidades y las inserta
        /// 
        /// Devuelve true, si no ocurrieron errores
        /// En caso de error lanza una excepcion del tipo DaoModel.
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="paramDbConnection">Esta conexion no se cerrará en el método</param>
        /// <param name="paramDbTransaction">Se toma como transaccion activa </param>
        /// <exception cref="DaoModelException">Encapsula la excepcion original</exception>
        /// <returns></returns>
        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return AddRange(entities, paramDbConnection, paramDbTransaction, out long recordsAffected);
        }
        /// <summary>
        /// Recibe una lista de entidades y las inserta
        /// 
        /// Devuelve true, si no ocurrieron errores
        /// En caso de error lanza una excepcion del tipo DaoModel.
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="paramDbConnection">Esta conexion no se cerrará en el método</param>
        /// <param name="paramDbTransaction">Se toma como transaccion activa </param>
        /// <param name="recordsAffected">Devuelve la cantidad de registros que se actualizaron</param>
        /// <exception cref="DaoModelException">Encapsula la excepcion original</exception>
        /// <returns></returns>
        public bool AddRange(IList<TEntity> entities,IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
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

            foreach (TEntity entity in entities)
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
                    insertRangeQuery = (string.Format("INSERT INTO {0} ( {1} ) VALUES ( {2} );", _syntaxProvider.GetTableName(TableName), ColumnsForInsert, parameterList));
                }

                try
                {
                    dbCommand.CommandText = insertRangeQuery;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    recordsAffected += dbCommand.ExecuteNonQuery();
                }
                catch (Exception commandException)
                {
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }

                    throw new DaoModelException(commandException, dbCommand);
                }
            }

            if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool Update(TEntity entity, out string errMessage)
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
        public bool Update(TEntity entity)
        {
            return Update(entity, null, null);
        }
        public bool Update(TEntity entity, IDbConnection paramDbConnection)
        {
            return Update(entity, paramDbConnection, null);
        }
        public bool Update(TEntity entity, IDbTransaction paramDbTransaction)
        {
            return Update(entity, null, paramDbTransaction);
        }
        public bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return Update(entity, paramDbConnection, paramDbTransaction, out long recordsAffected);
        }
        public bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
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
                    parameterList.Append(string.Format("{0} = {1},", _syntaxProvider.GetColumnName(i.Name), parameterName));
                }
                else
                {
                    hasPrimary = true;

                    if (where.ToString() != String.Empty)
                        where.Append(" AND ");

                    where.Append(string.Format("({0} = {1})", _syntaxProvider.GetColumnName(i.Name), parameterName));
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
                updateQuery.Append(string.Format("UPDATE  {0} SET {1} WHERE {2}", _syntaxProvider.GetTableName(TableName), parameterList, where));

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = updateQuery.ToString();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected = dbCommand.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
                finally
                {
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                }
            }
            return true;
        }
        public bool UpdateRange(IList<TEntity> entities, out string errMessage)
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
        public bool UpdateRange(IList<TEntity> entities)
        {
            return UpdateRange(entities, null,null);
        }

        private bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection)
        {
            return UpdateRange(entities, paramDbConnection, null);
        }
        private bool UpdateRange(IList<TEntity> entities, IDbTransaction paramDbTransaction)
        {
            return UpdateRange(entities, null, paramDbTransaction);
        }
        private bool UpdateRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return UpdateRange(entities, paramDbConnection, paramDbTransaction, out long recordsAffected);
        }

        private bool UpdateRange(IList<TEntity> entities,IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
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

            foreach (TEntity entity in entities)
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
                        parameterList.Append(string.Format("{0} = {1},", _syntaxProvider.GetColumnName(i.Name), parameterName));
                    }
                    else
                    {
                        if (where.ToString() != String.Empty)
                            where.Append(" AND ");

                        where.Append(string.Format("({0} = {1})", _syntaxProvider.GetColumnName(i.Name), parameterName));
                    }
                    ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                }

                if (parameterList.ToString() != string.Empty)
                {
                    parameterList.Remove(parameterList.Length - 1, 1);
                    updateRangeQuery.AppendLine(string.Format("UPDATE {0} SET {1} WHERE {2}; ", _syntaxProvider.GetTableName(TableName), parameterList, where));
                }

                try
                {
                    dbCommand.CommandText = updateRangeQuery.ToString();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected += dbCommand.ExecuteNonQuery();
                }
                catch(Exception exceptionCommand)
                {
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                    throw new DaoModelException(exceptionCommand, dbCommand);
                }
            }

            if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return true;
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
        public bool DeleteById(long Id)
        {
            return DeleteById(Id, null, null);
        }
        public bool DeleteById(long Id, IDbConnection paramDbConnection)
        {
            return DeleteById(Id, paramDbConnection, null);
        }
        public bool DeleteById(long Id, IDbTransaction paramDbTransaction)
        {
            return DeleteById(Id, null, paramDbTransaction);
        }
        public bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return DeleteById(Id, paramDbConnection, paramDbTransaction, out long recordsAffected);
        }
        public bool DeleteById(long Id, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            TEntity entity = new TEntity();
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

                    where.Append(string.Format("({0} = {1})", _syntaxProvider.GetColumnName(i.Name), parameterName));

                    ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, Id, cdbi.MaxLenght));

                }
            }

            if (where.ToString() != string.Empty)
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(string.Format("DELETE FROM {0} WHERE {1}", _syntaxProvider.GetTableName(TableName), where));

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = deleteQuery.ToString();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected = dbCommand.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
                finally
                {
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                }
            }

            return true;
        }
        public bool DeleteAll(out string errMessage)
        {
            bool result = false;
            errMessage = "";

            try
            {
                DeleteAll();
                result = true;
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
        public long DeleteAll()
        {
            return DeleteAll(null, null);
        }
        public long DeleteAll(IDbConnection paramDbConnection)
        {
            return DeleteAll(paramDbConnection, null);
        }
        public long DeleteAll(IDbTransaction paramDbTransaction)
        {
            return DeleteAll(null, paramDbTransaction);
        }

        public long DeleteAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            long recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));
            
            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            if (paramDbTransaction != null)
                dbCommand.Transaction = paramDbTransaction;

            dbCommand.CommandText = string.Format("DELETE FROM {0}", _syntaxProvider.GetTableName(TableName));

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                recordsAffected = dbCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }
            finally
            {
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }

            return recordsAffected;
        }











        public TEntity GetById(object id)
        {
            return GetById(id, null,null);
        }
        public TEntity GetById(object id, IDbConnection paramDbConnection)
        {
            return GetById(id,paramDbConnection, null);
        }
        public TEntity GetById(object id, IDbTransaction paramDbTransaction)
        {
            return GetById(id, null, paramDbTransaction);
        }
        public TEntity GetById(object id,IDbConnection paramDbConnection,IDbTransaction paramDbTransaction)
        {
            TEntity entity = new TEntity();

            StringBuilder clause = new StringBuilder();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            if (paramDbTransaction != null)
                dbCommand.Transaction = paramDbTransaction;

            foreach (var pi in ListObjectPropertyInfo)
            {
                var pk = pi.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (pk != null && pk.IsPrimary)
                {
                    string parameterName = "@param_" + pi.Name;
                    clause.Append(string.Format("{0}={1}", _syntaxProvider.GetColumnName(pi.Name), id));

                    ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[pi.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, id, cdbi.MaxLenght));

                    break;
                }
            }

            entity = null;
            if (clause.ToString() != string.Empty)
            {
                StringBuilder getQuery = new StringBuilder();
                getQuery.Append(string.Format("SELECT {0} FROM {1} WHERE {2}", ColumnsForSelect, _syntaxProvider.GetTableName(TableName), clause));

                dbCommand.CommandText = getQuery.ToString();

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                    if (reader.Read())
                        entity = new IDataReaderToEntity().MapSingle<TEntity>(reader, ListObjectPropertyInfo);


                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, dbCommand);
                }
                finally
                {
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                }
            }

            return entity;
        }
        /// <summary>
        /// Obtiene todas las entidades
        /// </summary>
        /// <returns></returns>
        public IList<TEntity> GetAll()
        {
            return GetAll(null,null);
        }
        public IList<TEntity> GetAll(IDbConnection paramDbConnection)
        {
            return GetAll(paramDbConnection, null);
        }
        public IList<TEntity> GetAll(IDbTransaction paramDbTransaction)
        {
            return GetAll(null, paramDbTransaction);
        }
        /// <summary>
        /// Obtiene todas las entidades, recibiendo una conexion externa la cual no se 
        /// cerrará al finalizar el metodo.
        /// </summary>
        /// <param name="paramDbConnection"></param>
        /// <returns></returns>
        public IList<TEntity> GetAll(IDbConnection paramDbConnection,IDbTransaction paramDbTransaction)
        {
            IList<TEntity> collection = new List<TEntity>();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            if (paramDbTransaction != null)
                dbCommand.Transaction = paramDbTransaction;

            dbCommand.CommandText = string.Format("SELECT {0} FROM {1}", ColumnsForSelect, _syntaxProvider.GetTableName(TableName));

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                collection = new IDataReaderToEntity().Map<TEntity>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }
            finally
            {
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }

            return collection;
        }
        /// <summary>
        /// Obtiene una lista filtrada por una expresion LAMBDA
        /// </summary>
        /// <param name="lambdaExpressionFilter"></param>
        /// <returns></returns>
        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, null,null);
        }
        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection dbConnection)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, dbConnection, null);
        }
        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction)
        {
            return GetListByLambdaExpressionFilter(lambdaExpressionFilter, null, paramDbTransaction);
        }

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection,IDbTransaction paramDbTransaction)
        {
            IList<TEntity> resultado = new List<TEntity>();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            if (paramDbTransaction != null)
                dbCommand.Transaction = paramDbTransaction;

            try
            {
                if(dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dbCommand.CommandText = query.QueryString;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                resultado = new IDataReaderToEntity().Map<TEntity>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);

            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, dbCommand);
            }
            finally
            {
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }

            return resultado;
        }
        protected DbExecute GetDbExecute()
        {
            return new DbExecute(ConnectionString, _dataBaseEngine);
        }

        protected Dictionary<string, object> CreateQueryParametersList()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            return parameters;
        }

        public IList<TEntity> Find(IEnumerable<object> ids)
        {
            return Find(ids, null, null);
        }
        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection dbConnection)
        {
            return Find(ids, dbConnection, null);
        }
        public IList<TEntity> Find(IEnumerable<object> ids, IDbTransaction paramDbTransaction)
        {
            return Find(ids, null, paramDbTransaction);
        }
        public IList<TEntity> Find(IEnumerable<object> ids, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            IList<TEntity> entities = new List<TEntity>();
            StringBuilder clause = new StringBuilder();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
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

                    clause.Append(string.Format("{0} IN ({1})", _syntaxProvider.GetColumnName(pi.Name), _ids));
                    break;
                }
            }

            if (clause.ToString() != string.Empty)
            {
                StringBuilder findQuery = new StringBuilder();
                findQuery.Append(string.Format("SELECT {0} FROM {1} WHERE {2}", ColumnsForSelect, _syntaxProvider.GetTableName(TableName), clause));

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = findQuery.ToString();
                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    entities = new IDataReaderToEntity().Map<TEntity>(dbCommand.ExecuteReader(), ListObjectPropertyInfo);

                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception,dbCommand);
                }
                finally
                {
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
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
            return _dataObjectProvider.GetIDbConnection(ConnectionString);
        }
        /// <summary>
        /// Devuelve un *DbConnection*, dependiendo del motor de base de datos del DAO
        /// </summary>
        /// <returns></returns>
        protected DbConnection GetDbConnection()
        {
            return _dataObjectProvider.GetDbConnection(ConnectionString);
        }
        
        /// <summary>
        /// Devuelve un IDbParameter, dependiendo del motor de base de datos del DAO
        /// </summary>
        /// <returns></returns>
        protected IDbDataParameter GetIDbDataParameter()
        {
            return _dataObjectProvider.GetIDbDataParameter();
        }

        /// <summary>
        /// Obtiene la lista de Columnas para INSERT (No incluye las columnas con atributo IsIdentity) 
        /// Obtiene la lista de Columnas para SELECT.
        /// Obtiene atributos de las columnas en la base de datos (Tipo y Longitud) 
        /// </summary>
        private void GetDbColumnsAndAtributes()
        {
            TEntity entity = new TEntity();

            StringBuilder sbColumnsInsert = new StringBuilder();
            StringBuilder sbColumnsSelect = new StringBuilder();

            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;

                if (!customAttribute.IsIdentity)
                    sbColumnsInsert.Append(string.Format("{0},", _syntaxProvider.GetColumnName(i.Name)));

                sbColumnsSelect.Append(string.Format("{0},", _syntaxProvider.GetColumnName(i.Name)));

                ColumnListWithDbInfo.Add(i.Name, GetColumnDbInfo(i.Name));
            }

            if (sbColumnsInsert.Length > 0)
                sbColumnsInsert.Remove(sbColumnsInsert.Length - 1, 1);

            if (sbColumnsSelect.Length > 0)
                sbColumnsSelect.Remove(sbColumnsSelect.Length - 1, 1);

            ColumnsForInsert = sbColumnsInsert.ToString();
            ColumnsForSelect = sbColumnsSelect.ToString();
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
            { "mediumtext", DbType.String },
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
                if (_dataBaseEngine == EDataBaseEngine.OleDb || _dataBaseEngine == EDataBaseEngine.MySql)
                    columnProperty = dbConnection.GetSchema("Columns", new[] { null, null, TableName, columnName });
                else
                    columnProperty = dbConnection.GetSchema("Columns", new[] { dbConnection.Database, null, TableName,columnName});

                DataRow dataRow;
                try
                {
                    dataRow = columnProperty.Rows[0];
                }
                catch (Exception)
                {
                    throw;
                }

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
            dataParameter.Value = value ?? (Object)DBNull.Value;

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
            dataParameter.Value = value ?? (Object)DBNull.Value;

            if (size != null && size !=0)
                dataParameter.Size = (int)size;

            return dataParameter;
        }

        private IList<PropertyInfo> GetPropertyInfoList(TEntity entity)
        {
            return entity.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();
        }

        protected long ParseToLong(object result)
        {
            if (result.GetType() == typeof(System.Int64))
                return (long)result;
            if (result.GetType() == typeof(System.Int32))
                return (long)(Int32)result;
            else if (result.GetType() == typeof(System.Decimal))
                return (long)(Decimal)result;
            else 
                return long.Parse(result.ToString());
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
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="reader"></param>
        /// <param name="listObjectPropertyInfo"></param>
        /// <returns></returns>
        public TEntity MapSingle<TEntity>(IDataReader reader) where TEntity : class, new()
        {
            return MapSingle<TEntity>(reader, null);
        }

        public TEntity MapSingle<TEntity>(IDataReader reader, IList<PropertyInfo> listObjectPropertyInfo) where TEntity : class, new()
        {
            TEntity obj = new TEntity();

            IList<PropertyInfo> _propertyInfo;

            if (listObjectPropertyInfo != null)
                _propertyInfo = listObjectPropertyInfo;
            else
                _propertyInfo = obj.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();

            obj = new TEntity();

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
        public TEntity MapSingle<TEntity>(IDataRecord dataRecord) where TEntity : class, new()
        {
            return MapSingle<TEntity>(dataRecord, null);
        }
        public TEntity MapSingle<TEntity>(IDataRecord dataRecord, IList<PropertyInfo> listObjectPropertyInfo) where TEntity : class, new()
        {
            TEntity obj = new TEntity();

            IList<PropertyInfo> _propertyInfo;

            if (listObjectPropertyInfo != null)
                _propertyInfo = listObjectPropertyInfo;
            else
                _propertyInfo = obj.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();

            obj = new TEntity();

            foreach (PropertyInfo i in _propertyInfo)
            {
                try
                {
                    var custumAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute));

                    if (((DbColumnAttribute)custumAttribute).Convert == true)
                    {
                        if (dataRecord[i.Name] != DBNull.Value)
                            i.SetValue(obj, Convert.ChangeType(dataRecord[i.Name], i.PropertyType));
                    }
                    else
                    {
                        if (dataRecord[i.Name] != DBNull.Value)
                        {
                            var value = dataRecord[i.Name];
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
}

/*
public object ExecScalar(string QueryString, IDictionary<string, object> QueryParameters)
{
    return ExecScalar(QueryString, QueryParameters, null, null);
}

public object ExecScalar(string QueryString, IDictionary<string, object> QueryParameters, DbConnection paramDbConnection)
{
    return ExecScalar(QueryString, QueryParameters, paramDbConnection, null);
}
public object ExecScalar(string QueryString, IDictionary<string, object> QueryParameters, DbTransaction paramDbTransaction)
{
    return ExecScalar(QueryString, QueryParameters, null, paramDbTransaction);
}
public object ExecScalar(string QueryString, IDictionary<string, object> QueryParameters, DbConnection paramDbConnection, DbTransaction paramDbTransaction)
{
    bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

    if (paramDbConnection == null && paramDbTransaction != null)
        paramDbConnection = paramDbTransaction.Connection;

    DbConnection dbConnection = paramDbConnection ?? GetDbConnection();
    DbCommand dbCommand = dbConnection.CreateCommand();

    object resultado;
    try
    {
        if (dbConnection.State != ConnectionState.Open)
            dbConnection.Open();

        dbCommand.CommandText = QueryString;

        if (paramDbTransaction != null)
            dbCommand.Transaction = paramDbTransaction;

        foreach (var parameters in QueryParameters)
            dbCommand.Parameters.Add(CreateIDbDataParameter((parameters.Key.StartsWith("@") ? "" : "@") + parameters.Key, parameters.Value));

        if (_dataBaseEngine == EDataBaseEngine.OleDb)
            ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

        resultado = dbCommand.ExecuteScalar();
    }
    catch (Exception exception)
    {
        throw new DaoModelException(exception, dbCommand);
    }
    finally
    {
        if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
        {
            dbConnection.Close();
            dbConnection.Dispose();
        }
    }

    return resultado;
}
*/
/*
public long ExecNonQuery(string QueryString)
{
    return ExecNonQuery(QueryString, null, null, null);
}
public long ExecNonQuery(string QueryString, DbConnection paramDbConnection)
{
    return ExecNonQuery(QueryString, null, paramDbConnection, null);
}
public long ExecNonQuery(string QueryString, IDictionary<string, object> QueryParameters)
{
    return ExecNonQuery(QueryString, QueryParameters, null, null);
}
public long ExecNonQuery(string QueryString, IDictionary<string, object> QueryParameters, DbConnection paramDbConnection)
{
    return ExecNonQuery(QueryString, QueryParameters, paramDbConnection, null);
}
public long ExecNonQuery(string QueryString, DbTransaction paramDbTransaction)
{
    return ExecNonQuery(QueryString, null, null, paramDbTransaction);
}
public long ExecNonQuery(string QueryString, IDictionary<string, object> QueryParameters, DbTransaction paramDbTransaction)
{
    return ExecNonQuery(QueryString, QueryParameters, null, paramDbTransaction);
}
public long ExecNonQuery(string QueryString, IDictionary<string, object> QueryParameters, DbConnection paramDbConnection, DbTransaction paramDbTransaction)
{
    bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

    if (paramDbConnection == null && paramDbTransaction != null)
        paramDbConnection = (DbConnection)paramDbTransaction.Connection;

    DbConnection dbConnection = paramDbConnection ?? GetDbConnection();
    DbCommand dbCommand= dbConnection.CreateCommand();

    long resultado;
    try
    {
        if (dbConnection.State != ConnectionState.Open)
            dbConnection.Open();

        dbCommand.CommandText = QueryString;

        if (paramDbTransaction != null)
            dbCommand.Transaction = paramDbTransaction;

        if (QueryParameters != null)
        {
            foreach (var parameters in QueryParameters)
                dbCommand.Parameters.Add(CreateIDbDataParameter((parameters.Key.StartsWith("@") ? "" : "@") + parameters.Key, parameters.Value));

            if (_dataBaseEngine == EDataBaseEngine.OleDb)
                ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

        }

        resultado = dbCommand.ExecuteNonQuery();

    }
    catch (Exception exception)
    {
        throw new DaoModelException(exception, dbCommand);
    }
    finally
    {
        if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
        {
            dbConnection.Close();
            dbConnection.Dispose();
        }
    }

    return resultado;
}
*/


/*
public IEnumerable<T> SelectQuery(string QueryString, IDictionary<string, object> QueryParameters)
{
    return SelectQuery(QueryString, QueryParameters, null, null);
}
public IEnumerable<T> SelectQuery(string QueryString, IDictionary<string, object> QueryParameters, DbConnection paramDbConnection)
{
    return SelectQuery(QueryString, QueryParameters, paramDbConnection, null);
}
public IEnumerable<T> SelectQuery(string QueryString, IDictionary<string, object> QueryParameters, DbTransaction paramDbTransaction)
{
    return SelectQuery(QueryString, QueryParameters, null, paramDbTransaction);
}

public IEnumerable<T> SelectQuery(string QueryString, IDictionary<string, object> QueryParameters,DbConnection paramDbConnection, DbTransaction paramDbTransaction)
{
    bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

    if (paramDbConnection == null && paramDbTransaction != null)
        paramDbConnection = paramDbTransaction.Connection;

    DbConnection dbConnection = paramDbConnection ?? GetDbConnection();
    if (dbConnection.State != ConnectionState.Open)
        dbConnection.Open();

    DbCommand dbCommand = dbConnection.CreateCommand();
    dbCommand.CommandText = QueryString;


    if (paramDbTransaction != null)
        dbCommand.Transaction = paramDbTransaction;

    foreach (var parameters in QueryParameters)
        dbCommand.Parameters.Add(CreateIDbDataParameter((parameters.Key.StartsWith("@")?"":"@") + parameters.Key, parameters.Value));

    foreach (IDataRecord dataRecord in dbCommand.ExecuteReader(closeConnection ? CommandBehavior.CloseConnection : CommandBehavior.Default))
        yield return new IDataReaderToEntity().MapSingle<T>(dataRecord, ListObjectPropertyInfo);
}
*/
