using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using SysWork.Data.Common.ValueObjects;

namespace SysWork.Data.Common.DataObjectProvider
{
    /// <summary>
    /// Static class that provides Database Objects depending on the database engine provided.
    /// </summary>
    public static class StaticDbObjectProvider
    {
        /// <summary>
        /// Gets an IDbConnection depending of the database engine,with the connectionString provided.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns>
        /// An instantiated IDbConnection depending of the database engine, with the connectionString provided. The object is closed.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static IDbConnection GetIDbConnection(EDatabaseEngine databaseEngine, string ConnectionString)
        {
            if (databaseEngine == EDatabaseEngine.MSSqlServer)
                return new SqlConnection(ConnectionString);
            else if (databaseEngine == EDatabaseEngine.OleDb)
                return new OleDbConnection(ConnectionString);
            else if (databaseEngine == EDatabaseEngine.SqLite)
                return new SQLiteConnection(ConnectionString);
            else if (databaseEngine == EDatabaseEngine.MySql)
                return new MySqlConnection(ConnectionString);
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets an IDbConnection depending of the database engine.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <returns>
        /// An instantiated IDbConnection depending of the database engine. The object is closed.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static IDbConnection GetIDbConnection(EDatabaseEngine databaseEngine)
        {
            if (databaseEngine == EDatabaseEngine.MSSqlServer)
                return new SqlConnection();
            else if (databaseEngine == EDatabaseEngine.OleDb)
                return new OleDbConnection();
            else if (databaseEngine == EDatabaseEngine.SqLite)
                return new SQLiteConnection();
            else if (databaseEngine == EDatabaseEngine.MySql)
                return new MySqlConnection();
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets an DbConnection depending of the database engine,with the connectionString provided.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns>
        /// An instantiated DbConnection depending of the database engine, with the connectionString provided. The object is closed.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static DbConnection GetDbConnection(EDatabaseEngine databaseEngine,string ConnectionString)
        {
            if (databaseEngine == EDatabaseEngine.MSSqlServer)
                return new SqlConnection(ConnectionString);
            else if (databaseEngine == EDatabaseEngine.OleDb)
                return  new OleDbConnection(ConnectionString);
            else if (databaseEngine == EDatabaseEngine.SqLite)
                return new SQLiteConnection(ConnectionString);
            else if (databaseEngine == EDatabaseEngine.MySql)
                return new MySqlConnection(ConnectionString);
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets an DbConnection depending of the database engine.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <returns>
        /// An instantiated DbConnection depending of the database engine. The object is closed.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static DbConnection GetDbConnection(EDatabaseEngine databaseEngine)
        {
            if (databaseEngine == EDatabaseEngine.MSSqlServer)
                return new SqlConnection();
            else if (databaseEngine == EDatabaseEngine.OleDb)
                return  new OleDbConnection();
            else if (databaseEngine == EDatabaseEngine.SqLite)
                return new SQLiteConnection();
            else if (databaseEngine == EDatabaseEngine.MySql)
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
        public static EDatabaseEngine GetDatabaseEngineFromDbConnection(DbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
                return EDatabaseEngine.MSSqlServer;
            else if (dbConnection is OleDbConnection)
                return EDatabaseEngine.OleDb;
            else if (dbConnection is SQLiteConnection)
                return EDatabaseEngine.SqLite;
            else if (dbConnection is MySqlConnection)
                return EDatabaseEngine.MySql;
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
        public static EDatabaseEngine GetDatabaseEngineFromDbCommand(DbCommand dbCommand)
        {
            if (dbCommand is SqlCommand)
                return EDatabaseEngine.MSSqlServer;
            else if (dbCommand is OleDbCommand)
                return EDatabaseEngine.OleDb;
            else if (dbCommand is SQLiteCommand)
                return EDatabaseEngine.SqLite;
            else if (dbCommand is MySqlCommand)
                return EDatabaseEngine.MySql;
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets an DbConnectionStringBuilder depending of the database engine.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <returns>
        /// An instantiated DbConnectionStrigBuilder depending of the database engine.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static DbConnectionStringBuilder GetDbConnectionStringBuilder(EDatabaseEngine databaseEngine)
        {
            if (databaseEngine == EDatabaseEngine.MSSqlServer)
                return new SqlConnectionStringBuilder();
            else if (databaseEngine == EDatabaseEngine.OleDb)
                return new OleDbConnectionStringBuilder();
            else if (databaseEngine == EDatabaseEngine.SqLite)
                return new SQLiteConnectionStringBuilder();
            else if (databaseEngine == EDatabaseEngine.MySql)
                return new MySqlConnectionStringBuilder();
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        /// <summary>
        /// Gets and DbDataAdapter data adapter.
        /// </summary>
        /// <param name="databaseEngine">The data base engine.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The databaseEngine value is not supported by this method.</exception>
        public static DbDataAdapter GetDbDataAdapter(EDatabaseEngine databaseEngine)
        {
            if (databaseEngine == EDatabaseEngine.MSSqlServer)
                return new SqlDataAdapter();
            else if (databaseEngine == EDatabaseEngine.OleDb)
                return new OleDbDataAdapter();
            else if (databaseEngine == EDatabaseEngine.SqLite)
                return new SQLiteDataAdapter();
            else if (databaseEngine == EDatabaseEngine.MySql)
                return new MySqlDataAdapter();
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }

        public static IDbDataParameter GetDbDataParameter(EDatabaseEngine databaseEngine)
        {
            if (databaseEngine == EDatabaseEngine.MSSqlServer)
                return new SqlParameter();
            else if (databaseEngine == EDatabaseEngine.OleDb)
                return new OleDbParameter();
            else if (databaseEngine == EDatabaseEngine.SqLite)
                return new SQLiteParameter();
            else if (databaseEngine == EDatabaseEngine.MySql)
                return new MySqlParameter();
            else
                throw new ArgumentOutOfRangeException("The databaseEngine value is not supported by this method.");
        }


    }

}
