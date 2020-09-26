using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text.RegularExpressions;
/*
    Existe un tema en OleDb(y Odbc también) no admite parámetros con nombre.
    Solo admite lo que se llama parámetros posicionales.
    En otras palabras: el nombre que le da a un parámetro cuando lo agrega a la lista de parámetros de comandos no importa. 
    Solo se usa internamente por la clase OleDbCommand para que pueda distinguir y hacer referencia a los parámetros.
    Lo que importa es el orden en el que agrega los parámetros a la lista.
    Debe ser el mismo orden en que se hace referencia a los parámetros en la declaración de SQL 
    a través del carácter de signo de interrogación (?).

    Pero aquí hay una solución que le permite usar parámetros con nombre en la declaración de SQL.
    Básicamente reemplaza todas las referencias de parámetros en la declaración de SQL con signos de interrogación y 
    reordena la lista de parámetros en consecuencia. 
    Funciona de la misma manera para la clase OdbcCommand, solo necesita reemplazar "OleDb" con "Odbc" en el código.


    Gloria a DIOS por StackOverflow
    https://stackoverflow.com/questions/2407685/oledbparameters-and-parameter-names
*/
namespace SysWork.Data.Common.Extensions.OleDbCommandExtensions
{
    /// <summary>
    /// Extensions for OleDbCommand's
    /// </summary>
    public static class OleDbCommandExtensions
    {
        /// <summary>
        /// Converts the named parameters to positional parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        public static void ConvertNamedParametersToPositionalParameters(this OleDbCommand command)
        {
            //1. Find all occurrences of parameter references in the SQL statement (such as @MyParameter).
            //2. Find the corresponding parameter in the commands parameters list.
            //3. Add the found parameter to the newParameters list and replace the parameter reference in the SQL with a question mark (?).
            //4. Replace the commands parameters list with the newParameters list.

            var newParameters = new List<OleDbParameter>();

            command.CommandText = Regex.Replace(command.CommandText, "(@\\w*)", match =>
            {
                var parameter = command.Parameters.OfType<OleDbParameter>().FirstOrDefault(a => a.ParameterName == match.Groups[1].Value);
                if (parameter != null)
                {
                    var parameterIndex = newParameters.Count;

                    var newParameter = command.CreateParameter();
                    newParameter.OleDbType = parameter.OleDbType;
                    newParameter.ParameterName = "@parameter" + parameterIndex.ToString();
                    newParameter.Value = parameter.Value;

                    newParameters.Add(newParameter);
                }
                return "?";
            });

            command.Parameters.Clear();
            command.Parameters.AddRange(newParameters.ToArray());
        }
    }
}
