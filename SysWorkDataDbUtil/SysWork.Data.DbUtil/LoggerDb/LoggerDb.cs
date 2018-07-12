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
            Info
        };

        public static string ConnectionString { get; set; }

        private static LoggerDb LogDbInstance = null;


        private LoggerDb()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentException("No se ha informado el connectionString del LoggerDB");


            if (!DbUtil.ExistsTable(ConnectionString, "LogDB"))
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
        public static bool Log(string tag, string mensaje,Exception excepcion)
        {
            return Logger(DateTime.Now, mensaje: mensaje, tag: tag,excepcion:excepcion.ToString());
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
            logDb.FechaHora = fechaHora;
            logDb.Usuario = usuario;
            logDb.Tag = tag;
            logDb.Mensaje = mensaje;

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
                    sqlCommand.Parameters["@pFechaHora"].Value = entity.FechaHora;

                    sqlCommand.Parameters.Add("@pUsuario", SqlDbType.NVarChar, 200);
                    sqlCommand.Parameters["@pUsuario"].Value = entity.Usuario == null ? (object)DBNull.Value : entity.Usuario;

                    sqlCommand.Parameters.Add("@ptag", SqlDbType.NVarChar, 200);
                    sqlCommand.Parameters["@ptag"].Value = entity.Tag == null ? (object)DBNull.Value : entity.Tag;

                    sqlCommand.Parameters.Add("@pMensaje", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pMensaje"].Value = entity.Mensaje;

                    sqlCommand.Parameters.Add("@pModulo", SqlDbType.NVarChar, 200);
                    sqlCommand.Parameters["@pModulo"].Value = entity.Modulo == null ? (object)DBNull.Value : entity.Modulo;

                    sqlCommand.Parameters.Add("@pMetodo", SqlDbType.NVarChar, 200);
                    sqlCommand.Parameters["@pMetodo"].Value = entity.Metodo == null ? (object)DBNull.Value : entity.Metodo;

                    sqlCommand.Parameters.Add("@pSentenciaSQL", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pSentenciaSQL"].Value = entity.SentenciaSQL == null ? (object)DBNull.Value : entity.SentenciaSQL;

                    sqlCommand.Parameters.Add("@pParametros", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pParametros"].Value = entity.Parametros == null ? (object)DBNull.Value : entity.Parametros;

                    sqlCommand.Parameters.Add("@pResultado", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pResultado"].Value = entity.Resultado == null ? (object)DBNull.Value : entity.Resultado;

                    sqlCommand.Parameters.Add("@pExcepcion", SqlDbType.NVarChar);
                    sqlCommand.Parameters["@pExcepcion"].Value = entity.Excepcion == null ? (object)DBNull.Value : entity.Excepcion;

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
        public long? IdLogDb { get; set; }
        public DateTime FechaHora { get; set; }
        public string Usuario { get; set; }
        public string Tag { get; set; }
        public string Mensaje { get; set; }
        public string Modulo { get; set; }
        public string Metodo { get; set; }
        public string SentenciaSQL { get; set; }
        public string Parametros { get; set; }
        public string Resultado { get; set; }
        public string Excepcion { get; set; }
    }


}
