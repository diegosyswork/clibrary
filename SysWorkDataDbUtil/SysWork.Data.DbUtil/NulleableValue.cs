using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Data.DbUtil
{
    public class NulleableValue
    {
        /// <summary>
        /// Valida si la cadena recibida es un DateTime valido, y en caso que 
        /// si castea, sino devuelve NULL
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime? NulleableDateTime(string dateTime)
        {
            DateTime? retorno = null;
            DateTime resultado;

            if (DateTime.TryParse(dateTime, out resultado))
                retorno = resultado;

            return retorno;
        }
        /// <summary>
        /// Util para cuando se desea tomar el SelectedValue de un comboBox por ejemplo
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static long? NulleableLong(object valor)
        {
            long? retorno = null;

            if (valor != null)
                retorno = (long)valor;

            return retorno;
        }

    }
}
