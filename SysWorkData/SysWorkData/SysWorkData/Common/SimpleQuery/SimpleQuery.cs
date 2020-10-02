using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.SimpleQuery
{
    
    /// <summary>
    /// Static class to Read Data Quickly.
    /// </summary>
    public static class SimpleQuery
    {
        /// <summary>
        /// Executes the specified commandText in a DbConnection.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns>An dynamic IEnumerable.</returns>
        public static IEnumerable<dynamic> Execute(DbConnection dbConnection, string commandText)
        {
            return Execute(StaticDbObjectProvider.GetDatabaseEngineFromDbConnection(dbConnection), dbConnection, commandText,false);
        }

        /// <summary>
        /// Executes the specified commandText in a DbConnection.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="closeConnection">if set to <c>true</c> [close connection].</param>
        /// <returns>An dynamic IEnumerable</returns>
        public static IEnumerable<dynamic> Execute(EDatabaseEngine databaseEngine, string connectionString, string commandText,bool closeConnection = true)
        {
            DbConnection dbConnection = StaticDbObjectProvider.GetDbConnection(databaseEngine,connectionString);
            return Execute(databaseEngine, dbConnection, commandText,closeConnection);
        }

        /// <summary>
        /// Executes the specified commandText in a DbConnection(MSSqlServer).
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="closeConnection">if set to <c>true</c> [close connection].</param>
        /// <returns>An dynamic IEnumerable</returns>
        public static IEnumerable<dynamic> Execute(string connectionString, string commandText,bool closeConnection = true)
        {
            DbConnection dbConnection = StaticDbObjectProvider.GetDbConnection(EDatabaseEngine.MSSqlServer);
            dbConnection.ConnectionString = connectionString;

            return Execute(EDatabaseEngine.MSSqlServer, dbConnection, commandText, closeConnection);
        }

        /// <summary>
        /// Executes the specified commandText in a DbConnection.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="closeConnection">if set to <c>true</c> [close connection].</param>
        /// <returns></returns>
        private static IEnumerable<dynamic> Execute(EDatabaseEngine databaseEngine, DbConnection dbConnection, string commandText,bool closeConnection = true)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            using (var dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = commandText;
                using (DbDataReader dataReader = dbCommand.ExecuteReader(closeConnection ? CommandBehavior.CloseConnection: CommandBehavior.Default))
                {
                    foreach (IDataRecord record in dataReader)
                    {
                        yield return new DataRecordDynamicWrapper(record);
                    }
                }
            }
        }
        /// <summary>
        /// Gets an element at the specify index.
        /// </summary>
        /// <typeparam name="dynamic">The type of the Dynamic.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static dynamic ElementAt<dynamic>(IEnumerable<dynamic> items, int index)
        {
            using (IEnumerator<dynamic> iter = items.GetEnumerator())
            {
                for (int i = 0; i <= index; i++, iter.MoveNext()) ;
                return iter.Current;
            }
        }
    }
}
