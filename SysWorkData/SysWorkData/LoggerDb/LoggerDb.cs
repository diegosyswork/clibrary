using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.Common.Utilities;
using SysWork.Data.DaoModel;
using SysWork.Data.DaoModel.Attributes;

namespace SysWork.Data.Logger
{
    public class LoggerDb
    {
        public enum ELoggerDbTagError
        {
            ErrorIntentandoAbrirConexion,
            ErrorDeInsercion,
            ErrorDeActualizacion,
            ErrorDeEliminacion,
            ErrorReaderToEntity,
            ErrorActualizandoEstructuraBaseDatos,
            ErrorDeLectura
        };
        public enum ELoggerDbTagInfo
        {
            InfoSeAgregoUnRegistro,
            InfoSeModificoUnRegistro,
            InfoSeEliminoUnRegistro,
            Info
        };

        private static string _connectionString;

        public static string ConnectionString
        {
            get{ return _connectionString; }
            set
            {
                if (value != _connectionString)
                    _loggerDbInstance = null;

                _connectionString = value;
            }
        }

        private static EDataBaseEngine _dataBaseEngine;
        public static EDataBaseEngine DataBaseEngine
        {
            get { return _dataBaseEngine; }
            set
            {
                if (value != _dataBaseEngine)
                    _loggerDbInstance = null;

                _dataBaseEngine = value;
            }
        }

        private static LoggerDb _loggerDbInstance = null;
        private static DaoLogDb _daoLogDb = null;

        private LoggerDb()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentException("No se ha informado el connectionString del LoggerDB");

            if (!DbUtil.ConnectionSuccess(_dataBaseEngine, ConnectionString, out string mensajeError))
            {
                throw new Exception(mensajeError);
            }

            if (!DbUtil.ExistsTable(_dataBaseEngine, ConnectionString, "LogDB"))
                DbUtil.ExecuteBatchNonQuery(_dataBaseEngine, GetLogDbScript(), ConnectionString);

            _daoLogDb = new DaoLogDb(ConnectionString, _dataBaseEngine);
            
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
                throw new Exception("No se ha especificado un SCRIPT para el tipo de base de datos establecida");
        }

        private string GetLogDbScriptMySql()
        {
            StringBuilder logDbScript = new StringBuilder("");

            logDbScript.AppendLine("   CREATE TABLE `logdb` (");
            logDbScript.AppendLine("   `idlogdb` INT NOT NULL AUTO_INCREMENT,");
            logDbScript.AppendLine("   `fechaHora` DATETIME NOT NULL,");
            logDbScript.AppendLine("   `usuario` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `tag` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `mensaje` MEDIUMTEXT NOT NULL,");
            logDbScript.AppendLine("   `modulo` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `metodo` NVARCHAR(200) NULL,");
            logDbScript.AppendLine("   `sentenciaSQL` INT NULL,");
            logDbScript.AppendLine("   `parametros` MEDIUMTEXT NULL,");
            logDbScript.AppendLine("   `resultado` MEDIUMTEXT NULL,");
            logDbScript.AppendLine("   `excepcion` MEDIUMTEXT NULL,");
            logDbScript.AppendLine("   PRIMARY KEY(`idlogdb`));");

            return logDbScript.ToString();
        }

        private string GetLogDbScriptOleDb()
        {
            StringBuilder logDbScript = new StringBuilder("");

            logDbScript.AppendLine("CREATE TABLE LogDb");
            logDbScript.AppendLine("(");
            logDbScript.AppendLine("");
            logDbScript.AppendLine(" [idLogDb]   AUTOINCREMENT NOT NULL  ,");
            logDbScript.AppendLine(" [fechaHora] DateTime NOT NULL, ");
            logDbScript.AppendLine(" [usuario] text NULL,");
            logDbScript.AppendLine("[tag] text NULL,");
            logDbScript.AppendLine(" [mensaje] text NOT NULL, ");
            logDbScript.AppendLine(" [modulo] text NULL,");
            logDbScript.AppendLine(" [metodo] text NULL,");
            logDbScript.AppendLine(" [sentenciaSQL] text NULL,");
            logDbScript.AppendLine(" [parametros] text NULL,");
            logDbScript.AppendLine(" [resultado] text NULL,");
            logDbScript.AppendLine(" [excepcion] text NULL,");
            logDbScript.AppendLine(" CONSTRAINT[PK_Log] PRIMARY KEY");
            logDbScript.AppendLine(" (");
            logDbScript.AppendLine(" [idLogDb]");
            logDbScript.AppendLine(" )");
            logDbScript.AppendLine(" )");

            return logDbScript.ToString();
        }

