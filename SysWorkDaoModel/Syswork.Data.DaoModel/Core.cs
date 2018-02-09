using SysWork.Data.DaoModel.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SysWork.Data.DaoModel.LambdaSqlBuilder;

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
    /// </summary>
    /// <typeparam name="T"> Clase e Instanciable T</typeparam>
    internal class ColumnDbInfo
    {
        internal SqlDbType SqlDbType { get; set; }
        internal Int32? MaxLenght { get; set; }
    }


    /// <summary>
    /// Clase Abstracta, debe implementarse en una clase.
    /// Usar con clases Entity/Model solamente
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDao<T> where T : class, new()
    {
        public string ConnectionString { get; private set; }

        private string TableName;

        private Hashtable ColumnListWithDbInfo = new Hashtable();

        /// <summary>
        /// Contiene la lista de propiedades de la entidad vinculadas a la DB.
        /// UPDATE: Cambio Private Por Public
        /// </summary>
        public IList<PropertyInfo> ListObjectPropertyInfo;

        /// <summary>
        /// Devuelve las columnas necesarias para hacer un Insert (No incluye las columnas Identity)
        /// </summary>
        public String ColumnsForInsert { get; private set; }

        /// <summary>
        /// Devuelve las columnas de la entidad.
        /// </summary>
        public String ColumnsForSelect { get; private set; }

        /// <summary>
        /// Crea una nueva instancia de BaseDao, sin pasar como parametro el nombre de la tabla a representar
        /// </summary>
        /// <param name="connectionString">Representa la cadena de conexion</param>
        public BaseDao(string ConnectionString)
        {
            T t = new T();
            ListObjectPropertyInfo = GetPropertyInfoList(t);

            TableName = GetTableName(t.GetType());

            if ((ListObjectPropertyInfo == null) || (ListObjectPropertyInfo.Count == 0))
            {
                throw new Exception(string.Format("La entidad {0}, no tiene atributos vinculados a la tabla: {1}, Utilice el decorador DbColumn para vincular las propiedades a los campos de la tabla", t.GetType().Name, TableName));
            }

            this.ConnectionString = ConnectionString;
            GetDbColumnsAndAtributes();
        }

        private static string GetTableName(Type type)
        {
            var column = type.GetCustomAttributes(false).OfType<DbTableAttribute>().FirstOrDefault();
            if (column != null)
                return column.Name;
            else
                return type.Name;
        }

        /// <summary>
        /// Devuelve un Objeto SqlConnection
        /// </summary>
        /// <returns>SqlConnection object</returns>
        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Devuelve un SQLCommand con la conexion abierta
        /// </summary>
        /// <returns>SqlConnection object</returns>
        public SqlCommand GetSqlCommand(string commandText, SqlConnection sqlConnection)
        {
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(commandText);
            sqlCommand.Connection = sqlConnection;

            return sqlCommand;
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

            SqlCommand sqlCommand = new SqlCommand();

            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttibute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (!customAttibute.IsIdentity)
                {
                    string parameterName = "@param_" + i.Name;

                    parameterList.Append(string.Format("{0},", parameterName));

                    ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[i.Name];

                    sqlCommand.Parameters.Add(CreateSqlParameter(parameterName, cdbi.SqlDbType, i.GetValue(entity), cdbi.MaxLenght));
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
                insertQuery.Append(string.Format("INSERT INTO [{0}] ( {1} ) VALUES ( {2} ) SELECT SCOPE_IDENTITY()  ", TableName, ColumnsForInsert, parameterList));
                try
                {
                    using (SqlConnection sqlConnection = GetSqlConnection())
                    {
                        sqlConnection.Open();

                        sqlCommand.CommandText = insertQuery.ToString();
                        sqlCommand.Connection = sqlConnection;

                        if (hasIdentity)
                        {

                            identity = (long)(Decimal)sqlCommand.ExecuteScalar();
                        }
                        else
                        {
                            sqlCommand.ExecuteNonQuery();
                            identity = 0;
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, sqlCommand);
                }
            }

            return identity;
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
        /// la cual contiene informacion extra como sel el SqlCommand que la causo y la 
        /// excepcion original.
        /// </summary>
        /// <param name="entities"></param>

        public bool AddRange(IList<T> entities)
        {

            StringBuilder insertRangeQuery = new StringBuilder();
            int paramNro = 0;

            SqlCommand sqlCommand = new SqlCommand();

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

                        sqlCommand.Parameters.Add(CreateSqlParameter(parameterName, cdbi.SqlDbType, i.GetValue(entity), cdbi.MaxLenght));
                    }
                }

                if (parameterList.ToString() != string.Empty)
                {
                    parameterList.Remove(parameterList.Length - 1, 1);

                    insertRangeQuery.Append(string.Format("INSERT INTO [{0}] ( {1} ) VALUES ( {2} )", TableName, ColumnsForInsert, parameterList));
                }

                paramNro++;
            }

            try
            {
                using (SqlConnection sqlConnection = GetSqlConnection())
                {
                    sqlConnection.Open();
                    sqlCommand.CommandText = insertRangeQuery.ToString();
                    sqlCommand.Connection = sqlConnection;

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, sqlCommand);
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

            SqlCommand sqlCommand = new SqlCommand();

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
                sqlCommand.Parameters.Add(CreateSqlParameter(parameterName, cdbi.SqlDbType, i.GetValue(entity), cdbi.MaxLenght));
            }

            if (!hasPrimary)
            {
                throw new ArgumentException("No hay una clave primaria definida en la entidad, al menos un atributo debe tener la propiedad IsPrimary = True");
            }

            if (parameterList.ToString() != string.Empty)
            {
                parameterList.Remove(parameterList.Length - 1, 1);
                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append(string.Format("UPDATE [{0}] SET {1} WHERE {2}", TableName, parameterList, where));

                try
                {
                    using (SqlConnection sqlConnection = GetSqlConnection())
                    {
                        sqlConnection.Open();

                        sqlCommand.CommandText = updateQuery.ToString();
                        sqlCommand.Connection = sqlConnection;

                        sqlCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, sqlCommand);
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

            StringBuilder updateRangeQuery = new StringBuilder();
            int paramNro = 0;

            SqlCommand sqlCommand = new SqlCommand();

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
                    sqlCommand.Parameters.Add(CreateSqlParameter(parameterName, cdbi.SqlDbType, i.GetValue(entity), cdbi.MaxLenght));
                }

                if (parameterList.ToString() != string.Empty)
                {
                    parameterList.Remove(parameterList.Length - 1, 1);

                    updateRangeQuery.AppendLine(string.Format("UPDATE [{0}] SET {1} WHERE {2} ", TableName, parameterList, where));
                }

                paramNro++;
            }
            try
            {
                using (SqlConnection sqlConnection = GetSqlConnection())
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = updateRangeQuery.ToString();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, sqlCommand);
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

            SqlCommand sqlCommand = new SqlCommand();

            foreach (var pi in ListObjectPropertyInfo)
            {
                var pk = pi.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (pk != null && pk.IsPrimary)
                {
                    string parameterName = "@param_" + pi.Name;
                    clause.Append(string.Format("[{0}]={1}", pi.Name, id));

                    ColumnDbInfo cdbi = (ColumnDbInfo)ColumnListWithDbInfo[pi.Name];
                    sqlCommand.Parameters.Add(CreateSqlParameter(parameterName, cdbi.SqlDbType, id, cdbi.MaxLenght));
                    break;
                }
            }

            entity = null;

            if (clause.ToString() != string.Empty)
            {
                StringBuilder getQuery = new StringBuilder();
                getQuery.Append(string.Format("SELECT {0} FROM [{1}] WHERE {2}", ColumnsForSelect, TableName, clause));

                sqlCommand.CommandText = getQuery.ToString();

                try
                {
                    using (SqlConnection sqlConnection = GetSqlConnection())
                    {
                        sqlConnection.Open();
                        sqlCommand.Connection = sqlConnection;

                        var _entities = new SqlDataReaderToEntity().Map<T>(sqlCommand.ExecuteReader(), ListObjectPropertyInfo);

                        if (_entities != null && _entities.Count > 0)
                            entity = _entities[0];
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, sqlCommand);
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

            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.CommandText = string.Format("SELECT {0} FROM [{1}]", ColumnsForSelect, TableName);

            try
            {
                using (SqlConnection sqlConnection = GetSqlConnection())
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;

                    collection = new SqlDataReaderToEntity().Map<T>(sqlCommand.ExecuteReader(), ListObjectPropertyInfo);
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, sqlCommand);
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

            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                using (SqlConnection sqlConnection = GetSqlConnection())
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = query.QueryString;

                    foreach (var parameters in query.QueryParameters)
                        sqlCommand.Parameters.AddWithValue(parameters.Key, parameters.Value);

                    collection = new SqlDataReaderToEntity().Map<T>(sqlCommand.ExecuteReader(), ListObjectPropertyInfo);
                }
            }
            catch (Exception exception)
            {
                throw new DaoModelException(exception, sqlCommand);
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

                SqlCommand sqlCommand = new SqlCommand();
                using (SqlConnection sqlConnection = GetSqlConnection())
                {
                    sqlConnection.Open();
                    sqlCommand.Connection = sqlConnection;

                    entities = new SqlDataReaderToEntity().Map<T>(sqlCommand.ExecuteReader(), ListObjectPropertyInfo);
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
            SqlCommand sqlCommand = new SqlCommand();

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
                    sqlCommand.Parameters.Add(CreateSqlParameter(parameterName, cdbi.SqlDbType, Id, cdbi.MaxLenght));
                }
            }

            if (where.ToString() != string.Empty)
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(string.Format("DELETE [{0}] WHERE {1}", TableName, where));

                try
                {
                    using (SqlConnection sqlConnection = GetSqlConnection())
                    {
                        sqlConnection.Open();

                        sqlCommand.CommandText = deleteQuery.ToString();
                        sqlCommand.Connection = sqlConnection;

                        sqlCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    throw new DaoModelException(exception, sqlCommand);
                }
            }

            return true;
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
        /// <summary>
        /// Obtiene atributos de las columnas en la base de datos (Tipo y Longitud) 
        /// </summary>
        /// <param name="columnName"></param>
        private ColumnDbInfo GetColumnDbInfo(string columnName)
        {
            ColumnDbInfo columnData = new ColumnDbInfo();

            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();

                string commandText = " SELECT DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + this.TableName + "' AND COLUMN_NAME = '" + columnName + "' ORDER BY ORDINAL_POSITION";

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = commandText;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    columnData.SqlDbType = SqlDataTypeToSqlDbTypeEnumerator(sqlDataReader.GetString(0));

                    if (sqlDataReader.IsDBNull(1))
                        columnData.MaxLenght = null;
                    else
                        columnData.MaxLenght = sqlDataReader.GetInt32(1);
                }
            }
            return columnData;
        }
        private SqlParameter CreateSqlParameter(string parameterName, SqlDbType sqlDbType, object value, Int32? size = null)
        {
            SqlParameter sqlParameter = new SqlParameter();

            sqlParameter.ParameterName = parameterName;
            sqlParameter.SqlDbType = sqlDbType;
            sqlParameter.Value = value == null ? (Object)DBNull.Value : value;

            if (size != null)
                sqlParameter.Size = (int)size;

            return sqlParameter;
        }
        /// <summary>
        /// Dado un string con el nombre de un tipo de datos SQL, devuelve el Enumerador del mismo. 
        /// </summary>
        /// <param name="sqlDbDataType"></param>
        /// <exception cref="ArgumentException"></exception>
        public SqlDbType SqlDataTypeToSqlDbTypeEnumerator(string sqlDbDataType)
        {
            SqlDbType sqlDbType;

            switch (sqlDbDataType)
            {
                case "bigint":
                    sqlDbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    sqlDbType = SqlDbType.VarBinary;
                    break;
                case "bit":
                    sqlDbType = SqlDbType.Bit;
                    break;
                case "char":
                    sqlDbType = SqlDbType.Char;
                    break;
                case "date":
                    sqlDbType = SqlDbType.Date;
                    break;
                case "datetime":
                    sqlDbType = SqlDbType.DateTime;
                    break;
                case "datetime2":
                    sqlDbType = SqlDbType.DateTime2;
                    break;
                case "datetimeoffset":
                    sqlDbType = SqlDbType.DateTimeOffset;
                    break;
                case "decimal":
                    sqlDbType = SqlDbType.Decimal;
                    break;
                case "float":
                    sqlDbType = SqlDbType.Float;
                    break;
                case "image":
                    sqlDbType = SqlDbType.Binary;
                    break;
                case "int":
                    sqlDbType = SqlDbType.Int;
                    break;
                case "money":
                    sqlDbType = SqlDbType.Money;
                    break;
                case "nchar":
                    sqlDbType = SqlDbType.NChar;
                    break;
                case "ntext":
                    sqlDbType = SqlDbType.NText;
                    break;
                case "numeric":
                    sqlDbType = SqlDbType.Decimal;
                    break;
                case "nvarchar":
                    sqlDbType = SqlDbType.NVarChar;
                    break;
                case "real":
                    sqlDbType = SqlDbType.Real;
                    break;
                case "rowversion":
                    sqlDbType = SqlDbType.Timestamp;
                    break;
                case "smalldatetime":
                    sqlDbType = SqlDbType.DateTime;
                    break;
                case "smallint":
                    sqlDbType = SqlDbType.SmallInt;
                    break;
                case "smallmoney":
                    sqlDbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    sqlDbType = SqlDbType.Variant;
                    break;
                case "text":
                    sqlDbType = SqlDbType.Text;
                    break;
                case "time":
                    sqlDbType = SqlDbType.Time;
                    break;
                case "timestamp":
                    sqlDbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    sqlDbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    sqlDbType = SqlDbType.UniqueIdentifier;
                    break;
                case "varbinary":
                    sqlDbType = SqlDbType.VarBinary;
                    break;
                case "varchar":
                    sqlDbType = SqlDbType.VarChar;
                    break;
                case "xml":
                    sqlDbType = SqlDbType.Xml;
                    break;
                default:
                    throw new ArgumentException("El tipo de Datos SQL no es reconocido por el parser");
            }
            return sqlDbType;
        }
        private IList<PropertyInfo> GetPropertyInfoList(T entity)
        {
            return entity.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbColumnAttribute)) != null).ToList();
        }
    }
    /// <summary>
    /// Author : Diego Martinez
    /// Email : dmartinez@syswork.com.ar
    /// Date : 07/12/2017
    /// Description : Funciona como un Mapper, dado un SqlDataReader y el Tipo de Objeto 
    ///               devuelve una coleccion de los que existan
    /// 
    /// UPDATES:
    /// =======
    /// 08/12/2017: Por un tema de Performance, puede recibir como parametro una lista de las
    ///             propiedades del objeto, para no tener que usar Reflexion en cada una de las
    ///             llamadas
    ///             
    /// </summary>
    public class SqlDataReaderToEntity
    {
        /// <summary>
        /// Mapea una entidad desde un SqlDataReader
        /// </summary>
        /// <param name="reader">Representa la cadena de conexion</param>
        /// <typeparam name="T">Entidad a mapear,solo los atributos que posean el decorador DbColumn seran tenidos en cuenta</typeparam>
        public IList<T> Map<T>(SqlDataReader reader) where T : class, new()
        {
            return Map<T>(reader, null);
        }

        /// <summary>
        /// **OPTIMIZADO** Mapea una entidad desde un SqlDataReader, Ideal para Bucles o multiples iteraciones, donde ya tenemos info del Objeto etc.
        /// </summary>
        /// <param name="reader">Representa la cadena de conexion</param>
        /// <param name="listObjectPropertyInfo">En el caso que se posea la lista de atributos de la clase que esten vinculados a la DB, se pasan como parametro, evitando volver a buscarlos</param>
        /// <typeparam name="T">Entidad a mapear,solo los atributos que posean el decorador DbColumn seran tenidos en cuenta</typeparam>
        public IList<T> Map<T>(SqlDataReader reader, IList<PropertyInfo> listObjectPropertyInfo) where T : class, new()
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

        public T MapSingle<T>(SqlDataReader reader, IList<PropertyInfo> listObjectPropertyInfo) where T : class, new()
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
        public SqlCommand SqlCommand { get; private set; }
        public string OriginalStackTrace { get; private set; }

        public DaoModelException(Exception originalException,SqlCommand sqlCommand):base(originalException.Message)
        {
            this.OriginalException = originalException;
            this.SqlCommand = sqlCommand;
            this.OriginalStackTrace = originalException.StackTrace;
        }

    }
}
