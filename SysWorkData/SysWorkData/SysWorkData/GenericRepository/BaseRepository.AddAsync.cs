using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.Common.Utilities;
using SysWork.Data.Common.Attributes;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.Common.DbInfo;
using SysWork.Data.Common.ValueObjects;
using System.Threading.Tasks;
using System.Data.Common;

namespace SysWork.Data.GenericRepository
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class BaseRepository<TEntity>
    {
        #region DOCUMENTATION Add(TEntity entity)
        /// <summary>
        /// Adds a record.
        /// </summary>
        /// <remarks>
        /// Adds a record.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.Common.Attributes;
        /// using SysWork.Data.GenericRepository.Exceptions;
        /// 
        /// [DbTable(Name = "Persons")]
        /// public class Person
        /// {
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Address { get; set; }
        ///
        ///     [DbColumn()]
        ///     public long? IdState { get; set; }
        ///
        ///     [DbColumn()]
        ///     public DateTime? BirthDate { get; set; }
        ///
        ///     [DbColumn(ColumnName = "Long Name Field")]
        ///     public string LongNameField { get; set; }
        ///
        ///     [DbColumn()]
        ///     public bool Active { get; set; }
        /// }
        /// 
        /// public class PersonRepository: BaseRepository<Person>
        /// {
        ///     public PersonRepository(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
        ///     {
        ///     }
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     public DbExecutor GetDbExecutor()
        ///     {
        ///         return BaseDbExecutor();
        ///     }
        ///     public DbConnection GetDbConnection()
        ///     {
        ///         return BaseDbConnection();
        ///     }
        /// }
        /// 
        /// public class Sample
        /// {
        ///     static static void Main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address";
        ///         p.BirthDate = new DateTime(1980,5,24);
        ///         p.Active = true;
        ///         
        ///         try
        ///         {
        ///             long id = personRepository.Add(p);
        ///             Console.WriteLine($"The generated id is{id}");
        ///         }
        ///         catch (RepositoryException ex)
        ///         {
        ///             Console.WriteLine($"The following error has occurred{ex.Message}");
        ///         }
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        #endregion
        public async Task<long> AddAsync(TEntity entity)
        {
            return await AddAsync(entity, null, null, null);
        }

        #region DOCUMENTATION Add(TEntity entity, int commandTimeOut)
        /// <summary>
        /// Adds a record and use a custom dbCommand timeout.
        /// </summary>
        /// <remarks>
        /// Adds a record and use a custom dbCommand timeout.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.Common.Attributes;
        /// using SysWork.Data.GenericRepository.Exceptions;
        /// 
        ///[DbTable(Name = "Persons")]
        ///public class Person
        ///{
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Address { get; set; }
        ///
        ///     [DbColumn()]
        ///     public long? IdState { get; set; }
        ///
        ///     [DbColumn()]
        ///     public DateTime? BirthDate { get; set; }
        ///
        ///     [DbColumn(ColumnName = "Long Name Field")]
        ///     public string LongNameField { get; set; }
        ///
        ///     [DbColumn()]
        ///     public bool Active { get; set; }
        /// }
        /// 
        /// public class PersonRepository: BaseRepository<Person>
        /// {
        ///     public PersonRepository(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
        ///     {
        ///     }
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     public DbExecutor GetDbExecutor()
        ///     {
        ///         return BaseDbExecutor();
        ///     }
        ///     public DbConnection GetDbConnection()
        ///     {
        ///         return BaseDbConnection();
        ///     }
        /// }
        /// 
        /// public class Sample
        /// {
        ///     static void Main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address";
        ///         p.BirthDate = new DateTime(1980,5,24);
        ///         p.Active = true;
        ///         
        ///         int commandTimeOut = 60;
        ///         
        ///         try
        ///         {
        ///             long id = personRepository.Add(p, commandTimeOut);
        ///             Console.WriteLine($"The generated id is{id}");
        ///         }
        ///         catch (RepositoryException ex)
        ///         {
        ///             Console.WriteLine($"The following error has occurred{ex.Message}");
        ///         }
        ///     }
        /// }
        /// ]]>
        ///</code>
        ///</example>
        #endregion
        public async Task<long> AddAsync(TEntity entity, int commandTimeOut)
        {
            return await AddAsync(entity, null, null, commandTimeOut);
        }

        #region DOCUMENTATION Add(TEntity entity, IDbConnection dbConnection)
        /// <summary>
        /// Adds a record using an DbConnection.
        /// </summary>
        /// <remarks>
        /// Adds a record using an DbConnection.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.If is null, a new one will be created and closed on completion.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.Common.Attributes;
        /// using SysWork.Data.GenericRepository.Exceptions;
        /// 
        /// [DbTable(Name = "Persons")]
        /// public class Person
        /// {
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Address { get; set; }
        ///
        ///     [DbColumn()]
        ///     public long? IdState { get; set; }
        ///
        ///     [DbColumn()]
        ///     public DateTime? BirthDate { get; set; }
        ///
        ///     [DbColumn(ColumnName = "Long Name Field")]
        ///     public string LongNameField { get; set; }
        ///
        ///     [DbColumn()]
        ///     public bool Active { get; set; }
        /// }
        /// 
        /// public class PersonRepository: BaseRepository<Person>
        /// {
        ///     public PersonRepository(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
        ///     {
        ///     }
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     public DbExecutor GetDbExecutor()
        ///     {
        ///         return BaseDbExecutor();
        ///     }
        ///     public DbConnection GetDbConnection()
        ///     {
        ///         return BaseDbConnection();
        ///     }
        /// }
        /// 
        /// public class Sample
        /// {
        ///     static void Main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address";
        ///         p.BirthDate = new DateTime(1980,5,24);
        ///         p.Active = true;
        ///         
        ///         int commandTimeOut = 60;
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         try
        ///         {
        ///             long id = personRepository.Add(p, dbConnection);
        ///             Console.WriteLine($"The generated id is{id}");
        ///         }
        ///         catch (RepositoryException ex)
        ///         {
        ///             Console.WriteLine($"The following error has occurred{ex.Message}");
        ///         }
        ///         finally
        ///         {
        ///             dbConnection.Close();
        ///             dbConnection.Dispose();
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        #endregion
        public async Task<long> AddAsync(TEntity entity, DbConnection dbConnection)
        {
            return await AddAsync(entity, dbConnection, null, null);
        }

        #region DOCUMENTATION Add(TEntity entity, IDbConnection dbConnection, int commandTimeOut)
        /// <summary>
        /// Adds a record using an DbConnection and custom dbCommand timeout.
        /// </summary>
        /// <remarks>
        /// Adds a record using an DbConnection and custom dbCommand timeout.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.If is null, a new one will be created and closed on completion.</param>
        /// <param name="commandTimeOut">The command timeout.If is null, DefaultCommandTimeout will be used.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.Common.Attributes;
        /// using SysWork.Data.GenericRepository.Exceptions;
        /// 
        /// [DbTable(Name = "Persons")]
        /// public class Person
        /// {
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Address { get; set; }
        ///
        ///     [DbColumn()]
        ///     public long? IdState { get; set; }
        ///
        ///     [DbColumn()]
        ///     public DateTime? BirthDate { get; set; }
        ///
        ///     [DbColumn(ColumnName = "Long Name Field")]
        ///     public string LongNameField { get; set; }
        ///
        ///     [DbColumn()]
        ///     public bool Active { get; set; }
        /// }
        /// 
        /// public class PersonRepository: BaseRepository<Person>
        /// {
        ///     public PersonRepository(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
        ///     {
        ///     }
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     public DbExecutor GetDbExecutor()
        ///     {
        ///         return BaseDbExecutor();
        ///     }
        ///     public DbConnection GetDbConnection()
        ///     {
        ///         return BaseDbConnection();
        ///     }
        /// }
        /// 
        /// public class Sample
        /// {
        ///     static void Main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address";
        ///         p.BirthDate = new DateTime(1980,5,24);
        ///         p.Active = true;
        ///         
        ///         int commandTimeOut = 60;
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         try
        ///         {
        ///             long id = personRepository.Add(p, dbConnection, commandTimeOut);
        ///             Console.WriteLine($"The generated id is{id}");
        ///         }
        ///         catch (RepositoryException ex)
        ///         {
        ///             Console.WriteLine($"The following error has occurred{ex.Message}");
        ///         }
        ///         finally
        ///         {
        ///             dbConnection.Close();
        ///             dbConnection.Dispose();
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        #endregion
        public async Task<long> AddAsync(TEntity entity, DbConnection dbConnection, int commandTimeOut)
        {
            return await AddAsync(entity, dbConnection, null, commandTimeOut);
        }

        #region DOCUMENTATION Add(TEntity entity, IDbTransaction dbTransaction)
        /// <summary>
        /// Adds a record using an DbTransaction.
        /// </summary>
        /// <remarks>
        /// Adds a record using an DbTransaction.
        /// 
        /// The connection from the dbTransaction will be used. The connection will not close at the end.
        /// 
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction">The database transaction.If dbConnection is null and dbTransaction is not null, the connection from the dbTransaction will be used.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.Common.Attributes;
        /// using SysWork.Data.GenericRepository.Exceptions;
        /// 
        /// [DbTable(Name = "Persons")]
        /// public class Person
        /// {
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Address { get; set; }
        ///
        ///     [DbColumn()]
        ///     public long? IdState { get; set; }
        ///
        ///     [DbColumn()]
        ///     public DateTime? BirthDate { get; set; }
        ///
        ///     [DbColumn(ColumnName = "Long Name Field")]
        ///     public string LongNameField { get; set; }
        ///
        ///     [DbColumn()]
        ///     public bool Active { get; set; }
        /// }
        /// 
        /// public class PersonRepository: BaseRepository<Person>
        /// {
        ///     public PersonRepository(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
        ///     {
        ///     }
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     public DbExecutor GetDbExecutor()
        ///     {
        ///         return BaseDbExecutor();
        ///     }
        ///     public DbConnection GetDbConnection()
        ///     {
        ///         return BaseDbConnection();
        ///     }
        /// }
        /// 
        /// public class Sample
        /// {
        ///     static void Main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address";
        ///         p.BirthDate = new DateTime(1980,5,24);
        ///         p.Active = true;
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             long id = personRepository.Add(p, dbTransaction);
        ///             dbTransaction.Commit();
        ///             Console.WriteLine($"The generated id is{id}");
        ///         }
        ///         catch (RepositoryException ex)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred{ex.Message}");
        ///         }
        ///         finally
        ///         {
        ///             dbConnection.Close();
        ///             dbConnection.Dispose();
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        #endregion
        public async Task<long> AddAsync(TEntity entity, DbTransaction dbTransaction)
        {
            return await AddAsync(entity, null, dbTransaction, null);
        }

        #region DOCUMENTATION Add(TEntity entity, IDbTransaction dbTransaction, int commandTimeOut)
        /// <summary>
        /// Adds a record using an DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <remarks>
        /// Adds a record using an DbTransaction and custom dbCommand timeout.
        /// 
        /// The connection from the dbTransaction will be used. The connection will not close at the end.
        /// 
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="dbTransaction">The database transaction.If dbConnection is null and dbTransaction is not null, the connection from the dbTransaction will be used.</param>
        /// <param name="commandTimeOut">The command timeout.If is null, DefaultCommandTimeout will be used.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.Common.Attributes;
        /// using SysWork.Data.GenericRepository.Exceptions;
        /// 
        /// [DbTable(Name = "Persons")]
        /// public class Person
        /// {
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Address { get; set; }
        ///
        ///     [DbColumn()]
        ///     public long? IdState { get; set; }
        ///
        ///     [DbColumn()]
        ///     public DateTime? BirthDate { get; set; }
        ///
        ///     [DbColumn(ColumnName = "Long Name Field")]
        ///     public string LongNameField { get; set; }
        ///
        ///     [DbColumn()]
        ///     public bool Active { get; set; }
        /// }
        /// 
        /// public class PersonRepository: BaseRepository<Person>
        /// {
        ///     public PersonRepository(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
        ///     {
        ///     }
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     public DbExecutor GetDbExecutor()
        ///     {
        ///         return BaseDbExecutor();
        ///     }
        ///     public DbConnection GetDbConnection()
        ///     {
        ///         return BaseDbConnection();
        ///     }
        /// }
        /// 
        /// public class Sample
        /// {
        ///     static void Main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address";
        ///         p.BirthDate = new DateTime(1980,5,24);
        ///         p.Active = true;
        ///         
        ///         int commandTimeOut = 60;
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             long id = personRepository.Add(p, dbTransaction, commandTimeOut);
        ///             dbTransaction.Commit();
        ///             Console.WriteLine($"The generated id is{id}");
        ///         }
        ///         catch (RepositoryException ex)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred{ex.Message}");
        ///         }
        ///         finally
        ///         {
        ///             dbConnection.Close();
        ///             dbConnection.Dispose();
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        #endregion
        public async Task<long> AddAsync(TEntity entity, DbTransaction dbTransaction, int commandTimeOut)
        {
            return await AddAsync(entity, null, dbTransaction, commandTimeOut);
        }

        #region DOCUMENTATION Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        /// <summary>
        /// Adds a record using an DbConnection and DbTransaction.
        /// </summary>
        /// <remarks>
        /// Adds a record using an DbConnection and DbTransaction.
        /// 
        /// If dbConnection is null, a new one will be created and closed on completion.
        /// If dbConnection is null and dbTransaction is not null, the connection from the dbTransaction will be used. The connection will not close at the end.
        /// If dbTransaction is null, no transaction will be used in the dbCommand.
        /// 
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.If is null, a new one will be created and closed on completion</param>
        /// <param name="dbTransaction">The database transaction.If dbConnection is null and dbTransaction is not null, the connection from the dbTransaction will be used</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.Common.Attributes;
        /// using SysWork.Data.GenericRepository.Exceptions;
        /// 
        /// [DbTable(Name = "Persons")]
        /// public class Person
        /// {
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Address { get; set; }
        ///
        ///     [DbColumn()]
        ///     public long? IdState { get; set; }
        ///
        ///     [DbColumn()]
        ///     public DateTime? BirthDate { get; set; }
        ///
        ///     [DbColumn(ColumnName = "Long Name Field")]
        ///     public string LongNameField { get; set; }
        ///
        ///     [DbColumn()]
        ///     public bool Active { get; set; }
        /// }
        /// 
        /// public class PersonRepository: BaseRepository<Person>
        /// {
        ///     public PersonRepository(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
        ///     {
        ///     }
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     public DbExecutor GetDbExecutor()
        ///     {
        ///         return BaseDbExecutor();
        ///     }
        ///     public DbConnection GetDbConnection()
        ///     {
        ///         return BaseDbConnection();
        ///     }
        /// }
        /// 
        /// public class Sample
        /// {
        ///     static void Main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address";
        ///         p.BirthDate = new DateTime(1980,5,24);
        ///         p.Active = true;
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             long id = personRepository.Add(p, dbConnection, dbTransaction);
        ///             dbTransaction.Commit();
        ///             Console.WriteLine($"The generated id is{id}");
        ///         }
        ///         catch (RepositoryException ex)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred{ex.Message}");
        ///         }
        ///         finally
        ///         {
        ///             dbConnection.Close();
        ///             dbConnection.Dispose();
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        #endregion
        public async Task<long> AddAsync(TEntity entity, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            return await AddAsync(entity, dbConnection, dbTransaction, null);
        }

        #region DOCUMENTATION Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        /// <summary>
        /// Adds a record using an DbConnection, DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <remarks>
        /// Adds a record using an DbConnection and DbTransaction and custom dbCommand timeout.
        /// 
        /// If dbConnection is null, a new one will be created and closed on completion.
        /// If dbConnection is null and dbTransaction is not null, the connection from the dbTransaction will be used. The connection will not close at the end.
        /// If dbTransaction is null, no transaction will be used in the dbCommand.
        /// If commandTimeout is null, DefaultCommandTimeout will be used.
        /// 
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.If is null, a new one will be created and closed on completion.</param>
        /// <param name="dbTransaction">The database transaction.If dbConnection is null and dbTransaction is not null, the connection from the dbTransaction will be used.</param>
        /// <param name="commandTimeOut">The command timeout.If is null, DefaultCommandTimeout will be used.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.Common.Attributes;
        /// using SysWork.Data.GenericRepository.Exceptions;
        /// 
        /// [DbTable(Name = "Persons")]
        /// public class Person
        /// {
        ///     [DbColumn(IsIdentity = true, IsPrimary = true)]
        ///     public long IdPerson { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string FirstName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string LastName { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Passport { get; set; }
        ///
        ///     [DbColumn()]
        ///     public string Address { get; set; }
        ///
        ///     [DbColumn()]
        ///     public long? IdState { get; set; }
        ///
        ///     [DbColumn()]
        ///     public DateTime? BirthDate { get; set; }
        ///
        ///     [DbColumn(ColumnName = "Long Name Field")]
        ///     public string LongNameField { get; set; }
        ///
        ///     [DbColumn()]
        ///     public bool Active { get; set; }
        /// }
        /// 
        /// public class PersonRepository: BaseRepository<Person>
        /// {
        ///     public PersonRepository(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
        ///     {
        ///     }
        ///     public Person GetByPassport(string passport)
        ///     {
        ///         return GetByLambdaExpressionFilter(entity => (entity.Passport == passport));
        ///     }
        ///     public DbExecutor GetDbExecutor()
        ///     {
        ///         return BaseDbExecutor();
        ///     }
        ///     public DbConnection GetDbConnection()
        ///     {
        ///         return BaseDbConnection();
        ///     }
        /// }
        /// 
        /// public class Sample
        /// {
        ///     static void Main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address";
        ///         p.BirthDate = new DateTime(1980,5,24);
        ///         p.Active = true;
        ///         
        ///         int commandTimeOut = 60;
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             long id = personRepository.Add(p, dbConnection, dbTransaction, commandTimeOut );
        ///             dbTransaction.Commit();
        ///             Console.WriteLine($"The generated id is{id}");
        ///         }
        ///         catch (RepositoryException ex)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred{ex.Message}");
        ///         }
        ///         finally
        ///         {
        ///             dbConnection.Close();
        ///             dbConnection.Dispose();
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        #endregion
        public async Task<long> AddAsync(TEntity entity, DbConnection dbConnection, DbTransaction dbTransaction, int? commandTimeOut)
        {
            long identity = 0;
            bool hasIdentity = false;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            DbConnection dbConnectionInUse = dbConnection ?? BaseDbConnection();
            DbCommand dbCommand = dbConnectionInUse.CreateCommand();

            StringBuilder parameterList = new StringBuilder();
            foreach (PropertyInfo i in EntityProperties)
            {
                var dbColumn = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                if (!dbColumn.IsIdentity)
                {
                    string parameterName = "@param_" + i.Name;

                    parameterList.Append(string.Format("{0},", parameterName));

                    DbColumnInfo cdbi = (DbColumnInfo)_columnListWithDbInfo[i.Name];

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
                    dbCommand.CommandText = insertQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

                    if (_databaseEngine == EDatabaseEngine.OleDb)
                        ((OleDbCommand)dbCommand).ConvertNamedParametersToPositionalParameters();

                    if (dbConnectionInUse.State != ConnectionState.Open)
                        await dbConnectionInUse.OpenAsync();

                    if (hasIdentity)
                    {
                        if (_databaseEngine == EDatabaseEngine.OleDb)
                        {
                            // In case of OleDbConnection, exec the command and immediately, 
                            // exec new query to obtain the identity;
                            await dbCommand.ExecuteNonQueryAsync();

                            dbCommand.CommandText = "Select @@Identity";
                            identity = DbUtil.ParseToLong(await dbCommand.ExecuteScalarAsync());
                        }
                        else
                        {
                            identity = DbUtil.ParseToLong(await dbCommand.ExecuteScalarAsync());
                        }
                    }
                    else
                    {
                        await dbCommand.ExecuteNonQueryAsync();
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
                    if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnectionInUse.Close();
                        dbConnectionInUse.Dispose();
                    }
                }
            }
            return identity;
        }
    }
}