        private string GetLogDbScriptSQLite()
        {
            StringBuilder logDbScript = new StringBuilder("");

            logDbScript.AppendLine("CREATE TABLE[LogDb]");
            logDbScript.AppendLine("(");
            logDbScript.AppendLine("[idLogDb]INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,");
            logDbScript.AppendLine("[fechaHora] TEXT NOT NULL,");
            logDbScript.AppendLine("[usuario] TEXT NULL, ");
            logDbScript.AppendLine("[tag] TEXT NULL, ");
            logDbScript.AppendLine("[mensaje] TEXT NOT NULL, ");
            logDbScript.AppendLine("[modulo] TEXT NULL,");
            logDbScript.AppendLine("[metodo] TEXT NULL,");
            logDbScript.AppendLine("[sentenciaSQL] TEXT NULL,");
            logDbScript.AppendLine("[parametros] TEXT NULL,");
            logDbScript.AppendLine("[resultado] TEXT NULL,");
            logDbScript.AppendLine("[excepcion] TEXT NULL )");

            return logDbScript.ToString();
        }

        private string GetLogDbScriptMSSQLServer()
        {
            StringBuilder logDbScript = new StringBuilder("");

            logDbScript.AppendLine(" SET ANSI_NULLS ON ");
            logDbScript.AppendLine(" GO ");
            logDbScript.AppendLine(" SET QUOTED_IDENTIFIER ON ");
            logDbScript.AppendLine(" GO ");
            logDbScript.AppendLine(" CREATE TABLE[dbo].[LogDb] ( ");
            logDbScript.AppendLine(" [idLogDb][int] IDENTITY(1,1) NOT NULL, ");
            logDbScript.AppendLine(" [fechaHora] [datetime] NOT NULL, ");
            logDbScript.AppendLine(" [usuario] [nvarchar] (200) NULL, ");
            logDbScript.AppendLine(" [tag] [nvarchar] (200) NULL, ");
            logDbScript.AppendLine(" [mensaje] [nvarchar] (max) NOT NULL, ");
            logDbScript.AppendLine(" [modulo] [nvarchar] (200) NULL, ");
            logDbScript.AppendLine(" [metodo] [nvarchar] (200) NULL, ");
            logDbScript.AppendLine(" [sentenciaSQL] [nvarchar] (max) NULL, ");
            logDbScript.AppendLine(" [parametros] [nvarchar](max) NULL, ");
            logDbScript.AppendLine(" [resultado] [nvarchar] (max) NULL, ");
            logDbScript.AppendLine(" [excepcion] [nvarchar](max) NULL, ");
            logDbScript.AppendLine(" CONSTRAINT[PK_LogDb] PRIMARY KEY CLUSTERED ");
            logDbScript.AppendLine(" ( ");
            logDbScript.AppendLine("    [idLogDb] ASC ");
            logDbScript.AppendLine(" )  ON[PRIMARY] ");
            logDbScript.AppendLine(" ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY] ");
            logDbScript.AppendLine(" GO ");

            return logDbScript.ToString();
        }

