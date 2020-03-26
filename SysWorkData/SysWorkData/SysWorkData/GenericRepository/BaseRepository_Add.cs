using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericRepository.Attributes;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.DbInfo;
using SysWork.Data.GenericRepository.Interfaces.Actions;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IAdd<TEntity>
    {
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
        ///public class PersonRepository: BaseRepository<Person>
        ///{
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var personRepository = new PersonRepository<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///
        ///         long id = PersonRepository.Add(p,out string errMessage);
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
            return Add(entity, out errMessage, null);
        }

        public long Add(TEntity entity, out string errMessage, int commandTimeOut)
        {
            return Add(entity, out errMessage, commandTimeOut);
        }

        private long Add(TEntity entity, out string errMessage, int? commandTimeOut)
        {
            errMessage = "";
            long identity = 0;

            try
            {
                identity = Add(entity, null, null, commandTimeOut);
            }
            catch (RepositoryException genericRepositoryException)
            {
                errMessage = genericRepositoryException.OriginalException.Message;
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
        /// <exception cref="RepositoryException"></exception>
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
        ///public class PersonRepository: BaseRepository<Person>
        ///{
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var personRepository = new PersonRepository<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///         
        ///         try
        ///         {
        ///             long id = PersonRepository.Add(p);
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
            return Add(entity, null, null, null);
        }

        public long Add(TEntity entity, int commandTimeOut)
        {
            return Add(entity, null, null, commandTimeOut);
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
        /// <exception cref="RepositoryException"></exception>
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
        ///public class PersonRepository: BaseRepository<Person>
        ///{
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var personRepository = new PersonRepository<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///         
        ///         // Get new Connection.
        ///         var extConnection = PersonRepository.GetDbConnection();
        ///         extConnection.Open();
        ///         
        ///         try
        ///         {
        ///             long id = PersonRepository.Add(p,extConnection);
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
            return Add(entity, paramDbConnection, null, null);
        }

        public long Add(TEntity entity, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return Add(entity, paramDbConnection, null, commandTimeOut);
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
        /// <exception cref="RepositoryException"></exception>
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
        ///public class PersonRepository: BaseRepository<Person>
        ///{
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var personRepository = new PersonRepository<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///         
        ///         // Get new Connection.
        ///         var extConnection = PersonRepository.GetDbConnection();
        ///         extConnection.Open();
        ///         
        ///         var extTransaction = extConnection.BeginTransaction();
        ///         try
        ///         {
        ///             long id = PersonRepository.Add(p,extTransaction);
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
            return Add(entity, null, paramDbTransaction, null);
        }

        public long Add(TEntity entity, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return Add(entity, null, paramDbTransaction, commandTimeOut);
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
        /// <exception cref="RepositoryException"></exception>
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
        ///public class PersonRepository: BaseRepository<Person>
        ///{
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var personRepository = new PersonRepository<Person>(myConnectionString,EDatabaseEngine.MSSqlServer);
        ///         var p = new Person();
        ///         p.LastName = "Martinez";
        ///         p.FirstName = "Diego";
        ///         p.Passport = "AR00127296";
        ///         
        ///         // Get new Connection.
        ///         var extConnection = PersonRepository.GetDbConnection();
        ///         extConnection.Open();
        ///         
        ///         var extTransaction = extConnection.BeginTransaction();
        ///         try
        ///         {
        ///             long id = PersonRepository.Add(p,extConnection,extTransaction);
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
        public long Add(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return Add(entity, paramDbConnection, paramDbTransaction, null);
        }

        public long Add(TEntity entity, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int? commandTimeOut)
        {
            long identity = 0;
            bool hasIdentity = false;

            bool closeConnection = ((paramDbConnection == null) && (paramDbTransaction == null));

            if (paramDbConnection == null && paramDbTransaction != null)
                paramDbConnection = paramDbTransaction.Connection;

            IDbConnection dbConnection = paramDbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

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
                    throw new RepositoryException(exception, dbCommand);
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
    }
}
