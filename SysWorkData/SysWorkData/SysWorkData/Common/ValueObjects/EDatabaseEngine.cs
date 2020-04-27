namespace SysWork.Data.Common.ValueObjects
{
    /// <summary>
    /// Supported Database Engines
    /// </summary>
    public enum EDataBaseEngine
    {
        /// <summary>
        /// Microsoft SQL Server
        /// </summary>
        MSSqlServer = 0,
        /// <summary>
        /// SQLite V3
        /// </summary>
        SqLite = 1,
        /// <summary>
        /// OleDb
        /// </summary>
        OleDb = 2,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 3
    }
}