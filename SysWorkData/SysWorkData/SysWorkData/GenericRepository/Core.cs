﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Dictionarys;
using SysWork.Data.Common.LambdaSqlBuilder;
using SysWork.Data.Common.LambdaSqlBuilder.ValueObjects;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Attributes;
using SysWork.Data.GenericRepository.Mapper;
using SysWork.Data.GenericRepostory.Attributes;
using SysWork.Data.GenericRepostory.Exceptions;
using SysWork.Data.GenericRepostory.Interfaces;
using SysWork.Data.Syntax;

namespace SysWork.Data.GenericRepostory
{
    #region DOCUMENTATION Class
    /// <summary>
    /// Abstract class to be partially implemented. It allows CRUD operations to be performed on database 
    /// entities represented by classes (classes must implement DbTableAttribute and DbColumnAttribute attributes). 
    /// Implementing this class allows: 
    /// Add, AddRange, Update, UpdateRange, GetById, GetByLambdaExpressionFilter, GetAll, GetListByLambdaExpressionFilter, 
    /// DeleteById, Find .
    /// <seealso cref="SysWork.Data.GenericRepostory.Attributes"/>
    /// </summary>
    /// <remarks>
    /// This class is multi database engine, see the supported database engines <see cref="Common.EDataBaseEngine"/>. 
    /// 
    /// All its methods, in case of not specifying a connection, create a new one and at the end they close it.
    /// 
    /// In case a transaction is specified, the active connection of the transaction is used.
    /// 
    /// In case a transaction and a connection are specified, the ones provided will be used.
    /// </remarks>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="SysWork.Data.GenericRepostory.Interfaces.IBaseGenericRepository{TEntity}" />
    #endregion
    public abstract class BaseGenericRepository<TEntity>: IBaseGenericRepository<TEntity> where TEntity : class, new() 
    {
        private string _connectionString;
        /// <summary>
        /// Gets the active ConnectionString.
        /// </summary>
        public string ConnectionString{get{return _connectionString;} private set{_connectionString = value;}}

        private EDataBaseEngine _dataBaseEngine;
        /// <summary>
        /// Gets the Database Engine.
        /// </summary>
        /// <seealso cref=" SysWork.Data.Common.EDataBaseEngine"/>
        public EDataBaseEngine DataBaseEngine{get{return _dataBaseEngine;}private set{_dataBaseEngine = value;}}

        private Hashtable _columnListWithDbInfo = new Hashtable();

        /// <summary>
        /// Gets the data object provider.
        /// </summary>
        /// <value>The DbObjectProvider used in this class.</value>
        protected DbObjectProvider DataObjectProvider { get; private set; }
        
        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Get the name of the database table to represent.
        /// </summary>
        public string TableName { get; private set; }
        
        /// <summary>
        /// Get the propertyInfo of the columns.
        /// </summary>
        public IList<PropertyInfo> ListObjectPropertyInfo { get; private set; }

        /// <summary>
        /// Get a list of the columns to perform a INSERT sentence on the represented table, separated by commas. The Identity columns are excluded.
        /// </summary>
        public string ColumnsForInsert { get; private set; }

        /// <summary>
        /// Get a list of the columns to perform a SELECT sentence on the represented table, separated by commas.
        /// </summary>
        public string ColumnsForSelect { get; private set; }

