using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace SysWork.Data.Common.ObjectResolver
{
    public static class DataObjectResolver
    {
        public static IDbCommand GetIDbCommand(EDataBaseEngine dataBaseEngine, string commandText, IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            IDbCommand dbCommand = GetIDbCommand(dataBaseEngine);

            dbCommand.Connection = dbConnection;
            dbCommand.CommandText = commandText;

            return dbCommand;
        }
        public static IDbCommand GetIDbCommand(EDataBaseEngine dataBaseEngine)
        {
            IDbCommand dbCommand = null;

            if (dataBaseEngine == EDataBaseEngine.SqlServer)
                dbCommand = new SqlCommand();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                dbCommand = new OleDbCommand();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                dbCommand = new SQLiteCommand();

            return dbCommand;
        }
        public static DbCommand GetDbCommand(EDataBaseEngine dataBaseEngine, string commandText, DbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            DbCommand dbCommand = GetDbCommand(dataBaseEngine);

            dbCommand.Connection = dbConnection;
            dbCommand.CommandText = commandText;

            return dbCommand;
        }
        public static DbCommand GetDbCommand(EDataBaseEngine dataBaseEngine)
        {
            DbCommand dbCommand = null;

            if (dataBaseEngine == EDataBaseEngine.SqlServer)
                dbCommand = new SqlCommand();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                dbCommand = new OleDbCommand();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                dbCommand = new SQLiteCommand();

            return dbCommand;
        }
        public static IDbConnection GetIDbConnection(EDataBaseEngine dataBaseEngine, string ConnectionString)
        {
            IDbConnection ret = null;

            if (dataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteConnection(ConnectionString);

            return ret;
        }
        public static IDbConnection GetIDbConnection(EDataBaseEngine dataBaseEngine)
        {
            IDbConnection ret = null;

            if (dataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlConnection();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbConnection();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteConnection();

            return ret;
        }

        public static DbConnection GetDbConnection(EDataBaseEngine dataBaseEngine,string ConnectionString)
        {
            DbConnection ret = null;

            if (dataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteConnection(ConnectionString);

            return ret;
        }


        public static DbConnection GetDbConnection(EDataBaseEngine dataBaseEngine)
        {
            DbConnection ret = null;

            if (dataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlConnection();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbConnection();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteConnection();

            return ret;
        }

        public static IDbDataParameter GetIDbDataParameter(EDataBaseEngine dataBaseEngine)
        {
            IDbDataParameter ret = null;

            if (dataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlParameter();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbParameter();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteParameter();

            return ret;
        }

        public static DbConnectionStringBuilder GetConnectionStringBuilder(EDataBaseEngine dataBaseEngine)
        {
            DbConnectionStringBuilder ret = null;
            
            if (dataBaseEngine == EDataBaseEngine.SqlServer)
                ret = new SqlConnectionStringBuilder();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                ret = new OleDbConnectionStringBuilder();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                ret = new SQLiteConnectionStringBuilder();
            return ret;
        }



    }
}
