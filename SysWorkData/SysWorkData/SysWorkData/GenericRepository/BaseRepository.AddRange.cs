using System;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using SysWork.Data.Common;
using SysWork.Data.Common.Extensions.OleDbCommandExtensions;
using SysWork.Data.GenericRepository.Attributes;
using SysWork.Data.GenericRepository.Exceptions;
using SysWork.Data.GenericRepository.DbInfo;
using SysWork.Data.GenericRepository.Interfaces.Actions;
using System.Collections.Generic;
using System.Data.Common;

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IAddRange<TEntity>
    {
        public bool AddRange(IList<TEntity> entities)
        {
            return AddRange(entities, null, null, out long recordsAffected, out IEnumerable<object> addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, int commandTimeOut)
        {
            return AddRange(entities, null, null, out long recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, null, null, out long recordsAffected, out addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, out IEnumerable<object> addedIds, int commandTimeOut)
        {
            return AddRange(entities, null, null, out long recordsAffected, out addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, out long recordsAffected)
        /// <summary>
        /// Adds a list of records. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, out long recordsAffected);
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///             
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
        ///         }
        ///         
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        #endregion
        public bool AddRange(IList<TEntity> entities, out long recordsAffected)
        {
            return AddRange(entities, null, null, out recordsAffected, out IEnumerable<object> addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, out long recordsAffected, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using a custom dbCommand timeout. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, out long recordsAffected, commandTimeOut);
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///             
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
        ///         }
        ///         
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        #endregion
        public bool AddRange(IList<TEntity> entities, out long recordsAffected, int commandTimeOut)
        {
            return AddRange(entities, null, null, out recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection)
        /// <summary>
        /// Adds a list of records using an DbConnection.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection);
        ///             Console.WriteLine("Records added succefully");
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection)
        {
            return AddRange(entities, dbConnection, null, out long recordsAffected, out IEnumerable<object> addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using an DbConnection and a custom dbCommand timeout. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         int commandTimeOut = 60;
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, commandTimeOut);
        ///             Console.WriteLine("Records added succefully");
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, int commandTimeOut)
        {
            return AddRange(entities, dbConnection, null, out long recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, out IEnumerable<object> addedIds)
        /// <summary>
        /// Adds a list of records using an DbConnection and a custom dbCommand timeout. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, out IEnumerable<object> addedIds);
        ///             
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, dbConnection, null, out long recordsAffected, out addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, out IEnumerable<object> addedIds, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using an DbConnection and a custom dbCommand timeout. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         int commandTimeOut = 60;
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, out IEnumerable<object> addedIds, commandTimeOut);
        ///             
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out IEnumerable<object> addedIds, int commandTimeOut)
        {
            return AddRange(entities, dbConnection, null, out long recordsAffected, out addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, out long recordsAffected)
        /// <summary>
        /// Adds a list of records using an DbConnection. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         int commandTimeOut = 60;
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, out long recordsAffected);
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///             
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out long recordsAffected)
        {
            return AddRange(entities, dbConnection, null, out recordsAffected, out IEnumerable<object> addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, out long recordsAffected, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using an DbConnection and a custom dbCommand timeout. Out parameter long returns the count of the records added. .
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         int commandTimeOut = 60;
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, out long recordsAffected, commandTimeOut);
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///             
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, out long recordsAffected, int commandTimeOut)
        {
            return AddRange(entities, dbConnection, null, out recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbTransaction dbTransaction)
        /// <summary>
        /// Adds a list of records using an dbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbTransaction);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine("The list was added succefully");
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction)
        {
            return AddRange(entities, null, dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using an dbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
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
        ///             bool result = personRepository.AddRange(entityList, dbTransaction, commandTimeOut);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return AddRange(entities, null, dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out IEnumerable<object> addedIds)
        /// <summary>
        /// Adds a list of records using an dbTransaction. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbTransaction, out IEnumerable<object> addedIds);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, null, dbTransaction, out long recordsAffected, out addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out IEnumerable<object> addedIds, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using an dbTransaction and custom dbCommand timeout. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
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
        ///             bool result = personRepository.AddRange(entityList, dbTransaction, out IEnumerable<object> addedIds, commandTimeOut);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out IEnumerable<object> addedIds, int commandTimeOut)
        {
            return AddRange(entities, null, dbTransaction, out long recordsAffected, out addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out long recordsAffected)
        /// <summary>
        /// Adds a list of records using an dbTransaction and a custom dbCommand timeout. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbTransaction, out long recordsAffected);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out long recordsAffected)
        {
            return AddRange(entities, null, dbTransaction, out recordsAffected, out IEnumerable<object> addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using an dbTransaction and a custom dbCommand timeout. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
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
        ///             bool result = personRepository.AddRange(entityList, dbTransaction, out long recordsAffected, commandTimeOut);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut)
        {
            return AddRange(entities, null, dbTransaction, out recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction)
        /// <summary>
        /// Adds a list of records using an DbConnection and dbTransaction.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, dbTransaction);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return AddRange(entities, dbConnection, dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using an DbConnection, dbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         int commandTimeOut = 60;
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, dbTransaction, commandTimeOut);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return AddRange(entities, dbConnection, dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out IEnumerable<object> addedIds)
        /// <summary>
        /// Adds a list of records using an DbConnection, dbTransaction and custom dbCommand timeout. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, dbTransaction, out IEnumerable<object> addedIds);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, dbConnection, dbTransaction, out long recordsAffected, out addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out IEnumerable<object> addedIds, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using an DbConnection, dbTransaction and custom dbCommand timeout. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         int commandTimeOut = 60;
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, dbTransaction, out IEnumerable<object> addedIds, commandTimeOut);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out IEnumerable<object> addedIds, int commandTimeOut)
        {
            return AddRange(entities, dbConnection, dbTransaction, out long recordsAffected, out addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected)
        /// <summary>
        /// Adds a list of records using an DbConnection and dbTransaction. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, dbTransaction, out long recordsAffected);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected)
        {
            return AddRange(entities, dbConnection, dbTransaction, out recordsAffected, out IEnumerable<object> addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut)
        /// <summary>
        /// Adds a list of records using an DbConnection, dbTransaction and a custom dbCommand timeout. Out parameter long returns the count of the records added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         int commandTimeOut = 60;
        /// 
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, dbTransaction, out long recordsAffected, commandTimeOut);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, int commandTimeOut)
        {
            return AddRange(entities, dbConnection, dbTransaction, out recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds)
        /// <summary>
        /// Adds a list of records using an DbConnection and dbTransaction. Out parameter long returns the count of the records added. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, dbConnection, dbTransaction, out recordsAffected, out addedIds, null);
        }

        #region DOCUMENTATION AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, int? commandTimeOut)
        /// <summary>
        /// Adds a list of records using an DbConnection, dbTransaction and custom dbCommand timeout. Out parameter long returns the count of the records added. Out parameter IEnumerable returns the Ids of the entities added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="recordsAffected">The records affected.</param>
        /// <param name="addedIds">The added ids.</param>
        /// <param name="commandTimeOut">The command time out.</param>
        /// <returns></returns>
        /// <example>
        /// <code><![CDATA[
        /// using System;
        /// using System.Data.Common;
        /// using SysWork.Data.Common;
        /// using SysWork.Data.Common.Utilities;
        /// using SysWork.Data.GenericRepository;
        /// using SysWork.Data.GenericRepository.Attributes;
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
        ///     public PersonRepository(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
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
        ///         var databaseEngine = EDataBaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository(connectionString, databaseEngine);
        ///         
        ///         var entityList = new List<Person>();
        ///         
        ///         entityList.Add(new Person { FirstName = "Diego", LastName = "Martinez", Passport = "AR00177286", LongNameField = "Long Field", Address = "Address 555",BirthDate = new DateTime(1980,5,24), Active = True });
        ///         entityList.Add(new Person { FirstName = "Cosme", LastName = "Fulanito", Passport = "AR00122216", LongNameField = "Long Field", Address = "Address 444",BirthDate = new DateTime(1990,4,26), Active = True });
        ///         entityList.Add(new Person { FirstName = "Jhon", LastName = "Perez", Passport = "AR00333333", LongNameField = "Long Field", Address = "Address 333",BirthDate = new DateTime(2000,3,1), Active = True });
        ///         
        ///         int commandTimeOut = 60;
        ///         var dbConnection = personRepository.GetDbConnection();
        ///         dbConnection.Open();
        ///         
        ///         var dbTransaction = dbConnection.BeginTransaction();
        ///         
        ///         try
        ///         {
        ///             bool result = personRepository.AddRange(entityList, dbConnection, dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        ///             dbTransaction.Commit();
        ///             
        ///             Console.WriteLine($"RecordsAffected: {recordsAffected}");
        ///             Console.WriteLine("Added ids:");
        ///             foreach (var id in addedIds)
        ///             {
        ///                 Console.WriteLine($"{id.ToString()}");
        ///             }
        ///         }
        ///         catch(RepositoryException re)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.WriteLine($"The following error has occurred: {re.OriginalException.Message}");
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
        public bool AddRange(IList<TEntity> entities, IDbConnection dbConnection, IDbTransaction dbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, int? commandTimeOut)
        {
            recordsAffected = 0;
            var idList = new List<object>();
            bool hasIdentity = false;
            addedIds = null;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand;

            try
            {
                if (dbConnectionInUse.State != ConnectionState.Open)
                    dbConnectionInUse.Open();
            }
            catch (Exception connectionException)
            {
                throw new RepositoryException(connectionException);
            }

            foreach (TEntity entity in entities)
            {
                string parameterList = "";
                dbCommand = dbConnectionInUse.CreateCommand();
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                string addRangeQuery = "";
                foreach (PropertyInfo i in ListObjectPropertyInfo)
                {
                    var customAttribute = i.GetCustomAttribute(typeof(DbColumnAttribute)) as DbColumnAttribute;
                    if (!customAttribute.IsIdentity)
                    {
                        hasIdentity = true;

                        string parameterName = "@param_" + i.Name;
                        parameterList += string.Format("{0},", parameterName);

                        ColumnDbInfo cdbi = (ColumnDbInfo)_columnListWithDbInfo[i.Name];
                        dbCommand.Parameters.Add(CreateIDbDataParameter(parameterName, cdbi.DbType, i.GetValue(entity), cdbi.MaxLenght));
                    }
                }

                if (parameterList != string.Empty)
                {
                    parameterList = parameterList.Substring(0, parameterList.Length - 1);
                    addRangeQuery = (string.Format("INSERT INTO {0} ( {1} ) VALUES ( {2} ) {3};", _syntaxProvider.GetSecureTableName(TableName), ColumnsForInsert, parameterList, _syntaxProvider.GetSubQueryGetIdentity()));
                }

                try
                {
                    dbCommand.CommandText = addRangeQuery;
                    if (dbTransaction != null)
                        dbCommand.Transaction = dbTransaction;

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
                            idList.Add(dbCommand.ExecuteScalar());
                        }
                        else
                        {
                            idList.Add(dbCommand.ExecuteScalar());
                        }
                        recordsAffected++;
                    }
                    else
                    {
                        recordsAffected += dbCommand.ExecuteNonQuery();
                    }

                    dbCommand.Dispose();
                }
                catch (Exception commandException)
                {
                    // In case of exception, if the command is open, close it.
                    if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
                    {
                        if (dbConnectionInUse.State == ConnectionState.Open)
                            dbConnectionInUse.Close();

                        dbConnectionInUse.Dispose();
                    }

                    throw new RepositoryException(commandException, dbCommand);
                }
            }

            addedIds = idList;

            if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
            {
                dbConnectionInUse.Close();
                dbConnectionInUse.Dispose();
            }

            return true;
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage)
        {
            return AddRange(entities, out errMessage, out IEnumerable<object> addedIds, out long recordsAffected, null);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage, int commandTimeOut)
        {
            return AddRange(entities, out errMessage, out IEnumerable<object> addedIds, out long recordsAffected, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, out errMessage, out addedIds, out long recordsAffected, null);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds, int commandTimeOut)
        {
            return AddRange(entities, out errMessage, out addedIds, out long recordsAffected, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage, out long recordsAffected)
        {
            return AddRange(entities, out errMessage, out IEnumerable<object> addedIds, out recordsAffected, null);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage, out long recordsAffected, int commandTimeOut)
        {
            return AddRange(entities, out errMessage, out IEnumerable<object> addedIds, out recordsAffected, commandTimeOut);
        }

        private bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds, out long recordsAffecteds, int? commandTimeOut)
        {
            errMessage = "";
            bool result = false;
            addedIds = new List<object>();
            recordsAffecteds = 0;
            try
            {
                result = AddRange(entities, null, null, out recordsAffecteds, out addedIds, commandTimeOut);
            }
            catch (RepositoryException genericRepositoryException)
            {
                errMessage = genericRepositoryException.OriginalException.Message;
                result = false;
            }
            catch (Exception exception)
            {
                errMessage = exception.Message;
                result = false;
            }

            return result;
        }
    }
}