        /// <summary>
        /// Initializes a new instance class. Using MSSqlServer as DataBaseEngine.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public BaseGenericRepository(string connectionString)
        {
            BaseGenericRepositoryConstructorResolver(connectionString, EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Initializes a new instance class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dataBaseEngine">The data base engine.</param>
        public BaseGenericRepository(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            BaseGenericRepositoryConstructorResolver(connectionString, dataBaseEngine);
        }

        private void BaseGenericRepositoryConstructorResolver(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            _connectionString = connectionString;
            _dataBaseEngine = dataBaseEngine;
            DataObjectProvider = new DbObjectProvider(_dataBaseEngine);
            _syntaxProvider = new SyntaxProvider(_dataBaseEngine);

            TEntity entity = new TEntity();
            ListObjectPropertyInfo = GetPropertyInfoList(entity);

            TableName = GetTableNameFromEntity(entity.GetType());

            if ((ListObjectPropertyInfo == null) || (ListObjectPropertyInfo.Count == 0))
            {
                throw new Exception(string.Format("The Entity {0}, has not linked attibutes to table: {1}, Use [DbColumn] attribute to link properties to the table.", entity.GetType().Name, TableName));
            }

            GetDbColumnsAndAtributes();
        }

        private string GetTableNameFromEntity(Type type)
        {
            var table = type.GetCustomAttributes(false).OfType<DbTableAttribute>().FirstOrDefault();
            if (table != null)
                return table.Name;
            else
                return type.Name;
        }

        #region DOCUMENTATION Add(TEntity entity, out string errMessage)
        /// <summary>
        /// Add a new entity to the database.
        /// </summary>
        /// <remarks>
        /// Add a new entity in the database, in case of error it does not throw exceptions. 
        /// If successful, it returns the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column, it returns 0.
        /// In case of error it returns -1 and shows the error in the errMessage output variable.
        /// </remarks>
        /// <param name="entity">Entidad a Insertar</param>
        /// <param name="errMessage">OUT En caso de ocurrir una excepcion, encapsula el mensaje de error de la excepcion original</param>
        /// <returns>
        /// If successful: the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column: 0.
        /// In case of error it : -1,  and shows the error in the errMessage output variable.
        /// </returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     void main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///
        ///         long id = DaoPerson.Add(p,out string errMessage);
        ///         if (id==-1)
        ///         {
        ///             Console.WriteLine ($"Ocurrs this error:{errMessage}");
        ///         }
        ///         else
        ///         {
        ///             Console.WriteLine ($"inserted id is: {id}");
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public long Add(TEntity entity, out string errMessage)
        {
            errMessage = "";
            long identity = 0;

            try
            {
                identity = Add(entity);
            }
            catch (GenericRepositoryException daoModelException)
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

        #region DOCUMENTATION Add(TEntity entity)
        /// <summary>
        /// Add a new entity to the database.
        /// </summary>
        /// <remarks>
        /// Add a new entity in the database. 
        /// If successful, it returns the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column, it returns 0.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// If successful: the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column: 0.
        /// </returns>
        /// <exception cref="GenericRepositoryException"></exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///         
        ///         try
        ///         {
        ///             long id = DaoPerson.Add(p);
        ///             Console.WriteLine ($"inserted id is: {id}");
        ///         }
        ///         catch(GenericRepositoryExpception gre)
        ///         {
        ///             Console.WriteLine ("Error ");
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public long Add(TEntity entity)
        {
            return Add(entity, null, null);
        }

        #region DOCUMENTATION Add(TEntity entity, IDbConnection paramDbConnection)
        /// <summary>
        /// Add a new entity to the database using a connection provided.
        /// </summary>
        /// <remarks>
        /// Add a new entity in the database. 
        /// If successful, it returns the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column, it returns 0.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection"></param>
        /// <returns>
        /// If successful: the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column: 0.
        /// </returns>
        /// <exception cref="GenericRepositoryException"></exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///         
        ///         // Get new Connection.
        ///         var extConnection = DaoPerson.GetDbConnection();
        ///         extConnection.Open();
        ///         
        ///         try
        ///         {
        ///             long id = DaoPerson.Add(p,extConnection);
        ///             Console.WriteLine ($"inserted id is: {id}");
        ///         }
        ///         catch(GenericRepositoryExpception gre)
        ///         {
        ///             Console.WriteLine ("Error ");
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public long Add(TEntity entity, IDbConnection paramDbConnection)
        {
            return Add(entity, paramDbConnection, null);
        }

        #region DOCUMENTATION Add(TEntity entity, IDbTransaction paramDbTransaction)
        /// <summary>
        /// Add a new entity to the database using a transaction provided.
        /// </summary>
        /// <remarks>
        /// Add a new entity in the database. 
        /// If successful, it returns the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column, it returns 0.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns>
        /// If successful: the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column: 0.
        /// </returns>
        /// <exception cref="GenericRepositoryException"></exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///         
        ///         // Get new Connection.
        ///         var extConnection = DaoPerson.GetDbConnection();
        ///         extConnection.Open();
        ///         
        ///         var extTransaction = extConnection.BeginTransaction();
        ///         try
        ///         {
        ///             long id = DaoPerson.Add(p,extTransaction);
        ///             extTransaction.Commit();
        ///             Console.WriteLine ($"inserted id is: {id}");
        ///         }
        ///         catch(GenericRepositoryExpception gre)
        ///         {
        ///             extTransaction.RollBack();
        ///             Console.WriteLine ("Error ");
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public long Add(TEntity entity, IDbTransaction paramDbTransaction)
        {
            return Add(entity, null, paramDbTransaction);
        }

        #region DOCUMENTATION Add(TEntity entity, IDbConnection paramDbConnection,IDbTransaction paramDbTransaction)
        /// <summary>
        /// Add a new entity to the database using a connection and a transaction provided.
        /// </summary>
        /// <remarks>
        /// Add a new entity in the database. 
        /// If successful, it returns the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column, it returns 0.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns>
        /// If successful: the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column: 0.
        /// </returns>
        /// <exception cref="GenericRepositoryException"></exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///         
        ///         // Get new Connection.
        ///         var extConnection = DaoPerson.GetDbConnection();
        ///         extConnection.Open();
        ///         
        ///         var extTransaction = extConnection.BeginTransaction();
        ///         try
        ///         {
        ///             long id = DaoPerson.Add(p,extConnection,extTransaction);
        ///             extTransaction.Commit();
        ///             Console.WriteLine ($"inserted id is: {id}");
        ///         }
        ///         catch(GenericRepositoryExpception gre)
        ///         {
        ///             extTransaction.RollBack();
        ///             Console.WriteLine ("Error ");
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public long Add(TEntity entity, IDbConnection paramDbConnection,IDbTransaction paramDbTransaction)
        {
            long identity = 0;
            bool hasIdentity = false;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            StringBuilder parameterList = new StringBuilder();
            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttibute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (!customAttibute.IsIdentity)
                {
                    string parameterName = "@param_" + i.Name;

                    parameterList.Append(string.Format("{0},", parameterName));

                    ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[i.Name];

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
                insertQuery.Append(string.Format("INSERT INTO {0} ( {1} ) VALUES ( {2} ) {3}  ", _syntaxProvider.GetSecureTableName(TableName), ColumnsForInsert, parameterList, _syntaxProvider.GetSubQueryGetIdentity()));
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
                            // In case of OleDbConnection, exec the command and immediately, 
                            // exec new query to obtain the identity;
                            dbCommand.ExecuteNonQuery();

                            dbCommand.CommandText = "Select @@Identity";
                            identity = DbUtil.ParseToLong(dbCommand.ExecuteScalar());
                        }
                        else
                        {
                            identity = DbUtil.ParseToLong(dbCommand.ExecuteScalar());
                        }
                    }
                    else
                    {
                        dbCommand.ExecuteNonQuery();
                        identity = 0;
                    }

                    dbCommand.Dispose();

                }
                catch (Exception exception)
                {
                   throw new GenericRepositoryException(exception, dbCommand);
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

        #region DOCUMENTATION AddRange(IList<TEntity> entities, out string errMessage)
        /// <summary>
        /// Add a list of entities to the database. No throw exceptions.
        /// </summary>
        /// <remarks>
        /// Add a IList of entities in the database. 
        /// If successful, it returns <c>true</c>.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entities">IList of entities.</param>
        /// <param name="errMessage">In case of exception the error message</param>
        /// <returns>
        /// Returns <c>true</c>, if all entities were inserted correctly, else return <c>false</c>. 
        /// </returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         var listEntities = new List<Person>();
        ///        
        ///         listEntities.Add(new Person { LastName = "Martinez", FirstName = "Diego", Passport = "AR00127296" });
        ///         listEntities.Add(new Person { LastName = "Perez", FirstName = "Juan", Passport = "AR00012224" });
        ///         listEntities.Add(new Person { LastName = "Fulanito", FirstName = "Cosme", Passport = "AR99999987" });
        ///          
        ///          var result = DaoPerson.AddRange(listEntities, out string errMessage);
        ///          if (result)
        ///          {
        ///              Console.WriteLine ("List inserted succefully");
        ///          }
        ///          else
        ///          {
        ///              Console.WriteLine ($"Error: {errMessage} ");
        ///          }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool AddRange(IList<TEntity> entities, out string errMessage)
        {
            errMessage = "";
            bool result = false;

            try
            {
                result = AddRange(entities);
            }
            catch (GenericRepositoryException daoModelException)
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

        #region DOCUMENTATION AddRange(IList<TEntity> entities)
        /// <summary>
        /// Add a list of entities to the database .
        /// </summary>
        /// <remarks>
        /// Add a IList of entities in the database. 
        /// If successful, it returns <c>true</c>.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entities">IList of entities.</param>
        /// <exception cref="GenericRepositoryException">
        /// </exception>
        /// <returns>
        /// Returns <c>true</c>, if all entities were inserted correctly, else return <c>false</c>. 
        /// </returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         var listEntities = new List<Person>();
        ///        
        ///         listEntities.Add(new Person { LastName = "Martinez", FirstName = "Diego", Passport = "AR00127296" });
        ///         listEntities.Add(new Person { LastName = "Perez", FirstName = "Juan", Passport = "AR00012224" });
        ///         listEntities.Add(new Person { LastName = "Fulanito", FirstName = "Cosme", Passport = "AR99999987" });
        ///          
        ///          try
        ///          {
        ///              DaoPerson.AddRange(listEntities);
        ///              Console.WriteLine ("List inserted succefully");
        ///          }
        ///          catch(GenericRepositoryExpception gre)
        ///          {
        ///              Console.WriteLine ("Error ");
        ///          }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool AddRange(IList<TEntity> entities)
        {
            return AddRange(entities, null,null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection paramDbConnection)
        /// <summary>
        /// Add a list of entities to the database using a connection provided.
        /// </summary>
        /// <remarks>
        /// Add a IList of entities in the database. 
        /// If successful, it returns <c>true</c>.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entities">IList of entities.</param>
        /// <param name="paramDbConnection">The database connection.</param>
        /// <exception cref="GenericRepositoryException">
        /// </exception>
        /// <returns>
        /// Returns <c>true</c>, if all entities were inserted correctly, else return <c>false</c>. 
        /// </returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         var listEntities = new List<Person>();
        ///        
        ///         listEntities.Add(new Person { LastName = "Martinez", FirstName = "Diego", Passport = "AR00127296" });
        ///         listEntities.Add(new Person { LastName = "Perez", FirstName = "Juan", Passport = "AR00012224" });
        ///         listEntities.Add(new Person { LastName = "Fulanito", FirstName = "Cosme", Passport = "AR99999987" });
        ///         
        ///          // Get new Connection.
        ///          var extConnection = DaoPerson.GetDbConnection();
        ///          extConnection.Open();
        ///          
        ///          try
        ///          {
        ///              DaoPerson.AddRange(listEntities,extConnection);
        ///              extTransaction.Commit();
        ///              Console.WriteLine ("List inserted succefully");
        ///          }
        ///          catch(GenericRepositoryExpception gre)
        ///          {
        ///              Console.WriteLine ("Error ");
        ///          }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection)
        {
            return AddRange(entities, paramDbConnection, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities,IDbTransaction paramDbTransaction)
        /// <summary>
        /// Add a list of entities to the database using a transaction provided.
        /// </summary>
        /// <remarks>
        /// Add a IList of entities in the database. 
        /// If successful, it returns <c>true</c>.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entities">IList of entities.</param>
        /// <param name="paramDbTransaction">The database transaction.</param>
        /// <exception cref="GenericRepositoryException">
        /// </exception>
        /// <returns>
        /// Returns <c>true</c>, if all entities were inserted correctly, else return <c>false</c>. 
        /// </returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         var listEntities = new List<Person>();
        ///        
        ///         listEntities.Add(new Person { LastName = "Martinez", FirstName = "Diego", Passport = "AR00127296" });
        ///         listEntities.Add(new Person { LastName = "Perez", FirstName = "Juan", Passport = "AR00012224" });
        ///         listEntities.Add(new Person { LastName = "Fulanito", FirstName = "Cosme", Passport = "AR99999987" });
        ///         
        ///          // Get new Connection.
        ///          var extConnection = DaoPerson.GetDbConnection();
        ///          extConnection.Open();
        ///         
        ///          var extTransaction = extConnection.BeginTransaction();
        ///          
        ///          try
        ///          {
        ///              DaoPerson.AddRange(listEntities,extTransaction);
        ///              extTransaction.Commit();
        ///              Console.WriteLine ("List inserted succefully");
        ///          }
        ///          catch(GenericRepositoryExpception gre)
        ///          {
        ///              extTransaction.RollBack();
        ///              Console.WriteLine ("Error ");
        ///          }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction)
        {
            return AddRange(entities, null, paramDbTransaction);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities,IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        /// <summary>
        /// Add a list of entities to the database using a connection and a transaction provided.
        /// </summary>
        /// <remarks>
        /// Add a IList of entities in the database. 
        /// If successful, it returns <c>true</c>.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entities">IList of entities.</param>
        /// <param name="paramDbConnection">The database connection.</param>
        /// <param name="paramDbTransaction">The database transaction.</param>
        /// <exception cref="GenericRepositoryException">
        /// </exception>
        /// <returns>
        /// Returns <c>true</c>, if all entities were inserted correctly, else return <c>false</c>. 
        /// </returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         var listEntities = new List<Person>();
        ///        
        ///         listEntities.Add(new Person { LastName = "Martinez", FirstName = "Diego", Passport = "AR00127296" });
        ///         listEntities.Add(new Person { LastName = "Perez", FirstName = "Juan", Passport = "AR00012224" });
        ///         listEntities.Add(new Person { LastName = "Fulanito", FirstName = "Cosme", Passport = "AR99999987" });
        ///         
        ///          // Get new Connection.
        ///          var extConnection = DaoPerson.GetDbConnection();
        ///          extConnection.Open();
        ///         
        ///          var extTransaction = extConnection.BeginTransaction();
        ///          
        ///          try
        ///          {
        ///              DaoPerson.AddRange(listEntities,extConnection,extTransaction);
        ///              extTransaction.Commit();
        ///              Console.WriteLine ("List inserted succefully");
        ///          }
        ///          catch(GenericRepositoryExpception gre)
        ///          {
        ///              extTransaction.RollBack();
        ///              Console.WriteLine ("Error ");
        ///          }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return AddRange(entities, paramDbConnection, paramDbTransaction, out long recordsAffected);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities,IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        /// <summary>
        /// Add a list of entities to the database using a connection and transaction provided.
        /// </summary>
        /// <remarks>
        /// Add a IList of entities in the database. 
        /// If successful, it returns <c>true</c>.
        /// In case of Exception trows new GenericRepositoryException.
        /// </remarks>
        /// <param name="entities">IList of entities.</param>
        /// <param name="paramDbConnection">The database connection.</param>
        /// <param name="paramDbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">OUT The records affected.</param>
        /// <exception cref="GenericRepositoryException">
        /// </exception>
        /// <returns>
        /// Returns <c>true</c>, if all entities were inserted correctly, else return <c>false</c>. 
        /// And in out variable returs the recordsAffecteds.
        /// </returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         var listEntities = new List<Person>();
        ///        
        ///         listEntities.Add(new Person { LastName = "Martinez", FirstName = "Diego", Passport = "AR00127296" });
        ///         listEntities.Add(new Person { LastName = "Perez", FirstName = "Juan", Passport = "AR00012224" });
        ///         listEntities.Add(new Person { LastName = "Fulanito", FirstName = "Cosme", Passport = "AR99999987" });
        ///         
        ///          // Get new Connection.
        ///          var extConnection = DaoPerson.GetDbConnection();
        ///          extConnection.Open();
        ///         
        ///          var extTransaction = extConnection.BeginTransaction();
        ///          
        ///          try
        ///          {
        ///              DaoPerson.AddRange(listEntities,extConnection,extTransaction,out long recordsAffected);
        ///              extTransaction.Commit();
        ///              Console.WriteLine ($"RecordsAffecteds{recordsAffected}");
        ///          }
        ///          catch(GenericRepositoryExpception gre)
        ///          {
        ///              extTransaction.RollBack();
        ///              Console.WriteLine ("Error ");
        ///          }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool AddRange(IList<TEntity> entities,IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
            }
            catch (Exception connectionException)
            {
                throw new GenericRepositoryException(connectionException);
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

                        ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[i.Name];
                        dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                    }
                }

                if (parameterList != string.Empty)
                {
                    parameterList = parameterList.Substring(0, parameterList.Length - 1);
                    insertRangeQuery = (string.Format("INSERT INTO {0} ( {1} ) VALUES ( {2} );", _syntaxProvider.GetSecureTableName(TableName), ColumnsForInsert, parameterList));
                }

                try
                {
                    dbCommand.CommandText = insertRangeQuery;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    recordsAffected += dbCommand.ExecuteNonQuery();

                    dbCommand.Dispose();
                }
                catch (Exception commandException)
                {
                    // In case of exception, if the command is open, close it.
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        if (dbConnection.State == ConnectionState.Open)
                            dbConnection.Close();

                        dbConnection.Dispose();
                    }

                    throw new GenericRepositoryException(commandException, dbCommand);
                }
            }

            if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return true;
        }

        #region DOCUMENTATION Update(TEntity entity, out string errMessage)
        /// <summary>
        /// Updates the specified entity. No throw exceptions. 
        /// If an exception occurs, its message will be displayed in the errMessage output variable.
        /// </summary>
        /// <remarks>
        /// Update a specified entity. To determine which row to update in the database, 
        /// this method uses the "IsPrimary" attributes of the DbColumnAttribute class. <see cref="Attributes.DbColumnAttribute"/>
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="errMessage"></param>
        /// <returns>
        /// Returns <c>true</c>, if the entity were updated correctly, else return <c>false</c>. 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// There is no primary key defined in the entity, at least one attribute must have the property IsPrimary = True
        /// </exception>
        /// <exception cref="GenericRepositoryException">Throw </exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     
        ///     public Person GetByFirstNameLastName(string firstName, string lastName)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.FirstName == firstName && entity.LastName == lastName));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        /// 
        ///         var person = DaoPerson.GetByFirstNameLastName("Diego", "Martinez");
        ///         if (person != null)
        ///         {
        ///             person.Passport = "AR27026754";
        ///             var result = DaoPerson.Update(person, out string errMessage);
        ///                 
        ///             if (result )
        ///                 Console.WriteLine($"The passport of {person.FirstName} was updated correctly.");
        ///             else
        ///                 Console.WriteLine($"Error updating the passport of {person.FirstName}, Error: {errMessage} );
        ///                 
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool Update(TEntity entity, out string errMessage)
        {
            errMessage = "";
            bool result = false;

            try
            {
                result = Update(entity);
            }
            catch (GenericRepositoryException daoModelException)
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
        #region DOCUMENTATION Update(TEntity entity)
        /// <summary>
        /// Updates the specified entity. 
        /// </summary>
        /// <remarks>
        /// Update a specified entity. To determine which row to update in the database, 
        /// this method uses the "IsPrimary" attributes of the DbColumnAttribute class. <see cref="Attributes.DbColumnAttribute"/>
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Returns <c>true</c>, if the entity were updated correctly, else return <c>false</c>. 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// There is no primary key defined in the entity, at least one attribute must have the property IsPrimary = True
        /// </exception>
        /// <exception cref="GenericRepositoryException">Throw </exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     
        ///     public Person GetByFirstNameLastName(string firstName, string lastName)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.FirstName == firstName && entity.LastName == lastName));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        /// 
        ///         try
        ///         {
        ///             var person = DaoPerson.GetByFirstNameLastName("Diego", "Martinez");
        ///             if (person != null)
        ///             {
        ///                 person.Passport = "AR27026754";
        ///                 var result = DaoPerson.Update(person);
        ///                 
        ///                 if (result )
        ///                     Console.WriteLine($"The passport of {person.FirstName} was updated correctly. {recordsAffected} was affected");
        ///             }
        ///         }    
        ///         catch(ArgumentException ae)
        ///         {
        ///                 Console.WriteLine("Error on the entity atribute");
        ///         }
        ///         catch(GenericRepositoryException gre)
        ///         {
        ///                 Console.WriteLine("Database error");
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool Update(TEntity entity)
        {
            return Update(entity, null, null);
        }

        #region DOCUMENTATION Update(TEntity entity, IDbConnection paramDbConnection)
        /// <summary>
        /// Updates the specified entity, using a connection provided. 
        /// </summary>
        /// <remarks>
        /// Update a specified entity. To determine which row to update in the database, 
        /// this method uses the "IsPrimary" attributes of the DbColumnAttribute class. <see cref="Attributes.DbColumnAttribute"/>
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection">The database connection.</param>
        /// <returns>
        /// Returns <c>true</c>, if the entity were updated correctly, else return <c>false</c>. 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// There is no primary key defined in the entity, at least one attribute must have the property IsPrimary = True
        /// </exception>
        /// <exception cref="GenericRepositoryException">Throw </exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     
        ///     public Person GetByFirstNameLastName(string firstName, string lastName)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.FirstName == firstName && entity.LastName == lastName));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         // Get new Connection.
        ///         var extConnection = DaoPerson.GetDbConnection();
        ///         extConnection.Open();
        /// 
        ///         try
        ///         {
        ///             
        ///             var person = DaoPerson.GetByFirstNameLastName("Diego" , "Martinez");
        ///             if (person != null)
        ///             {
        ///                 person.Passport = "AR27026754";
        ///                 var result = DaoPerson.Update(person, extConnection);
        ///                 
        ///                 if (result )
        ///                     Console.WriteLine($"The passport of {person.FirstName} was updated correctly.");
        ///             }
        ///         }    
        ///         catch(ArgumentException ae)
        ///         {
        ///                 Console.WriteLine("Error on the entity atribute");
        ///         }
        ///         catch(GenericRepositoryException gre)
        ///         {
        ///                 Console.WriteLine("Database error");
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool Update(TEntity entity, IDbConnection paramDbConnection)
        {
            return Update(entity, paramDbConnection, null);
        }

        #region DOCUMENTATION Update(TEntity entity, IDbTransaction paramDbTransaction)
        /// <summary>
        /// Updates the specified entity, using a transaction provided. 
        /// </summary>
        /// <remarks>
        /// Update a specified entity. To determine which row to update in the database, 
        /// this method uses the "IsPrimary" attributes of the DbColumnAttribute class. <see cref="Attributes.DbColumnAttribute"/>
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbTransaction">The database transaction.</param>
        /// <returns>
        /// Returns <c>true</c>, if the entity were updated correctly, else return <c>false</c>. 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// There is no primary key defined in the entity, at least one attribute must have the property IsPrimary = True
        /// </exception>
        /// <exception cref="GenericRepositoryException">Throw </exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     
        ///     public Person GetByFirstNameLastName(string firstName, string lastName)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.FirstName == firstName && entity.LastName == lastName));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         // Get new Connection.
        ///         var extConnection = DaoPerson.GetDbConnection();
        ///         extConnection.Open();
        ///         var extTransaction = extConnection.BeginTransaction();
        /// 
        ///         try
        ///         {
        ///             
        ///             var person = DaoPerson.GetByFirstNameLastName("Diego" , "Martinez");
        ///             if (person != null)
        ///             {
        ///                 person.Passport = "AR27026754";
        ///                 var result = DaoPerson.Update(person, extTransaction);
        ///                 extTransaction.Commit();
        ///                 
        ///                 if (result )
        ///                     Console.WriteLine($"The passport of {person.FirstName} was updated correctly.");
        ///             }
        ///         }    
        ///         catch(ArgumentException ae)
        ///         {
        ///                 Console.WriteLine("Error on the entity atribute");
        ///                 extTransaction.Rollback();
        ///         }
        ///         catch(GenericRepositoryException gre)
        ///         {
        ///                 Console.WriteLine("Database error");
        ///                 extTransaction.Rollback();
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool Update(TEntity entity, IDbTransaction paramDbTransaction)
        {
            return Update(entity, null, paramDbTransaction);
        }

        #region DOCUMENTATION Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        /// <summary>
        /// Updates the specified entity, using a connection and transaction provided. 
        /// </summary>
        /// <remarks>
        /// Update a specified entity. To determine which row to update in the database, 
        /// this method uses the "IsPrimary" attributes of the DbColumnAttribute class. <see cref="Attributes.DbColumnAttribute"/>
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection">The database connection.</param>
        /// <param name="paramDbTransaction">The database transaction.</param>
        /// <returns>
        /// Returns <c>true</c>, if the entity were updated correctly, else return <c>false</c>. 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// There is no primary key defined in the entity, at least one attribute must have the property IsPrimary = True
        /// </exception>
        /// <exception cref="GenericRepositoryException">Throw </exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     
        ///     public Person GetByFirstNameLastName(string firstName, string lastName)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.FirstName == firstName && entity.LastName == lastName));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         // Get new Connection.
        ///         var extConnection = DaoPerson.GetDbConnection();
        ///         extConnection.Open();
        ///         var extTransaction = extConnection.BeginTransaction();
        /// 
        ///         try
        ///         {
        ///             
        ///             var person = DaoPerson.GetByFirstNameLastName("Diego", "Martinez");
        ///             if (person != null)
        ///             {
        ///                 person.Passport = "AR27026754";
        ///                 var result = DaoPerson.Update(person, extConnection, extTransaction);
        ///                 extTransaction.Commit();
        ///                 
        ///                 if (result )
        ///                     Console.WriteLine($"The passport of {person.FirstName} was updated correctly.");
        ///             }
        ///         }    
        ///         catch(ArgumentException ae)
        ///         {
        ///                 Console.WriteLine("Error on the entity atribute");
        ///                 extTransaction.Rollback();
        ///         }
        ///         catch(GenericRepositoryException gre)
        ///         {
        ///                 Console.WriteLine("Database error");
        ///                 extTransaction.Rollback();
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return Update(entity, paramDbConnection, paramDbTransaction, out long recordsAffected);
        }

        #region DOCUMENTATION Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        /// <summary>
        /// Updates the specified entity, using a connection and transaction provided. 
        /// </summary>
        /// <remarks>
        /// Update a specified entity. To determine which row to update in the database, 
        /// this method uses the "IsPrimary" attributes of the DbColumnAttribute class. <see cref="Attributes.DbColumnAttribute"/>
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="paramDbConnection">The database connection.</param>
        /// <param name="paramDbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">out the records affected.</param>
        /// <returns>
        /// Returns <c>true</c>, if the entity were updated correctly, else return <c>false</c>. 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// There is no primary key defined in the entity, at least one attribute must have the property IsPrimary = True
        /// </exception>
        /// <exception cref="GenericRepositoryException">Throw </exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///[DbTable (Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///}
        ///
        ///public class DaoPerson: BaseGenericRepository<Person>
        ///{
        ///     public DaoPerson(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        ///     {
        ///     
        ///     }
        ///     
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     
        ///     public Person GetByFirstNameLastName(string firstName, string lastName)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.FirstName == firstName && entity.LastName == lastName));
        ///     }
        ///}
        ///
        ///class Test
        ///{
        ///     static void Main()
        ///     {
        ///         // MSSqlServer connectionString.
        ///         var myConnectionString = "MyConnectionString";
        ///      
        ///         var DaoPerson = new DaoPerson<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         
        ///         // Get new Connection.
        ///         var extConnection = DaoPerson.GetDbConnection();
        ///         extConnection.Open();
        ///         var extTransaction = extConnection.BeginTransaction();
        /// 
        ///         try
        ///         {
        ///             
        ///             var person = DaoPerson.GetByFirstNameLastName("Diego" , "Martinez");
        ///             if (person != null)
        ///             {
        ///                 person.Passport = "AR27026754";
        ///                 var result = DaoPerson.Update(person, extConnection, extTransaction, out long recordsAffected);
        ///                 extTransaction.Commit();
        ///                 
        ///                 if (result )
        ///                     Console.WriteLine($"The passport of {person.FirstName} was updated correctly. {recordsAffected} was affected");
        ///             }
        ///         }    
        ///         catch(ArgumentException ae)
        ///         {
        ///                 Console.WriteLine("Error on the entity atribute");
        ///                 extTransaction.Rollback();
        ///         }
        ///         catch(GenericRepositoryException gre)
        ///         {
        ///                 Console.WriteLine("Database error");
        ///                 extTransaction.Rollback();
        ///         }
        ///     }
        ///}
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public bool Update(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            StringBuilder parameterList = new StringBuilder();
            StringBuilder where = new StringBuilder();

            bool hasPrimary = false;

            string parameterName;
            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                parameterName = "@param_" + i.Name;
                var columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name); 

                if (!customAttribute.IsPrimary)
                {
                    parameterList.Append(string.Format("{0} = {1},", columnName, parameterName));
                }
                else
                {
                    hasPrimary = true;

                    if (where.ToString() != String.Empty)
                        where.Append(" AND ");

                    where.Append(string.Format("({0} = {1})", columnName, parameterName));
                }

                ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[i.Name];
                dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
            }

            if (!hasPrimary)
            {
                throw new ArgumentException("There is no primary key defined in the entity, at least one attribute must have the property IsPrimary = True");
            }

            if (parameterList.ToString() != string.Empty)
            {
                parameterList.Remove(parameterList.Length - 1, 1);
                StringBuilder updateQuery = new StringBuilder();
                updateQuery.Append(string.Format("UPDATE  {0} SET {1} WHERE {2}", _syntaxProvider.GetSecureTableName(TableName), parameterList, where));

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
                    dbCommand.Dispose();
                }
                catch (Exception exception)
                {
                    throw new GenericRepositoryException(exception, dbCommand);
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
            catch (GenericRepositoryException daoModelException)
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

        /// <summary>
        /// Updates the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        /// <exception cref="GenericRepositoryException">
        /// </exception>
        private bool UpdateRange(IList<TEntity> entities,IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
            }
            catch (Exception exceptionDb)
            {
                throw new GenericRepositoryException(exceptionDb);
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
                    var columnName = _syntaxProvider.GetSecureColumnName(customAttibute.ColumnName ?? i.Name);

                    parameterName = "@param_" + i.Name;

                    if (!customAttibute.IsPrimary)
                    {
                        parameterList.Append(string.Format("{0} = {1},", columnName, parameterName));
                    }
                    else
                    {
                        if (where.ToString() != String.Empty)
                            where.Append(" AND ");

                        where.Append(string.Format("({0} = {1})", columnName, parameterName));
                    }
                    ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                }

                if (parameterList.ToString() != string.Empty)
                {
                    parameterList.Remove(parameterList.Length - 1, 1);
                    updateRangeQuery.AppendLine(string.Format("UPDATE {0} SET {1} WHERE {2}; ", _syntaxProvider.GetSecureTableName(TableName), parameterList, where));
                }

                try
                {
                    dbCommand.CommandText = updateRangeQuery.ToString();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    recordsAffected += dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();

                }
                catch(Exception exceptionCommand)
                {
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnection.Close();
                        dbConnection.Dispose();
                    }
                    throw new GenericRepositoryException(exceptionCommand, dbCommand);
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
            catch (GenericRepositoryException daoModelException)
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

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            TEntity entity = new TEntity();
            StringBuilder where = new StringBuilder();

            string parameterName;
            foreach (PropertyInfo i in ListObjectPropertyInfo)
            {
                var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                var columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

                parameterName = "@param_" + i.Name;

                if (customAttribute.IsPrimary)
                {
                    if (where.ToString() != String.Empty)
                        where.Append(" AND ");

                    where.Append(string.Format("({0} = {1})", columnName, parameterName));

                    ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[i.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, Id, cdbi.MaxLenght));

                }
            }

            if (where.ToString() != string.Empty)
            {
                StringBuilder deleteQuery = new StringBuilder();
                deleteQuery.Append(string.Format("DELETE FROM {0} WHERE {1}", _syntaxProvider.GetSecureTableName(TableName), where));

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
                    dbCommand.Dispose();
                }
                catch (Exception exception)
                {
                    throw new GenericRepositoryException(exception, dbCommand);
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
            catch (GenericRepositoryException daoModelException)
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

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = string.Format("DELETE FROM {0}", _syntaxProvider.GetSecureTableName(TableName));

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;


                recordsAffected = dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException(exception, dbCommand);
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

        /// <summary>
        /// Deletes the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return DeleteByLambdaExpressionFilter(lambdaExpressionFilter, null, null);
        }

        /// <summary>
        /// Deletes the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection)
        {
            return DeleteByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, null);
        }

