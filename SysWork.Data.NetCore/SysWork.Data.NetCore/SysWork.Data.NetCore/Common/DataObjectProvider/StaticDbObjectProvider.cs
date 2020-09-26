using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using SysWork.Data.NetCore.Common.ValueObjects;

namespace SysWork.Data.NetCore.Common.DataObjectProvider
{
    /// <summary>
    /// Static class that provides Database Objects depending on the database engine provided.
    /// </summary>
    public static class StaticDbObjectProvider
    {
        /// <summary>
        /// Gets an IDbConnection depending of the database engine,with the connectionString provided.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns>
        /// An instantiated IDbConnection depending of the database engine, with the connectionString provided. The object is closed.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static IDbConnection GetIDbConnection(EDataBaseEngine dataBaseEngine, string ConnectionString)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlConnection(ConnectionString);
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets an IDbConnection depending of the database engine.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <returns>
        /// An instantiated IDbConnection depending of the database engine. The object is closed.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static IDbConnection GetIDbConnection(EDataBaseEngine dataBaseEngine)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnection();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbConnection();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteConnection();
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlConnection();
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets an DbConnection depending of the database engine,with the connectionString provided.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns>
        /// An instantiated DbConnection depending of the database engine, with the connectionString provided. The object is closed.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static DbConnection GetDbConnection(EDataBaseEngine dataBaseEngine,string ConnectionString)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return  new OleDbConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlConnection(ConnectionString);
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets an DbConnection depending of the database engine.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <returns>
        /// An instantiated DbConnection depending of the database engine. The object is closed.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static DbConnection GetDbConnection(EDataBaseEngine dataBaseEngine)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnection();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return  new OleDbConnection();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteConnection();
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlConnection();
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets the EdatabaseEngine from DbConnection Object.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <returns>
        /// An EDatabaseEngine value depending on a DbConnection object.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The dbConnection provided is not supported by this method.</exception>
        public static EDataBaseEngine GetDataBaseEngineFromDbConnection(DbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
                return EDataBaseEngine.MSSqlServer;
            else if (dbConnection is OleDbConnection)
                return EDataBaseEngine.OleDb;
            else if (dbConnection is SQLiteConnection)
                return EDataBaseEngine.SqLite;
            else if (dbConnection is MySqlConnection)
                return EDataBaseEngine.MySql;
            else
                throw new ArgumentOutOfRangeException("The dbConnection provided is not supported by this method.");
        }

        /// <summary>
        /// Gets the EdatabaseEngine from DbCommand Object.
        /// </summary>
        /// <param name="dbCommand">The database command.</param>
        /// <returns>
        /// An EDatabaseEngine value depending on a DbCommand object.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static EDataBaseEngine GetDataBaseEngineFromDbCommand(DbCommand dbCommand)
        {
            if (dbCommand is SqlCommand)
                return EDataBaseEngine.MSSqlServer;
            else if (dbCommand is OleDbCommand)
                return EDataBaseEngine.OleDb;
            else if (dbCommand is SQLiteCommand)
                return EDataBaseEngine.SqLite;
            else if (dbCommand is MySqlCommand)
                return EDataBaseEngine.MySql;
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets an DbConnectionStringBuilder depending of the database engine.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <returns>
        /// An instantiated DbConnectionStrigBuilder depending of the database engine.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static DbConnectionStringBuilder GetDbConnectionStringBuilder(EDataBaseEngine dataBaseEngine)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnectionStringBuilder();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbConnectionStringBuilder();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteConnectionStringBuilder();
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlConnectionStringBuilder();
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets and DbDataAdapter data adapter.
        /// </summary>
        /// <param name="dataBaseEngine">The data base engine.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static DbDataAdapter GetDbDataAdapter(EDataBaseEngine dataBaseEngine)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlDataAdapter();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbDataAdapter();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteDataAdapter();
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlDataAdapter();
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

    }

}
