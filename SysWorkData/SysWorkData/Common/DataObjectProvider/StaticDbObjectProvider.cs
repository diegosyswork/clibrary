using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace SysWork.Data.Common.DataObjectProvider
{
    
    public static class StaticDbObjectProvider
    {
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
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlConnection(ConnectionString);
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
                return new SQLiteConnection();
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlConnection();
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
                return new SQLiteConnection(ConnectionString);
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlConnection(ConnectionString);
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
            else if (dataBaseEngine == EDataBaseEngine.MySql)
                return new MySqlConnection();
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
            else if (dbConnection is MySqlConnection)
                return EDataBaseEngine.MySql;
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
            else if (dbCommand is MySqlCommand)
                return EDataBaseEngine.MySql;
            else
                throw new ArgumentException("El Objecto enviado no esta dentro del rango esperado");
        }
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
                throw new ArgumentException("No hay un objeto para el dataBaseEngine Solicitado");

        }
    }

}
