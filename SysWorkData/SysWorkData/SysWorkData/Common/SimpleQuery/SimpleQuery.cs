using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SysWork.Data.Common.DataObjectProvider;

namespace SysWork.Data.Common.SimpleQuery
{
    /// <summary>
    /// Static class to Read Data Quickly.
    /// </summary>
    public static class SimpleQuery
    {
        /// <summary>
        /// Executes the specified commandText in a DbConnection (MSSqlServer).
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns>An dynamic IEnumerable.</returns>
        public static IEnumerable<dynamic> Execute(DbConnection dbConnection, string commandText)
        {
            return Execute(StaticDbObjectProvider.GetDataBaseEngineFromDbConnection(dbConnection), dbConnection, commandText,false);
        }

        /// <summary>
        /// Executes the specified commandText in a DbConnection.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="closeConnection">if set to <c>true</c> [close connection].</param>
        /// <returns>An dynamic IEnumerable</returns>
        public static IEnumerable<dynamic> Execute(EDataBaseEngine dataBaseEngine, string connectionString, string commandText,bool closeConnection = true)
        {
            DbConnection dbConnection = StaticDbObjectProvider.GetDbConnection(dataBaseEngine,connectionString);
            return Execute(dataBaseEngine, dbConnection, commandText,closeConnection);
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
            DbConnection dbConnection = StaticDbObjectProvider.GetDbConnection(EDataBaseEngine.MSSqlServer);
            dbConnection.ConnectionString = connectionString;

            return Execute(EDataBaseEngine.MSSqlServer, dbConnection, commandText, closeConnection);
        }

        /// <summary>
        /// Executes the specified commandText in a DbConnection.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="closeConnection">if set to <c>true</c> [close connection].</param>
        /// <returns></returns>
        private static IEnumerable<dynamic> Execute(EDataBaseEngine dataBaseEngine, DbConnection dbConnection, string commandText,bool closeConnection = true)
        {
            using (var connection = dbConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var dbCommand = connection.CreateCommand())
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
        }

        /// <summary>
        /// gets an elements at the position.
        /// </summary>
        /// <typeparam name="dynamic">The type of the ynamic.</typeparam>
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
