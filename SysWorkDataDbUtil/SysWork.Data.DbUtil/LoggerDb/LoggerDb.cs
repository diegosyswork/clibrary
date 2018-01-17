using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.DbUtil.LoggerDb
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
        };

        public static string ConnectionString { get; set; }

        private static LoggerDb LogDbInstance = null;


        private LoggerDb()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentException("No se ha informado el connectionString del LoggerDB");


            if (!DbUtil.ExistsTable(ConnectionString, "LoggerDB"))
                DbUtil.ExecuteBatchNonQuery(GetLogDbScript(), new SqlConnection(ConnectionString));
        }

        private string GetLogDbScript()
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
            logDbScript.AppendLine(" CONSTRAINT[PK_Log] PRIMARY KEY CLUSTERED ");
            logDbScript.AppendLine(" ( ");
            logDbScript.AppendLine("    [idLogDb] ASC ");
            logDbScript.AppendLine(" ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY] ");
            logDbScript.AppendLine(" ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY] ");
            logDbScript.AppendLine(" GO ");

            return logDbScript.ToString();
        }

        private static void VerifyInstance()
        {
            if (LogDbInstance == null)
                LogDbInstance = new LoggerDb();
        }
        public static bool Log(string mensaje)
        {
            return Logger(DateTime.Now, mensaje);
        }
        public static bool Log(string tag, string mensaje)
        {
            return Logger(DateTime.Now, mensaje: mensaje, tag: tag);
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

        public static bool Log(string tag, SqlCommand sqlCmd)
        {
            string sentenciaSQL = DbUtil.ConvertCommandParamatersToLiteralValues(sqlCmd);
            return Logger(DateTime.Now, tag: tag, mensaje: "", sentenciaSQL: sentenciaSQL);
        }
        public static bool Log(string tag, string mensaje, SqlCommand sqlCmd)
        {
            string sentenciaSQL = DbUtil.ConvertCommandParamatersToLiteralValues(sqlCmd);
            return Logger(DateTime.Now, mensaje: mensaje, sentenciaSQL: sentenciaSQL, tag: tag);
        }

        public static bool Log(string tag, SqlCommand sqlCmd, Exception exception)
        {
            string sentenciaSQL = DbUtil.ConvertCommandParamatersToLiteralValues((sqlCmd));
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

            LogDb logDb = new LogDb();
            logDb.fechaHora = fechaHora;
            logDb.usuario = usuario;
            logDb.tag = tag;
            logDb.mensaje = mensaje;

            if (modulo == null)
                modulo = st.GetFrame(2).GetMethod().DeclaringType.Name;

            logDb.modulo = modulo;

            if (metodo == null)
                metodo = st.GetFrame(2).GetMethod().Name;

            logDb.metodo = metodo;

            logDb.sentenciaSQL = sentenciaSQL;
            logDb.parametros = parametros;
            logDb.resultado = resultado;
            logDb.excepcion = excepcion;

            return Add(logDb) != -1;
        }

        private static long Add(LogDb entity)
        {
            long id = -1;

            string sqlQuery = "INSERT INTO LogDb (fechaHora,usuario,tag,mensaje,modulo,metodo,sentenciaSQL,parametros,resultado,excepcion) OUTPUT INSERTED.idLogDb ";
            sqlQuery += " VALUES (@pFechaHora,@pUsuario,@ptag,@pMensaje,@pModulo,@pMetodo,@pSentenciaSQL,@pParametros,@pResultado,@pExcepcion)";

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

                    sqlCommand.Parameters.Add("@pFechaHora", SqlDbType.DateTime);
                    sqlCommand.Parameters["@pFechaHora"].Value = entity.fechaHora;

                    sqlCommand.Parameters.Add("@pUsuario", SqlDbType.NVarChar, 200);
                    sqlCommand.Parameters["@pUsuario"].Value = entity.usuario == null ? (object)DBNull.Value : entity.usuario;

                    sqlCommand.Parameters.Add("@ptag", SqlDbType.NVarChar, 200);
                    sqlCommand.Parameters["@ptag"].Value = entity.tag == null ? (object)DBNull.Value : entity.tag;

                    sqlCommand.Parameters.Add("@pMensaje", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pMensaje"].Value = entity.mensaje;

                    sqlCommand.Parameters.Add("@pModulo", SqlDbType.NVarChar, 200);
                    sqlCommand.Parameters["@pModulo"].Value = entity.modulo == null ? (object)DBNull.Value : entity.modulo;

                    sqlCommand.Parameters.Add("@pMetodo", SqlDbType.NVarChar, 200);
                    sqlCommand.Parameters["@pMetodo"].Value = entity.metodo == null ? (object)DBNull.Value : entity.metodo;

                    sqlCommand.Parameters.Add("@pSentenciaSQL", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pSentenciaSQL"].Value = entity.sentenciaSQL == null ? (object)DBNull.Value : entity.sentenciaSQL;

                    sqlCommand.Parameters.Add("@pParametros", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pParametros"].Value = entity.parametros == null ? (object)DBNull.Value : entity.parametros;

                    sqlCommand.Parameters.Add("@pResultado", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pResultado"].Value = entity.resultado == null ? (object)DBNull.Value : entity.resultado;

                    sqlCommand.Parameters.Add("@pExcepcion", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pExcepcion"].Value = entity.excepcion == null ? (object)DBNull.Value : entity.excepcion;

                    id = (Int32)sqlCommand.ExecuteScalar();
                }
                catch (Exception e)
                {

                }
                finally
                {
                    sqlConnection.Dispose();
                    sqlConnection.Close();
                }
            }
            return id;
        }
    }

    class LogDb
    {
        public long? idLogDb { get; set; }
        public DateTime fechaHora { get; set; }
        public string usuario { get; set; }
        public string tag { get; set; }
        public string mensaje { get; set; }
        public string modulo { get; set; }
        public string metodo { get; set; }
        public string sentenciaSQL { get; set; }
        public string parametros { get; set; }
        public string resultado { get; set; }
        public string excepcion { get; set; }
    }


}
