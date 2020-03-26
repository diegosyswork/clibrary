﻿using System;
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

namespace SysWork.Data.GenericRepository
{
    public abstract partial class BaseRepository<TEntity> : IAddRange<TEntity>
    {
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
        /// <exception cref="RepositoryException">
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
        ///         
        ///         var listEntities = new List<Person>();
        ///        
        ///         listEntities.Add(new Person { LastName = "Martinez", FirstName = "Diego", Passport = "AR00127296" });
        ///         listEntities.Add(new Person { LastName = "Perez", FirstName = "Juan", Passport = "AR00012224" });
        ///         listEntities.Add(new Person { LastName = "Fulanito", FirstName = "Cosme", Passport = "AR99999987" });
        ///          
        ///          try
        ///          {
        ///              PersonRepository.AddRange(listEntities);
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

        public bool AddRange(IList<TEntity> entities, out long recordsAffected)
        {
            return AddRange(entities, null, null, out recordsAffected, out IEnumerable<object> addedIds, null);
        }
        public bool AddRange(IList<TEntity> entities, out long recordsAffected, int commandTimeOut)
        {
            return AddRange(entities, null, null, out recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection)
        {
            return AddRange(entities, paramDbConnection, null, out long recordsAffected, out IEnumerable<object> addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, int commandTimeOut)
        {
            return AddRange(entities, paramDbConnection, null, out long recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, paramDbConnection, null, out long recordsAffected, out addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out IEnumerable<object> addedIds, int commandTimeOut)
        {
            return AddRange(entities, paramDbConnection, null, out long recordsAffected, out addedIds, commandTimeOut);
        }
        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out long recordsAffected)
        {
            return AddRange(entities, paramDbConnection, null, out recordsAffected, out IEnumerable<object> addedIds, null);
        }
        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, out long recordsAffected, int commandTimeOut)
        {
            return AddRange(entities, paramDbConnection, null, out recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction)
        {
            return AddRange(entities, null, paramDbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return AddRange(entities, null, paramDbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, null, paramDbTransaction, out long recordsAffected, out addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out IEnumerable<object> addedIds, int commandTimeOut)
        {
            return AddRange(entities, null, paramDbTransaction, out long recordsAffected, out addedIds, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            return AddRange(entities, null, paramDbTransaction, out recordsAffected, out IEnumerable<object> addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, IDbTransaction paramDbTransaction, out long recordsAffected, int commandTimeOut)
        {
            return AddRange(entities, null, paramDbTransaction, out recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }
        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction)
        {
            return AddRange(entities, paramDbConnection, paramDbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, int commandTimeOut)
        {
            return AddRange(entities, paramDbConnection, paramDbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, paramDbConnection, paramDbTransaction, out long recordsAffected, out addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out IEnumerable<object> addedIds, int commandTimeOut)
        {
            return AddRange(entities, paramDbConnection, paramDbTransaction, out long recordsAffected, out addedIds, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected)
        {
            return AddRange(entities, paramDbConnection, paramDbTransaction, out recordsAffected, out IEnumerable<object> addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, int commandTimeOut)
        {
            return AddRange(entities, paramDbConnection, paramDbTransaction, out recordsAffected, out IEnumerable<object> addedIds, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage)
        {
            return AddRange(entities, out errMessage, out IEnumerable<object> addedIds,out long recordsAffected, null);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage, int commandTimeOut)
        {
            return AddRange(entities, out errMessage, out IEnumerable<object> addedIds, out long recordsAffected, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, out errMessage, out  addedIds, out long recordsAffected, null);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage, out IEnumerable<object> addedIds, int commandTimeOut)
        {
            return AddRange(entities, out errMessage, out addedIds, out long recordsAffected, commandTimeOut);
        }

        public bool AddRange(IList<TEntity> entities, out string errMessage, out long recordsAffected)
        {
            return AddRange(entities, out errMessage, out IEnumerable<object> addedIds, out recordsAffected,null);
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


        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, out IEnumerable<object> addedIds)
        {
            return AddRange(entities, paramDbConnection, paramDbTransaction, out recordsAffected, out addedIds, null);
        }

        public bool AddRange(IList<TEntity> entities, IDbConnection paramDbConnection, IDbTransaction paramDbTransaction, out long recordsAffected, out IEnumerable<object> addedIds, int? commandTimeOut)
        {
            recordsAffected = 0;
            var idList = new List<object>();
            bool hasIdentity = false;
            addedIds = null;

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
                throw new RepositoryException(connectionException);
            }

            foreach (TEntity entity in entities)
            {
                string parameterList = "";
                dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandTimeout = commandTimeOut ?? _defaultCommandTimeout;

                string insertRangeQuery = "";
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
                    insertRangeQuery = (string.Format("INSERT INTO {0} ( {1} ) VALUES ( {2} ) {3};", _syntaxProvider.GetSecureTableName(TableName), ColumnsForInsert, parameterList, _syntaxProvider.GetSubQueryGetIdentity()));
                }

                try
                {
                    dbCommand.CommandText = insertRangeQuery;
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
                    if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
                    {
                        if (dbConnection.State == ConnectionState.Open)
                            dbConnection.Close();

                        dbConnection.Dispose();
                    }

                    throw new RepositoryException(commandException, dbCommand);
                }
            }

            addedIds = idList;

            if ((dbConnection != null) && (dbConnection.State == ConnectionState.Open) && (closeConnection))
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }

            return true;
        }

    }
}
