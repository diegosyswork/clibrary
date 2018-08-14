using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SysWork.Data.Common.ObjectResolver;

namespace SysWork.Data.Common.SimpleQuery
{
    public static class SimpleQuery
    {
        public static IEnumerable<dynamic> Execute(EDataBaseEngine dataBaseEngine,DbConnection dbConnection, string commandText)
        {
            using (var connection = dbConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var command = DataObjectResolver.GetDbCommand(dataBaseEngine,commandText, connection))
                {
                    using (DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        foreach (IDataRecord record in reader)
                        {
                           yield return new DataRecordDynamicWrapper(record);
                        }
                    }
                }
            }
        }
        public static IEnumerable<dynamic> Execute(DbConnection dbConnection, string commandText)
        {
            return Execute(EDataBaseEngine.SqlServer, dbConnection, commandText);
        }

        public static IEnumerable<dynamic> Execute(EDataBaseEngine dataBaseEngine, string connectionString, string commandText)
        {
            DbConnection dbConnection = DataObjectResolver.GetDbConnection(dataBaseEngine);
            dbConnection.ConnectionString = connectionString;

            return Execute(EDataBaseEngine.SqlServer, dbConnection, commandText);
        }
        public static IEnumerable<dynamic> Execute(string connectionString, string commandText)
        {
            DbConnection dbConnection = DataObjectResolver.GetDbConnection(EDataBaseEngine.SqlServer);
            dbConnection.ConnectionString = connectionString;

            return Execute(EDataBaseEngine.SqlServer, dbConnection, commandText);
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
