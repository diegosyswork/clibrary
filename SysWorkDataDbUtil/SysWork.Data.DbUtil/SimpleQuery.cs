using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SysWork.Data.DbUtil
{
    /// <summary>
    /// Devuelve un IEnumerable con el contenido de una consulta.
    /// </summary>
    public static class SimpleQuery
    {
        /// <summary>
        /// Devuelve un IEnumerable en base a un comando y una conexion.
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Execute(SqlConnection sqlConnection, string commandText)
        {
            using (var connection = sqlConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                   
                using (var command = new SqlCommand(commandText, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        foreach (IDataRecord record in reader)
                        {
                            yield return new DataRecordDynamicWrapper(record);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve un IEnumerable en base a una cadena de conexion.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Execute(string connectionString, string commandText)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            return Execute(sqlConnection, commandText);
        }

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
