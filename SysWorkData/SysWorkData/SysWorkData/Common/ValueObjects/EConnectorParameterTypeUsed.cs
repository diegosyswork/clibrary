namespace SysWork.Data.Common.ValueObjects
{
    /// <summary>
    /// Type of parameters used in the Db Connectors
    /// </summary>
    public enum EConnectorParameterTypeUsed
    {
        /// <summary>
        /// Use manual parameters.
        /// </summary>
        ManualParameters = 0,
        /// <summary>
        /// Use an connectionString.
        /// </summary>
        ConnectionString = 1
    }
}