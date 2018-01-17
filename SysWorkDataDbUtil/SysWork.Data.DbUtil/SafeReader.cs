using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SysWork.Data.DbUtil
{
    public static class SafeReader
    {
        public static Int32? ReadInt32(SqlDataReader reader, string columnName)
        {
            return ReadInt32(reader, reader.GetOrdinal(columnName));
        }
        public static Int32? ReadInt32(SqlDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetInt32(position);
        }

        public static bool ReadBool(SqlDataReader reader, string columnName)
        {
            return ReadBool(reader, reader.GetOrdinal(columnName));
        }

        public static bool ReadBool(SqlDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return false;
            else
                return reader.GetBoolean(position);
        }

        public static string ReadString(SqlDataReader reader, string columnName)
        {
            return ReadString(reader, reader.GetOrdinal(columnName));
        }
        public static string ReadString(SqlDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetString(position);
        }
        public static DateTime? ReadDateTime(SqlDataReader reader, string columnName)
        {
            return ReadDateTime(reader, reader.GetOrdinal(columnName));
        }
        public static DateTime? ReadDateTime(SqlDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetDateTime(position);
        }
        public static long? ReadLong(SqlDataReader reader, string columnName)
        {
            return ReadLong(reader, reader.GetOrdinal(columnName));
        }
        public static long? ReadLong(SqlDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetInt32(position);
        }

    }
}
