using System;
using System.Data;

namespace SysWork.Data.NetCore.Common.Utilities
{
    /// <summary>
    /// Helper to work with IDataReaders
    /// </summary>
    public static class SafeReader
    {

        /// <summary>
        /// Reads an int32 column (by the column name). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static Int32? ReadInt32(IDataReader reader, string columnName)
        {
            return ReadInt32(reader, reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Reads an int32 column (by the position). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static Int32? ReadInt32(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetInt32(position);
        }

        /// <summary>
        /// Reads an bit column (by the column name). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns></returns>
        public static bool ReadBool(IDataReader reader, string columnName)
        {
            return ReadBool(reader, reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Reads an bit column (by the column position). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static bool ReadBool(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return false;
            else
                return reader.GetBoolean(position);
        }
        
        /// <summary>
        /// Reads an string column (by the column name). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns></returns>
        public static string ReadString(IDataReader reader, string columnName)
        {
            return ReadString(reader, reader.GetOrdinal(columnName));
        }


        /// <summary>
        /// Reads an string column (by the column position). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static string ReadString(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetString(position);
        }

        /// <summary>
        /// Reads an DateTime column (by the column name). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns></returns>
        public static DateTime? ReadDateTime(IDataReader reader, string columnName)
        {
            return ReadDateTime(reader, reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Reads an DateTime column (by the column position). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static DateTime? ReadDateTime(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetDateTime(position);
        }

        /// <summary>
        /// Reads an Long column (by the column name). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns></returns>
        public static long? ReadLong(IDataReader reader, string columnName)
        {
            return ReadLong(reader, reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Reads an Long column (by the column position). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static long? ReadLong(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetInt32(position);
        }

        /// <summary>
        /// Reads an Decimal column (by the position). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>
        /// Decimal value or null
        /// </returns>
        public static Decimal? ReadDecimal(IDataReader reader, string columnName)
        {
            return ReadDecimal(reader, reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Reads an Decimal column (by the position). if Database value is DbNull return null else the original value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        /// Decimal value or null
        /// </returns>
        public static Decimal? ReadDecimal(IDataReader reader, int position)
        {
            if (reader.IsDBNull(position))
                return null;
            else
                return reader.GetDecimal(position);
        }
    }

}