        /// <summary>
        /// Deletes the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction)
        {
            return DeleteByLambdaExpressionFilter(lambdaExpressionFilter, null, paramDbTransaction);
        }

        /// <summary>
        /// Deletes the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        /// <exception cref="GenericRepositoryException"></exception>
        public long DeleteByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter,IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {

            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            long recordsAffected = 0;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = string.Format("DELETE FROM {0} {1}"  , _syntaxProvider.GetSecureTableName(TableName),query.QueryWhere);

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                recordsAffected = dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException(exception, dbCommand);
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

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public TEntity GetById(object id)
        {
            return GetById(id, null,null);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        public TEntity GetById(object id, IDbConnection paramDbConnection)
        {
            return GetById(id,paramDbConnection, null);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        public TEntity GetById(object id, IDbTransaction paramDbTransaction)
        {
            return GetById(id, null, paramDbTransaction);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        /// <exception cref="GenericRepositoryException"></exception>
        public TEntity GetById(object id,IDbConnection paramDbConnection,IDbTransaction paramDbTransaction)
        {
            TEntity entity = new TEntity();

            StringBuilder clause = new StringBuilder();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            foreach (var pi in ListObjectPropertyInfo)
            {
                var pk = pi.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (pk != null && pk.IsPrimary)
                {
                    var columnName = _syntaxProvider.GetSecureColumnName(pk.ColumnName ?? pi.Name);

                    string parameterName = "@param_" + pi.Name;
                    clause.Append(string.Format("{0}={1}", columnName, id));

                    ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[pi.Name];
                    dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, id, cdbi.MaxLenght));

                    break;
                }
            }

            entity = null;
            if (clause.ToString() != string.Empty)
            {
                StringBuilder getQuery = new StringBuilder();
                getQuery.Append(string.Format("SELECT {0} FROM {1} WHERE {2}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName), clause));

                dbCommand.CommandText = getQuery.ToString();

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    if (_dataBaseEngine == EDataBaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    IDataReader reader = dbCommand.ExecuteReader();

                    if (reader.Read())
                        entity = new MapDataReaderToEntity().MapSingle<TEntity>(reader, ListObjectPropertyInfo);
                    else
                        entity = default(TEntity);

                    reader.Close();
                    reader.Dispose();
                    dbCommand.Dispose();

                }
                catch (Exception exception)
                {
                    throw new GenericRepositoryException(exception, dbCommand);
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
        /// Gets the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <returns></returns>
        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, null, null);
        }

        /// <summary>
        /// Gets the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <returns></returns>
        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, paramDbConnection, null);
        }

        /// <summary>
        /// Gets the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbTransaction paramDbTransaction)
        {
            return GetByLambdaExpressionFilter(lambdaExpressionFilter, null, paramDbTransaction);
        }

        /// <summary>
        /// Gets the by lambda expression filter.
        /// </summary>
        /// <param name="lambdaExpressionFilter">The lambda expression filter.</param>
        /// <param name="paramDbConnection">The parameter database connection.</param>
        /// <param name="paramDbTransaction">The parameter database transaction.</param>
        /// <returns></returns>
        /// <exception cref="GenericRepositoryException"></exception>
        public TEntity GetByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            TEntity entity = null;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);
            
            string getQuery = string.Format("SELECT {0} FROM {1} {2}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName), query.QueryWhere);

            dbCommand.CommandText = getQuery.ToString();
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                    entity = new MapDataReaderToEntity().MapSingle<TEntity>(reader, ListObjectPropertyInfo);
                else
                    entity = default(TEntity);

                reader.Close();
                reader.Dispose();

                dbCommand.Dispose();
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException(exception, dbCommand);
            }
            finally
            {
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }

            return entity;
        }

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
        public IList<TEntity> GetAll(IDbConnection paramDbConnection,IDbTransaction paramDbTransaction)
        {
            IList<TEntity> collection = new List<TEntity>();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = string.Format("SELECT {0} FROM {1}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName));

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                IDataReader reader = dbCommand.ExecuteReader();
                collection = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo,_dataBaseEngine);

                reader.Close();reader.Dispose();
                dbCommand.Dispose();
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException(exception, dbCommand);
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
            IList<TEntity> result = new List<TEntity>();
            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);
            
            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                if(dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                dbCommand.CommandText = query.QueryString;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader();
                result = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo,_dataBaseEngine);

                reader.Close();
                reader.Dispose();
                dbCommand.Dispose();

            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException(exception, dbCommand);
            }
            finally
            {
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            return result;
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

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            foreach (var pi in ListObjectPropertyInfo)
            {
                var pk = pi.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (pk != null && pk.IsPrimary)
                {
                    var columnName = _syntaxProvider.GetSecureColumnName(pk.ColumnName ?? pi.Name);

                    string _ids = string.Empty;
                    foreach (var id in ids)
                    {
                        if (_ids != string.Empty)
                            _ids = _ids + ",";

                        _ids = _ids + id.ToString();
                    }

                    clause.Append(string.Format("{0} IN ({1})", columnName, _ids));
                    break;
                }
            }

            if (clause.ToString() != string.Empty)
            {
                StringBuilder findQuery = new StringBuilder();
                findQuery.Append(string.Format("SELECT {0} FROM {1} WHERE {2}", ColumnsForSelect, _syntaxProvider.GetSecureTableName(TableName), clause));

                try
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    dbCommand.CommandText = findQuery.ToString();

                    if (paramDbTransaction != null)
                        dbCommand.Transaction = paramDbTransaction;

                    IDataReader reader = dbCommand.ExecuteReader();
                    entities = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo,_dataBaseEngine);

                    reader.Close();
                    reader.Dispose();

                    dbCommand.Dispose();
                }
                catch (Exception exception)
                {
                    throw new GenericRepositoryException(exception,dbCommand);
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
        /// Gets IDBConnection instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// A closed IDBConnection instantiated according to the database engine
        /// </returns>
        protected IDbConnection BaseIDbConnection()
        {
            return DataObjectProvider.GetIDbConnection(_connectionString);
        }

        /// <summary>
        /// Gets DBConnection instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// A closed DBConnection instantiated according to the database engine.
        /// </returns>
        protected DbConnection BaseDbConnection()
        {
            return DataObjectProvider.GetDbConnection(_connectionString);
        }

        /// <summary>
        /// A new instance of DbExecute instantiated according to the database engine
        /// </summary>
        /// <returns>
        /// A new instance of DbExecute instantiated according to the database engine.
        /// </returns>
        protected DbExecutor BaseDbExecutor()
        {
            return new DbExecutor(_connectionString, _dataBaseEngine);
        }

        /// <summary>
        /// Creates an query parameters list.
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, object> CreateQueryParametersList()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            return parameters;
        }

