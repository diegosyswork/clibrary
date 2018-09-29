using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SysWork.Data.Common.ObjectResolver;

namespace SysWork.Data.Common.SimpleQuery
{
    public static class SimpleQuery
    {
        /// <summary>
        /// 
        /// Dado un DbConnection instanciado y un commandText
        /// ejecuta una consulta de devuelve un IEnumerable dinamico.
        /// En caso que la conexion este cerrada, intenta abrirla.
        /// *NO Cierra* la conexion al finalizar el metodo
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="commandText"></param>
        /// <returns>IEnumerable dinamico</returns>
        public static IEnumerable<dynamic> Execute(DbConnection dbConnection, string commandText)
        {
            return Execute(StaticDataObjectProvider.GetDataBaseEngineFromDbConnection(dbConnection), dbConnection, commandText,false);
        }

        public static IEnumerable<dynamic> Execute(EDataBaseEngine dataBaseEngine, string connectionString, string commandText,bool closeConnection = true)
        {
            DbConnection dbConnection = StaticDataObjectProvider.GetDbConnection(dataBaseEngine,connectionString);
            return Execute(dataBaseEngine, dbConnection, commandText,closeConnection);
        }
        /// <summary>
        /// Dado un connectionString y un commandText intentará crear una
        /// SqlConnection, ejecutara la consulta que devuelve un IEnumerable dinamico.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Execute(string connectionString, string commandText,bool closeConnection = true)
        {
            DbConnection dbConnection = StaticDataObjectProvider.GetDbConnection(EDataBaseEngine.MSSqlServer);
            dbConnection.ConnectionString = connectionString;

            return Execute(EDataBaseEngine.MSSqlServer, dbConnection, commandText, closeConnection);
        }
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
        /// Dado un IEnumerableDinamico y una posicion, devuelve el dinamico
        /// que corresponde con dicha posicion
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <param name="items"></param>
        /// <param name="index"></param>
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
