namespace SysWork.Data.Common.ValueObjects
{
    public static class DefaultValues
    {
        public static EDatabaseEngine DefaultDatabaseEngine { get; set; } = EDatabaseEngine.MSSqlServer;
        public static string ConnectionString { get; set; } = null;
    }
}
