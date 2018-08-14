using System;
using System.Data;

namespace SysWork.Data.Common.Utilities
{
    public static class SafeReader
    {
        public static Int32? ReadInt32(IDataReader reader, string columnName)
        {
            return ReadInt32(reader, reader.GetOrdinal(columnName));
        }
        public static Int32? ReadInt32(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetInt32(position);
        }

        public static bool ReadBool(IDataReader reader, string columnName)
        {
            return ReadBool(reader, reader.GetOrdinal(columnName));
        }

        public static bool ReadBool(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return false;
            else
                return reader.GetBoolean(position);
        }

        public static string ReadString(IDataReader reader, string columnName)
        {
            return ReadString(reader, reader.GetOrdinal(columnName));
        }
        public static string ReadString(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetString(position);
        }
        public static DateTime? ReadDateTime(IDataReader reader, string columnName)
        {
            return ReadDateTime(reader, reader.GetOrdinal(columnName));
        }
        public static DateTime? ReadDateTime(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetDateTime(position);
        }
        public static long? ReadLong(IDataReader reader, string columnName)
        {
            return ReadLong(reader, reader.GetOrdinal(columnName));
        }
        public static long? ReadLong(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetInt32(position);
        }

    }
}
