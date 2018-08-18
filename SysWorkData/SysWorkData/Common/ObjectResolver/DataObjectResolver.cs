using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace SysWork.Data.Common.ObjectResolver
{
    public static class DataObjectResolver
    {
        /// <summary>
        /// Retorna un *IDbCommand* en base al dataBaseEngine solicitado
        /// con un IDbConnection abierto y un commandText Cargado
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <param name="commandText"></param>
        /// <param name="iDbConnection"></param>
        /// <returns></returns>
        public static IDbCommand GetIDbCommand(EDataBaseEngine dataBaseEngine, string commandText, IDbConnection iDbConnection)
        {
            if (iDbConnection.State != ConnectionState.Open)
                iDbConnection.Open();

            IDbCommand iDbCommand = GetIDbCommand(dataBaseEngine);

            iDbCommand.Connection = iDbConnection;
            iDbCommand.CommandText = commandText;

            return iDbCommand;
        }
        /// <summary>
        /// Devuelve un *IDbCommand* en base al dataBaseEngine seleccionado
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <returns></returns>
        public static IDbCommand GetIDbCommand(EDataBaseEngine dataBaseEngine)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlCommand();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbCommand();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteCommand();
            else
                throw new ArgumentException("No hay un objeto para el dataBaseEngine Solicitado");

        }
        /// <summary>
        /// Retorna un *DbCommand* en base al dataBaseEngine solicitado
        /// con un DbConnection abierto y un commandText Cargado
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <param name="commandText"></param>
        /// <param name="iDbConnection"></param>
        /// <returns></returns>
        public static DbCommand GetDbCommand(EDataBaseEngine dataBaseEngine, string commandText, DbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            DbCommand dbCommand = GetDbCommand(dataBaseEngine);
            dbCommand.Connection = dbConnection;

            dbCommand.CommandText = commandText;

            return dbCommand;
        }
        /// <summary>
        /// Devuelve un *DbCommand* en base al dataBaseEngine seleccionado
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <returns></returns>
        public static DbCommand GetDbCommand(EDataBaseEngine dataBaseEngine)
        {

            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlCommand();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbCommand();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteCommand();
            else
                throw new ArgumentException("No hay un objeto para el dataBaseEngine Solicitado");

        }
        /// <summary>
        /// Devuelve un *IDbConnection* instanciado con el ConnectionString
        /// cargado, y *CERRADO*
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static IDbConnection GetIDbConnection(EDataBaseEngine dataBaseEngine, string ConnectionString)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteConnection(ConnectionString);
            else
                throw new ArgumentException("No hay un objeto para el dataBaseEngine Solicitado");
        }
        /// <summary>
        /// Devuelve un *IDbConnection* instanciado SIN el ConnectionString
        /// cargado, y *CERRADO*
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static IDbConnection GetIDbConnection(EDataBaseEngine dataBaseEngine)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnection();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbConnection();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return  new SQLiteConnection();
            else
                throw new ArgumentException("No hay un objeto para el dataBaseEngine Solicitado");
        }
        /// <summary>
        /// Devuelve un *DbConnection* instanciado, con el ConnectionString cargado
        /// y Cerrado
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static DbConnection GetDbConnection(EDataBaseEngine dataBaseEngine,string ConnectionString)
        {

            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return  new OleDbConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return  new SQLiteConnection(ConnectionString);
            else
                throw new ArgumentException("No hay un objeto para el dataBaseEngine Solicitado");
        }
        /// <summary>
        /// Devuelve un *DbConnection* instanciado, SIN el ConnectionString cargado
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <returns></returns>
        public static DbConnection GetDbConnection(EDataBaseEngine dataBaseEngine)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnection();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return  new OleDbConnection();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteConnection();
            else
                throw new ArgumentException("No hay un objeto para el dataBaseEngine Solicitado");
        }
        /// <summary>
        /// Devuelve un *IDbDataParameter* instanciado en base al databaseEngine
        /// solicitado
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <returns></returns>
        public static IDbDataParameter GetIDbDataParameter(EDataBaseEngine dataBaseEngine)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlParameter();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbParameter();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteParameter();
            else
                throw new ArgumentException("No hay un objeto para el dataBaseEngine Solicitado");
        }
        /// <summary>
        /// Devuelve un *DbConnectionStringBuilder* en base al databaseEngine solicitado
        /// </summary>
        /// <param name="dataBaseEngine"></param>
        /// <returns></returns>
        public static DbConnectionStringBuilder GetConnectionStringBuilder(EDataBaseEngine dataBaseEngine)
        {
            if (dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return new SqlConnectionStringBuilder();
            else if (dataBaseEngine == EDataBaseEngine.OleDb)
                return new OleDbConnectionStringBuilder();
            else if (dataBaseEngine == EDataBaseEngine.SqLite)
                return new SQLiteConnectionStringBuilder();
            else
                throw new ArgumentException("No hay un objeto para el dataBaseEngine Solicitado");
        }
        /// <summary>
        /// Dado un DbConnection instanciado, devuelve el EDataBaseEngine correspondiente
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public static EDataBaseEngine GetDataBaseEngineFromDbConnection(DbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
                return EDataBaseEngine.MSSqlServer;
            else if (dbConnection is OleDbConnection)
                return EDataBaseEngine.OleDb;
            else if (dbConnection is SQLiteConnection)
                return EDataBaseEngine.SqLite;
            else
                throw new ArgumentException("El Objecto enviado no esta dentro del rango esperado");
        }
        /// <summary>
        /// Dado un DbCommand instanciado, devuelve el EDataBaseEngine correspondiente
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        public static EDataBaseEngine GetDataBaseEngineFromDbCommand(DbCommand dbCommand)
        {
            if (dbCommand is SqlCommand)
                return EDataBaseEngine.MSSqlServer;
            else if (dbCommand is OleDbCommand)
                return EDataBaseEngine.OleDb;
            else if (dbCommand is SQLiteCommand)
                return EDataBaseEngine.SqLite;
            else
                throw new ArgumentException("El Objecto enviado no esta dentro del rango esperado");
        }


    }
}