        private void GetDbColumnsAndAtributes()
        {
            TEntity entity = new TEntity();

            StringBuilder sbColumnsInsert = new StringBuilder();
            StringBuilder sbColumnsSelect = new StringBuilder();

            using (DbConnection conn = BaseDbConnection())
            {
                conn.Open();

                foreach (PropertyInfo i in ListObjectPropertyInfo)
                {
                    var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                    string columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

                    if (!customAttribute.IsIdentity)
                        sbColumnsInsert.Append(string.Format("{0},", columnName));

                    sbColumnsSelect.Append(string.Format("{0},", columnName));

                    string schemaColumnName = _syntaxProvider.RemoveStartersAndEndersColumnName(customAttribute.ColumnName ?? i.Name);
                    _columnListWithDbInfo.Add(i.Name, GetColumnDbInfo(schemaColumnName, conn));
                }
            }

            if (sbColumnsInsert.Length > 0)
                sbColumnsInsert.Remove(sbColumnsInsert.Length - 1, 1);

            if (sbColumnsSelect.Length > 0)
                sbColumnsSelect.Remove(sbColumnsSelect.Length - 1, 1);

            ColumnsForInsert = sbColumnsInsert.ToString();
            ColumnsForSelect = sbColumnsSelect.ToString();
        }

