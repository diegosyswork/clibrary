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
    /// <summary>
    /// Adds a record.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract partial class BaseRepository<TEntity> : IAdd<TEntity>
    {
        /// <summary>
        /// Adds a record.
        /// </summary>
        /// <remarks>
        /// Adds a record in the database.
        /// If successful, it returns the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column, it returns 0.
        /// In case of Exception throws a new GenericRepositoryException.
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated entity.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        public long Add(TEntity entity)
        {
            return Add(entity, null, null, null);
        }

        public long Add(TEntity entity, int commandTimeOut)
        {
            return Add(entity, null, null, commandTimeOut);
        }

        public long Add(TEntity entity, IDbConnection dbConnection)
        {
            return Add(entity, dbConnection, null, null);
        }

        public long Add(TEntity entity, IDbConnection dbConnection, int commandTimeOut)
        {
            return Add(entity, dbConnection, null, commandTimeOut);
        }

        public long Add(TEntity entity, IDbTransaction dbTransaction)
        {
            return Add(entity, null, dbTransaction, null);
        }

        public long Add(TEntity entity, IDbTransaction dbTransaction, int commandTimeOut)
        {
            return Add(entity, null, dbTransaction, commandTimeOut);
        }

        #region DOCUMENTATION public long Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        /// <summary>
        /// Adds a record using an DbConnection and DbTransaction and custom dbCommand timeout.
        /// </summary>
        /// <remarks>
        /// Adds a record using an DbConnection and DbTransaction and custom dbCommand timeout.
        /// 
        /// If dbConnection is null, a new one will be created and closed on completion.
        /// If dbConnection is null and dbTransaction is not null, the connection from the dbTransaction will be used. The connection will not close at the end.
        /// If dbTransaction is null, no transaction will be used in the dbCommand.
        /// 
        /// </remarks>
        /// <param name="entity">The entity.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <![CDATA[
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
        /// class Sample
        /// {
        ///     void main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository<Person>(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address");
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
        ///             long id = PersonRepository.Add(p, dbConnection, dbTransaction);
        ///             dbTransaction.Commit();
        ///             Console.Writeline($"The generated id is{id}");
        ///         }
        ///         catch (GenericRepositoryException ex)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.Writeline($"The following error has occurred{ex.Message}");
        ///         }
        ///         finally
        ///         {
        ///             dbConnection.Close();
        ///             dbConnection.Dispose();
        ///         }
        ///     }
        /// }
        /// ]]>
        /// </example>
        #endregion
        public long Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return Add(entity, dbConnection, dbTransaction, null);
        }

        #region DOCUMENTATION public long Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        /// <summary>
        /// Adds a record using an DbConnection and DbTransaction and custom dbCommand timeout.
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
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="commandTimeOut">The command timeout.</param>
        /// <returns>
        /// If successful, it returns the identity(Id) of the generated record.
        /// In case it does not have an Identity type column, it returns 0.
        /// </returns>
        /// <exception cref="RepositoryException"></exception>
        /// <example>
        /// <![CDATA[
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
        /// class Sample
        /// {
        ///     void main()
        ///     {
        ///         var connectionString = "Data Source=.;Initial Catalog=DB;User ID=MyUser;Password=MyPass";
        ///         var databaseEngine = EDatabaseEngine.MSSqlServer;
        ///      
        ///         var personRepository = new PersonRepository<Person>(connectionString, databaseEngine);
        ///
        ///         var p = new Person();
        ///         p.FirstName = "Diego";
        ///         p.LastName = "Martinez";
        ///         p.Passport = "AR00127296";
        ///         p.LongNameField = "Field With Long Name";
        ///         p.Address = "Address");
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
        ///             long id = PersonRepository.Add(p, dbConnection, dbTransaction, commandTimeOut );
        ///             dbTransaction.Commit();
        ///             Console.Writeline($"The generated id is{id}");
        ///         }
        ///         catch (GenericRepositoryException ex)
        ///         {
        ///             dbTransaction.Rollback();
        ///             Console.Writeline($"The following error has occurred{ex.Message}");
        ///         }
        ///         finally
        ///         {
        ///             dbConnection.Close();
        ///             dbConnection.Dispose();
        ///         }
        ///     }
        /// }
        /// ]]>
        /// </example>
        #endregion
        public long Add(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction, int? commandTimeOut)
        {
            long identity = 0;
            bool hasIdentity = false;

            bool closeConnection = ((dbConnection == null) && (dbTransaction == null));

            if (dbConnection == null && dbTransaction != null)
                dbConnection = dbTransaction.Connection;

            IDbConnection dbConnectionInUse = dbConnection ?? BaseIDbConnection();
            IDbCommand dbCommand = dbConnectionInUse.CreateCommand();

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
                    if (dbConnectionInUse.State != ConnectionState.Open)
                        dbConnectionInUse.Open();

                    dbCommand.CommandText = insertQuery.ToString();
                    dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

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
                    if ((dbConnectionInUse != null) && (dbConnectionInUse.State == ConnectionState.Open) && (closeConnection))
                    {
                        dbConnectionInUse.Close();
                        dbConnectionInUse.Dispose();
                    }
                }
            }
            return identity;
        }

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

    }
}
