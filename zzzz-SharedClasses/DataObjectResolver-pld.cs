using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace SysWork.Data.Common.ObjectResolver
{
    public static class DataObjectResolver
    {
        public static IDbCommand GetIDbCommand(EDataBaseEngine resolverDataBaseEngine, string commandText, IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            IDbCommand dbCommand = GetIDbCommand(resolverDataBaseEngine);

            dbCommand.Connection = dbConnection;
            dbCommand.CommandText = commandText;

            return dbCommand;
        }
        public static IDbCommand GetIDbCommand(EDataBaseEngine resolverDataBaseEngine)
        {
            IDbCommand dbCommand = null;

            if (resolverDataBaseEngine == EDataBaseEngine.SqlServer)
                dbCommand = new SqlCommand();
            else if (resolverDataBaseEngine == EDataBaseEngine.OleDb)
                dbCommand = new OleDbCommand();
            else if (resolverDataBaseEngine == EDataBaseEngine.SqLite)
                dbCommand = new SQLiteCommand();

            return dbCommand;
        }
        public static DbCommand GetDbCommand(EDataBaseEngine resolverDataBaseEngine, string commandText, DbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            DbCommand dbCommand = GetDbCommand(resolverDataBaseEngine);

            dbCommand.Connection = dbConnection;
            dbCommand.CommandText = commandText;

            return dbCommand;
        }
        public static DbCommand GetDbCommand(EDataBaseEngine resolverDataBaseEngine)
        {
            DbCommand dbCommand = null;

            if (resolverDataBaseEngine == EDataBaseEngine.SqlServer)
                dbCommand = new SqlCommand();
            else if (resolverDataBaseEngine == EDataBaseEngine.OleDb)
                dbCommand = new OleDbCommand();
            else if (resolverDataBaseEngine == EDataBaseEngine.SqLite)
                dbCommand = new SQLiteCommand();

            return dbCommand;
        }
        public static IDbConnection GetIDbConnection(EDataBaseEngine resolverDataBaseEngine, string ConnectionString)
        {
            IDbConnection ret = null;

            if (resolverDataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlConnection(ConnectionString);
            else if (resolverDataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbConnection(ConnectionString);
            else if (resolverDataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteConnection(ConnectionString);

            return ret;
        }
        public static IDbConnection GetIDbConnection(EDataBaseEngine resolverDataBaseEngine)
        {
            IDbConnection ret = null;

            if (resolverDataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlConnection();
            else if (resolverDataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbConnection();
            else if (resolverDataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteConnection();

            return ret;
        }

        public static DbConnection GetDbConnection(EDataBaseEngine resolverDataBaseEngine,string ConnectionString)
        {
            DbConnection ret = null;

            if (resolverDataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlConnection(ConnectionString);
            else if (resolverDataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbConnection(ConnectionString);
            else if (resolverDataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteConnection(ConnectionString);

            return ret;
        }


        public static DbConnection GetDbConnection(EDataBaseEngine resolverDataBaseEngine)
        {
            DbConnection ret = null;

            if (resolverDataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlConnection();
            else if (resolverDataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbConnection();
            else if (resolverDataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteConnection();

            return ret;
        }

        public static IDbDataParameter GetIDbDataParameter(EDataBaseEngine resolverDataBaseEngine)
        {
            IDbDataParameter ret = null;

            if (resolverDataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlParameter();
            else if (resolverDataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbParameter();
            else if (resolverDataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteParameter();

            return ret;
        }
    }
}