        private ColumnDbInfo GetColumnDbInfo(string columnName,DbConnection dbConnection)
        {
            ColumnDbInfo columnData = new ColumnDbInfo();

            DataTable columnProperty;
            if (_dataBaseEngine == EDataBaseEngine.OleDb || _dataBaseEngine == EDataBaseEngine.MySql)
                columnProperty = dbConnection.GetSchema("Columns", new[] { null, null, TableName, columnName });
            else
                columnProperty = dbConnection.GetSchema("Columns", new[] { dbConnection.Database, null, TableName, columnName });

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
                columnData.MaxLenght = maxLength;

            var dataType = dataRow["DATA_TYPE"];

            if (_dataBaseEngine == EDataBaseEngine.OleDb)
            {
                if (DbTypeDictionary.DbColumnTypeToDbTypeEnum.TryGetValue(((OleDbType)dataType).ToString(), out DbType dBTypeValue))
                    columnData.DbType = dBTypeValue;
                else
                    throw new ArgumentOutOfRangeException("The datatype is not recognited. See the DbColumnTypeToDbTypeEnum Dictionary");
            }
            else
            {
                if (DbTypeDictionary.DbColumnTypeToDbTypeEnum.TryGetValue(dataType.ToString(), out DbType dBTypeValue))
                    columnData.DbType = dBTypeValue;
                else
                    throw new ArgumentOutOfRangeException("The datatype is not recognited. See the DbColumnTypeToDbTypeEnum Dictionary");
            }

            return columnData;
            
        }

