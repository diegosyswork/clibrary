using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.DataObjectProvider;
using SysWork.Data.Common.Utilities;
using SysWork.Data.GenericRepostory.Exceptions;

namespace SysWork.Data.LoggerDb
{
    /// <summary>
    /// Error Tags
    /// </summary>
    public enum EDbErrorTag
    {
        /// <summary>
        /// The open database error
        /// </summary>
        OpenDbError,
        /// <summary>
        /// The insert error
        /// </summary>
        InsertError,
        /// <summary>
        /// The update error
        /// </summary>
        UpdateError,
        /// <summary>
        /// The delete error
        /// </summary>
        DeleteError,
        /// <summary>
        /// The mapper error
        /// </summary>
        MapperError,
        /// <summary>
        /// The update database structure error
        /// </summary>
        UpdateDbStructError,
        /// <summary>
        /// The read error
        /// </summary>
        ReadError,
        /// <summary>
        /// The other error
        /// </summary>
        Error
    }

    /// <summary>
    /// Info Tags
    /// </summary>
    public enum EDbInfoTag
    {
        /// <summary>
        /// The insert information
        /// </summary>
        InsertInfo,
        /// <summary>
        /// The update information
        /// </summary>
        UpdateInfo,
        /// <summary>
        /// The delete information
        /// </summary>
        DeleteInfo,
        /// <summary>
        /// The information
        /// </summary>
        Info
    }

    /// <summary>
    /// Database Logger. Multi Database Engine.-
    /// </summary>
    public class DbLogger
    {
        /// <summary>
        /// Gets or sets the name of the application user.
        /// </summary>
        /// <value>
        /// The name of the application user.
        /// </value>
        public static string AppUserName { get; set; } = "";

        /// <summary>
        /// Gets or sets the name of the database user.
        /// </summary>
        /// <value>
        /// The name of the database user.
        /// </value>
        public static string DbUserName { get; set; } = "";


        static string _connectionString = null;
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                if ((value != ConnectionString) && (_dbLoggerInstance != null))
                    _dbLoggerInstance = null;