        private static void VerifyInstance()
        {
            if (_loggerDbInstance == null)
                _loggerDbInstance = new LoggerDb();
        }
        public static bool Log(string mensaje)
        {
            return Logger(DateTime.Now, mensaje);
        }
        public static bool Log(string tag, string mensaje)
        {
            return Logger(DateTime.Now, mensaje: mensaje, tag: tag);
        }
        public static bool Log(string tag, string mensaje, Exception excepcion)
        {
            return Logger(DateTime.Now, mensaje: mensaje, tag: tag, excepcion: excepcion.ToString());
        }
        public static bool Log(string tag, string mensaje, string usuario)
        {
            return Logger(DateTime.Now, mensaje: mensaje, tag: tag, usuario: usuario);
        }
        public static bool Log(string tag, string mensaje, string excepcion, string sentenciaSQL)
        {
            return Logger(DateTime.Now, mensaje: mensaje, tag: tag, excepcion: excepcion, sentenciaSQL: sentenciaSQL);
        }
        private static bool Log(DateTime fechaHora, string mensaje, string usuario = null, string tag = null, string modulo = null, string metodo = null, string sentenciaSQL = null, string parametros = null, string resultado = null, string excepcion = null)
        {
            return Logger(fechaHora, mensaje, usuario, tag, modulo, metodo, sentenciaSQL, parametros, resultado, excepcion);
        }
        public static bool Log(string tag, IDbCommand dbCommand)
        {
            string sentenciaSQL = (dbCommand == null) ? DbUtil.ConvertCommandParamatersToLiteralValues((dbCommand)) : null;
            return Logger(DateTime.Now, tag: tag, mensaje: "", sentenciaSQL: sentenciaSQL);
        }
        public static bool Log(string tag, string mensaje, IDbCommand dbCommand)
        {
            string sentenciaSQL = (dbCommand == null) ? DbUtil.ConvertCommandParamatersToLiteralValues((dbCommand)) : null;
            return Logger(DateTime.Now, mensaje: mensaje, sentenciaSQL: sentenciaSQL, tag: tag);
        }

        public static bool Log(string tag, IDbCommand dbCommand, Exception exception)
        {

            string sentenciaSQL = (dbCommand == null) ? DbUtil.ConvertCommandParamatersToLiteralValues((dbCommand)) : null;
            return Logger(DateTime.Now, tag: tag, mensaje: "", sentenciaSQL: sentenciaSQL, excepcion: exception.ToString());
        }
        public static bool Log(string tag, Exception exception)
        {
            return Logger(DateTime.Now, tag: tag, mensaje: "", sentenciaSQL: "", excepcion: exception.ToString());
        }

        private static bool Logger(DateTime fechaHora, string mensaje, string usuario = null, string tag = null, string modulo = null, string metodo = null, string sentenciaSQL = null, string parametros = null, string resultado = null, string excepcion = null)
        {
            VerifyInstance();

            StackTrace st = new StackTrace();

            LogDb logDb = new LogDb
            {
                FechaHora = fechaHora,
                Usuario = usuario,
                Tag = tag,
                Mensaje = mensaje
            };

            if (modulo == null)
                modulo = st.GetFrame(2).GetMethod().DeclaringType.Name;

            logDb.Modulo = modulo;

            if (metodo == null)
                metodo = st.GetFrame(2).GetMethod().Name;

            logDb.Metodo = metodo;

            logDb.SentenciaSQL = sentenciaSQL;
            logDb.Parametros = parametros;
            logDb.Resultado = resultado;
            logDb.Excepcion = excepcion;

            return _daoLogDb.Add(logDb) != -1;
        }
    }
    internal class DaoLogDb : BaseDao<LogDb>
    {
        public DaoLogDb(string ConnectionString) : base(ConnectionString)
        {

        }
        public DaoLogDb(string ConnectionString,EDataBaseEngine dataBaseEngine) : base(ConnectionString, dataBaseEngine)
        {

        }
    }

    [DbTable(Name = "LogDb")]
    internal class LogDb
    {
        [DbColumn (IsIdentity = true, IsPrimary = true)]
        public long? IdLogDb { get; set; }
        [DbColumn()]
        public DateTime FechaHora { get; set; }
        [DbColumn()]
        public string Usuario { get; set; }
        [DbColumn()]
        public string Tag { get; set; }
        [DbColumn()]
        public string Mensaje { get; set; }
        [DbColumn()]
        public string Modulo { get; set; }
        [DbColumn()]
        public string Metodo { get; set; }
        [DbColumn()]
        public string SentenciaSQL { get; set; }
        [DbColumn()]
        public string Parametros { get; set; }
        [DbColumn()]
        public string Resultado { get; set; }
        [DbColumn()]
        public string Excepcion { get; set; }
    }
}