        /// <summary>
        /// Gets IDbDataParameter instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// An IDbDataParameter instantiated according to the database engine.
        /// </returns>
        protected IDbDataParameter GetIDbDataParameter()
        {
            return DataObjectProvider.GetIDbDataParameter();
        }

        /// <summary>
        /// Creates an IDbDataParameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// An IDbDataParameter instantiated according to the database engine.
        /// </returns>
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
        /// Creates an IDbDataParameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="DbType">Database Tyoe.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// An IDbDataParameter instantiated according to the database engine.
        /// </returns>
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
                throw new ArgumentOutOfRangeException("The databaseEngine is not supported by this method");
        }

    }
    internal class ColumnDbInfo
    {
        internal DbType DbType { get; set; }
        internal Int32? MaxLenght { get; set; }
    }


    /// <summary>
    /// Generic Class to manage views
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class BaseGenericViewManager<TEntity> where TEntity : class, new()
    {
        private string _connectionString;
        /// <summary>
        /// Gets the active ConnectionString.
        /// </summary>
        public string ConnectionString { get { return _connectionString; } private set { _connectionString = value; } }

        private EDataBaseEngine _dataBaseEngine;
        /// <summary>
        /// Gets the Database Engine.
        /// </summary>
        /// <seealso cref=" SysWork.Data.Common.EDataBaseEngine"/>
        public EDataBaseEngine DataBaseEngine { get { return _dataBaseEngine; } private set { _dataBaseEngine = value; } }

        /// <summary>
        /// Gets the data object provider.
        /// </summary>
        /// <value>The DbObjectProvider used in this class.</value>
        protected DbObjectProvider DataObjectProvider { get; private set; }

        private SyntaxProvider _syntaxProvider;

        /// <summary>
        /// Get the name of the database table to represent.
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// Get the propertyInfo of the columns.
        /// </summary>
        public IList<PropertyInfo> ListObjectPropertyInfo { get; private set; }

        /// <summary>
        /// Get a list of the columns to perform a SELECT sentence on the represented table, separated by commas.
        /// </summary>
        public string ColumnsForSelect { get; private set; }

        /// <summary>
        /// Initializes a new instance class. Using MSSqlServer as DataBaseEngine.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public BaseGenericViewManager(string connectionString)
        {
            BaseGenericViewManagerConstructorResolver(connectionString, EDataBaseEngine.MSSqlServer);
        }

        /// <summary>
        /// Initializes a new instance class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dataBaseEngine">The data base engine.</param>
        public BaseGenericViewManager(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            BaseGenericViewManagerConstructorResolver(connectionString, dataBaseEngine);
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        /// <returns></returns>
        protected DbConnection GetDbConnection()
        {
            return DataObjectProvider.GetDbConnection(_connectionString);
        }

        /// <summary>
        /// Gets IDbDataParameter instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// An IDbDataParameter instantiated according to the database engine.
        /// </returns>
        protected IDbDataParameter GetIDbDataParameter()
        {
            return DataObjectProvider.GetIDbDataParameter();
        }

        /// <summary>
        /// Creates an IDbDataParameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// An IDbDataParameter instantiated according to the database engine.
        /// </returns>
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
        /// Gets IDBConnection instantiated according to the databaseEngine.
        /// </summary>
        /// <returns>
        /// A closed IDBConnection instantiated according to the database engine
        /// </returns>
        protected IDbConnection GetIDbConnection()
        {
            return DataObjectProvider.GetIDbConnection(_connectionString);
        }

        public IList<TEntity> GetAll()
        {
            return GetAll(null, null);
        }

        public IList<TEntity> GetAll(IDbTransaction paramDbTransaction)
        {
            return GetAll(null, paramDbTransaction);
        }
        public IList<TEntity> GetAll(IDbConnection paramDbConnection)
        {
            return GetAll(paramDbConnection, null);
        }

        public IList<TEntity> GetAll(IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            IList<TEntity> collection = new List<TEntity>();

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = string.Format("SELECT {0} FROM {1}", ColumnsForSelect, _syntaxProvider.GetSecureViewName(ViewName));

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                IDataReader reader = dbCommand.ExecuteReader();
                collection = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo, _dataBaseEngine);

                reader.Close(); reader.Dispose();
                dbCommand.Dispose();
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException(exception, dbCommand);
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

        public IList<TEntity> GetListByLambdaExpressionFilter(Expression<Func<TEntity, bool>> lambdaExpressionFilter, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            IList<TEntity> result = new List<TEntity>();
            SetSqlLamAdapter();
            var query = new SqlLam<TEntity>(lambdaExpressionFilter);

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? GetIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                if (paramDbTransaction != null)
                    dbCommand.Transaction = paramDbTransaction;

                dbCommand.CommandText = query.QueryString;

                foreach (var parameters in query.QueryParameters)
                    dbCommand.Parameters.Add(CreateIDbDataParameter("@" + parameters.Key, parameters.Value));

                if (_dataBaseEngine == EDataBaseEngine.OleDb)
                    ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                IDataReader reader = dbCommand.ExecuteReader();
                result = new MapDataReaderToEntity().Map<TEntity>(reader, ListObjectPropertyInfo, _dataBaseEngine);

                reader.Close();
                reader.Dispose();
                dbCommand.Dispose();
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException(exception, dbCommand);
            }
            finally
            {
                if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }

            return result;
        }

        private void BaseGenericViewManagerConstructorResolver(string connectionString, EDataBaseEngine dataBaseEngine)
        {
            _connectionString = connectionString;
            _dataBaseEngine = dataBaseEngine;
            DataObjectProvider = new DbObjectProvider(_dataBaseEngine);
            _syntaxProvider = new SyntaxProvider(_dataBaseEngine);

            TEntity entity = new TEntity();
            ListObjectPropertyInfo = GetPropertyInfoList(entity);

            ViewName = GetViewNameFromEntity(entity.GetType());

            if ((ListObjectPropertyInfo == null) || (ListObjectPropertyInfo.Count == 0))
            {
                throw new Exception(string.Format("The Entity {0}, has not linked attibutes to table: {1}, Use [DbColumn] attribute to link properties to the table.", entity.GetType().Name, ViewName));
            }

            GetDbColumns();
        }

        private string GetViewNameFromEntity(Type type)
        {
            var view = type.GetCustomAttributes(false).OfType<DbViewAttribute>().FirstOrDefault();
            if (view != null)
                return view.Name;
            else
                return type.Name;
        }
        private IList<PropertyInfo> GetPropertyInfoList(TEntity entity)
        {
            return entity.GetType().GetProperties().Where(p => p.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DbViewAttribute)) != null).ToList();
        }

        private void GetDbColumns()
        {
            TEntity entity = new TEntity();

            StringBuilder sbColumnsSelect = new StringBuilder();

            using (DbConnection conn = GetDbConnection())
            {
                conn.Open();

                foreach (PropertyInfo i in ListObjectPropertyInfo)
                {
                    var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                    string columnName = _syntaxProvider.GetSecureColumnName(customAttribute.ColumnName ?? i.Name);

                    sbColumnsSelect.Append(string.Format("{0},", columnName));
                }
            }

            if (sbColumnsSelect.Length > 0)
                sbColumnsSelect.Remove(sbColumnsSelect.Length - 1, 1);

            ColumnsForSelect = sbColumnsSelect.ToString();
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
                throw new ArgumentOutOfRangeException("The databaseEngine is not supported by this method");
        }
    }
}
