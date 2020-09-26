namespace SysWork.Data.NetCore.Common.LambdaSqlBuilder.ValueObjects
{
    /// <summary>
    /// An enumeration of the available SQL adapters. Can be used to set the backing database for db specific SQL syntax
    /// </summary>
    public enum SqlAdapter
    {
        /// <summary>
        /// The MSSQL Server2008 Adapter
        /// </summary>
        SqlServer2008,
        /// <summary>
        /// The MSSQL Server2012
        /// </summary>
        SqlServer2012,
        /// <summary>
        /// MySql Adapter
        /// </summary>
        MySql,
        /// <summary>
        /// The SQLite adapter
        /// </summary>
        SQLite
    }
}