                _connectionString = value;
            }
        }

        private static EDataBaseEngine _dataBaseEngine;
        /// <summary>
        /// Gets or sets the data base engine.
        /// </summary>
        /// <value>
        /// The data base engine.
        /// </value>
        public static EDataBaseEngine DataBaseEngine
        {
            get
            {
                return _dataBaseEngine;
            }
            set
            {
                if ((value != DataBaseEngine) && (_dbLoggerInstance != null))
                    _dbLoggerInstance = null;

                _dataBaseEngine = value;
            }
        }

        private static string _dbLogTableName =  "LogDbV2";
        /// <summary>
        /// Gets or sets the name of the database log table.
        /// </summary>
        /// <value>
        /// The name of the database log table.
        /// </value>
        public static string DbLogTableName
        {
            get
            {
                return _dbLogTableName;
            }
            set
            {
                if ((value != _dbLogTableName) && (_dbLoggerInstance != null))
                    _dbLoggerInstance = null;

                _dbLogTableName = value;
            }
        }

        /// <summary>
        /// Gets or sets the dbCommandTimeout.
        /// </summary>
        /// <value>
        /// The default command timeout.
        /// </value>
        public static int DbCommandTimeout { get; set; } = 3;

        private static DbLogger _dbLoggerInstance = null;
        private static DbConnection _dbConnection = null;
        private static string _osVersion = "";
        private static string _machineName = "";
        private static string _osUser = "";

        private DbLogger()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentException("The ConnectionString is not set.");

            if (!DbUtil.ConnectionSuccess(_dataBaseEngine, ConnectionString, out string errMessage))
                throw new Exception(errMessage);

            if (!DbUtil.ExistsTable(_dataBaseEngine, ConnectionString, _dbLogTableName))
                DbUtil.ExecuteBatchNonQuery(_dataBaseEngine, GetLogDbScript(), ConnectionString);

            GetActiveConnection();

            _osVersion = GetSOVersion();
            _machineName = System.Environment.MachineName;
            _osUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }

        private static void GetActiveConnection()
        {
            _dbConnection = StaticDbObjectProvider.GetDbConnection(_dataBaseEngine, _connectionString);
            _dbConnection.Open();
        }

        private static void VerifyInstance()
        {
            if (_dbLoggerInstance== null)
                _dbLoggerInstance = new DbLogger();
        }

        private static string GetSOVersion()
        {
            string version =
                System.Environment.OSVersion.Version.Major.ToString() + "." +
                System.Environment.OSVersion.Version.Minor.ToString();

            string result;

            switch (version)
            {
                case "10.0":
                    result = "Windows 10/Server 2016";
                    break;
                case "6.3":
                    result = "Windows 8.1/Server 2012 R2";
                    break;
                case "6.2":
                    result = "Windows 8/Server 2012";
                    break;
                case "6.1":
                    result = "Windows 7/Server 2008 R2";
                    break;
                case "6.0":
                    result = "Windows Server 2008/Vista";
                    break;
                case "5.2":
                    result = "Windows Server 2003 R2/Server 2003/XP 64-Bit Edition";
                    break;
                case "5.1":
                    result = "Windows XP";
                    break;
                case "5.0":
                    result = "Windows 2000";
                    break;
                default:
                    result = "Unknown";
                    break;
            }

            result += (" " + System.Environment.OSVersion.ServicePack ?? "") + (System.Environment.Is64BitOperatingSystem ? " x64" : " x32");

            return result;
        }


        private string GetLogDbScript()
        {
            if (_dataBaseEngine == EDataBaseEngine.MSSqlServer)
                return GetLogDbScriptMSSQLServer();
            else if (_dataBaseEngine == EDataBaseEngine.SqLite)
                return GetLogDbScriptSQLite();
            else if (_dataBaseEngine == EDataBaseEngine.OleDb)
                return GetLogDbScriptOleDb();
            else if (_dataBaseEngine == EDataBaseEngine.MySql)
                return GetLogDbScriptMySql();
            else
                throw new ArgumentOutOfRangeException("The DataBaseEngine, is not supported by this method");
        }

        private string GetLogDbScriptMySql()
        {
            StringBuilder logDbScript = new StringBuilder("");

            logDbScript.AppendLine("   CREATE TABLE `" + _dbLogTableName + "` ");
            logDbScript.AppendLine("   (");
            logDbScript.AppendLine("   `idlog` INT NOT NULL AUTO_INCREMENT,");
            logDbScript.AppendLine("   `dateTime` DATETIME NOT NULL,");
            logDbScript.AppendLine("   `tag` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `summary` MEDIUMTEXT NOT NULL,");
            logDbScript.AppendLine("   `details` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `method` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `class` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `sqlstatement` MEDIUMTEXT NULL,");
            logDbScript.AppendLine("   `parameters` MEDIUMTEXT NULL,");
            logDbScript.AppendLine("   `excepcion` MEDIUMTEXT NULL,");
            logDbScript.AppendLine("   `result` MEDIUMTEXT NULL,");
            logDbScript.AppendLine("   `dbUser` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `osUser` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `sysUser` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `terminal` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `osversion` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   PRIMARY KEY(`idlog`));");

            return logDbScript.ToString();
        }

        private string GetLogDbScriptOleDb()
        {
            StringBuilder logDbScript = new StringBuilder("");

            logDbScript.AppendLine("CREATE TABLE " + _dbLogTableName + "");
            logDbScript.AppendLine("(");
            logDbScript.AppendLine("");
            logDbScript.AppendLine(" [idLog]   AUTOINCREMENT NOT NULL  ,");
            logDbScript.AppendLine(" [dateTime] DateTime NOT NULL, ");
            logDbScript.AppendLine(" [tag] text NULL,");
            logDbScript.AppendLine(" [summary] text NOT NULL,");
            logDbScript.AppendLine(" [details] text NULL,");
            logDbScript.AppendLine(" [method] text NULL, ");
            logDbScript.AppendLine(" [class] text NULL,");
            logDbScript.AppendLine(" [sqlStatement] text NULL,");
            logDbScript.AppendLine(" [parameters] text NULL,");
            logDbScript.AppendLine(" [exception] text NULL,");
            logDbScript.AppendLine(" [result] text NULL,");
            logDbScript.AppendLine(" [dbUser] text NULL,");
            logDbScript.AppendLine(" [osUser] text NULL,");
            logDbScript.AppendLine(" [sysUser] text NULL,");
            logDbScript.AppendLine(" [terminal] text NULL,");
            logDbScript.AppendLine(" [osVersion] text NULL,");
            logDbScript.AppendLine(" CONSTRAINT[PK_Log] PRIMARY KEY");
            logDbScript.AppendLine(" (");
            logDbScript.AppendLine(" [idLog]");
            logDbScript.AppendLine(" )");
            logDbScript.AppendLine(" )");

            return logDbScript.ToString();
        }

        private string GetLogDbScriptSQLite()
        {
            StringBuilder logDbScript = new StringBuilder("");

            logDbScript.AppendLine("CREATE TABLE[" + _dbLogTableName + "]");
            logDbScript.AppendLine("(");
            logDbScript.AppendLine("[idLog]INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,");
            logDbScript.AppendLine("[dateTime] TEXT NOT NULL,");
            logDbScript.AppendLine("[tag] TEXT NULL, ");
            logDbScript.AppendLine("[summary] TEXT NOT NULL, ");
            logDbScript.AppendLine("[details] TEXT NULL, ");
            logDbScript.AppendLine("[method] TEXT NULL, ");
            logDbScript.AppendLine("[class] TEXT NULL, ");
            logDbScript.AppendLine("[sqlStatement] TEXT NULL, ");
            logDbScript.AppendLine("[parameters] TEXT NULL, ");
            logDbScript.AppendLine("[exception] TEXT NULL, ");
            logDbScript.AppendLine("[result] TEXT NULL, ");
            logDbScript.AppendLine("[dbUser] TEXT NULL, ");
            logDbScript.AppendLine("[osUser] TEXT NULL, ");
            logDbScript.AppendLine("[sysUser] TEXT NULL, ");
            logDbScript.AppendLine("[terminal] TEXT NULL, ");
            logDbScript.AppendLine("[osVersion] TEXT NULL ");
            logDbScript.AppendLine(")");

            return logDbScript.ToString();
        }

        private string GetLogDbScriptMSSQLServer()
        {
            StringBuilder logDbScript = new StringBuilder("");


            logDbScript.AppendLine(" SET ANSI_NULLS ON ");
            logDbScript.AppendLine(" GO ");
            logDbScript.AppendLine(" SET QUOTED_IDENTIFIER ON ");
            logDbScript.AppendLine(" GO ");
            logDbScript.AppendLine(" CREATE TABLE[dbo].[" + _dbLogTableName + "] ( ");
            logDbScript.AppendLine("     [IdLog] [int] IDENTITY(1,1) NOT NULL,");
            logDbScript.AppendLine("     [dateTime] [datetime]NOT NULL,");
            logDbScript.AppendLine("     [Tag] [nvarchar] (200) NULL,");
            logDbScript.AppendLine("     [Summary] [nvarchar] (200) NULL,");
            logDbScript.AppendLine("     [Details][nvarchar](max) NULL,");
            logDbScript.AppendLine("     [Method] [nvarchar] (200) NULL,");
            logDbScript.AppendLine("     [Class] [nvarchar] (200) NULL,");
            logDbScript.AppendLine("     [SQLStatement][nvarchar](max) NULL,");
            logDbScript.AppendLine("     [Parameters] [nvarchar](max) NULL,");
            logDbScript.AppendLine("     [Exception] [nvarchar](max) NULL,");
            logDbScript.AppendLine("     [Result] [nvarchar](max) NULL,");
            logDbScript.AppendLine("     [DbUser] [nvarchar] (200) NULL,");
            logDbScript.AppendLine("     [OsUser] [nvarchar] (200) NULL,");
            logDbScript.AppendLine("     [SysUser] [nvarchar] (200) NULL,");
            logDbScript.AppendLine("     [Terminal] [nvarchar] (200) NULL,");
            logDbScript.AppendLine("     [OsVersion] [nvarchar] (200) NULL,");
            logDbScript.AppendLine(" CONSTRAINT[PK_LogDb] PRIMARY KEY CLUSTERED ");
            logDbScript.AppendLine(" ( ");
            logDbScript.AppendLine("    [idLog] ASC ");
            logDbScript.AppendLine(" )  ON[PRIMARY] ");
            logDbScript.AppendLine(" ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY] ");
            logDbScript.AppendLine(" GO ");

            return logDbScript.ToString();
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="eErrorTag">The e error tag.</param>
        /// <param name="summary">The summary.</param>
        /// <returns></returns>
        public static bool LogError(EDbErrorTag eErrorTag, string summary)
        {
            return DbLog(eErrorTag, summary);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="eErrorTag">The e error tag.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static bool LogError(EDbErrorTag eErrorTag, string summary, Exception exception)
        {
            return DbLog(eErrorTag, summary, exception: exception.ToString());
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="eErrorTag">The e error tag.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="repositoryException">The repository exception.</param>
        /// <returns></returns>
        public static bool LogError(EDbErrorTag eErrorTag, string summary, GenericRepositoryException repositoryException)
        {
            string sqlStatement = (repositoryException.DbCommand != null) ? DbUtil.ConvertCommandParamatersToLiteralValues((repositoryException.DbCommand)) : null;
            return DbLog(eErrorTag, summary, sqlStatement: sqlStatement, exception: repositoryException.OriginalException.ToString());
        }



        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="eErrorTag">The e error tag.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        public static bool LogError(EDbErrorTag eErrorTag, string summary, string details)
        {
            return DbLog(eErrorTag, summary, details);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="eErrorTag">The e error tag.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="details">The details.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static bool LogError(EDbErrorTag eErrorTag, string summary, string details, Exception exception)
        {
            return DbLog(eErrorTag, summary, details,exception:exception.ToString());
        }


        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="eErrorTag">The e error tag.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="dbCommand">The database command.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static bool LogError(EDbErrorTag eErrorTag, string summary, IDbCommand dbCommand, Exception exception)
        {
            string sqlStatement = (dbCommand != null) ? DbUtil.ConvertCommandParamatersToLiteralValues(dbCommand) : null;
            return DbLog(eErrorTag, summary,sqlStatement:sqlStatement,exception:exception.ToString());
        }

        /// <summary>
        /// Logs error.
        /// </summary>
        /// <param name="eErrorTag">The e error tag.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="details">The details.</param>
        /// <param name="dbCommand">The database command.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static bool LogError(EDbErrorTag eErrorTag, string summary,string details, IDbCommand dbCommand, Exception exception)
        {
            string sqlStatement = (dbCommand != null) ? DbUtil.ConvertCommandParamatersToLiteralValues(dbCommand) : null;

            return DbLog(eErrorTag, summary, sqlStatement: sqlStatement, exception: exception.ToString(),details:details);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="eInfoTag">The e information tag.</param>
        /// <param name="summary">The summary.</param>
        /// <returns></returns>
        public static bool LogInfo(EDbInfoTag eInfoTag, string summary)
        {
            return DbLog(eInfoTag, summary);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="eInfoTag">The e information tag.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        public static bool LogInfo(EDbInfoTag eInfoTag, string summary, string details)
        {
            return DbLog(eInfoTag, summary, details);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="eInfoTag">The e information tag.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="dbCommand">The database command.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool LogInfo(EDbInfoTag eInfoTag, string summary, IDbCommand dbCommand, string result)
        {
            string sqlStatement = (dbCommand != null) ? DbUtil.ConvertCommandParamatersToLiteralValues(dbCommand) : null;

            return DbLog(eInfoTag, summary, sqlStatement: sqlStatement,result:result);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="eInfoTag">The e information tag.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="details">The details.</param>
        /// <param name="dbCommand">The database command.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool LogInfo(EDbInfoTag eInfoTag, string summary, string details, IDbCommand dbCommand, string result)
        {
            string sqlStatement = (dbCommand != null) ? DbUtil.ConvertCommandParamatersToLiteralValues(dbCommand) : null;

            return DbLog(eInfoTag, summary, details, sqlStatement: sqlStatement, result: result);
        }


        private static bool DbLog(Object tag, string summary = null, string details = null,
            string method = null, string _class = null, string sqlStatement = null, string parameters = null, string exception = null,
            string result = null, string dbUser = null, string osUser = null, string sysUser = null, string terminal = null, string osVersion = null)
        {

            VerifyInstance();

            StackTrace st = new StackTrace();

            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,0);
            

            string _stringTag = null;

            if (tag.GetType().Equals(typeof(EDbErrorTag)))
                _stringTag = ((EDbErrorTag)tag).ToString();

            if (tag.GetType().Equals(typeof(EDbInfoTag)))
                _stringTag = ((EDbInfoTag)tag).ToString();

            if (_class == null)
                _class = st.GetFrame(2).GetMethod().DeclaringType.Name;

            if (method == null)
                method = st.GetFrame(2).GetMethod().Name;

            if (osUser == null)
                osUser = _osUser;

            if (sysUser == null)
                sysUser = AppUserName;

            if (terminal == null)
                terminal = _machineName;

            if (osVersion == null)
                osVersion = _osVersion;

            if (dbUser == null)
                dbUser = DbUserName;

            try
            {
                if (_dbConnection.State != ConnectionState.Open)
                    GetActiveConnection();

                var queryResult = new DbExecutor(_dbConnection) 
                    .InsertQuery(_dbLogTableName)
                    .AddFieldWithValue("DateTime", dateTime)
                    .AddFieldWithValue("Tag", _stringTag, 200)
                    .AddFieldWithValue("Summary", summary, 200)
                    .AddFieldWithValue("Details", details)
                    .AddFieldWithValue("Method", method, 200)
                    .AddFieldWithValue("Class", _class, 200)
                    .AddFieldWithValue("SQLStatement", sqlStatement)
                    .AddFieldWithValue("Parameters", parameters)
                    .AddFieldWithValue("Exception", exception)
                    .AddFieldWithValue("Result", result)
                    .AddFieldWithValue("DbUser", dbUser)
                    .AddFieldWithValue("OsUser", osUser)
                    .AddFieldWithValue("SysUser", sysUser)
                    .AddFieldWithValue("Terminal", terminal)
                    .AddFieldWithValue("OsVersion", osVersion)
                    .ExecuteNonQuery(DbCommandTimeout);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

    }
}
