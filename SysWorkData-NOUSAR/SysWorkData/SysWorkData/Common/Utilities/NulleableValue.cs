using System;

namespace SysWork.Data.Common.Utilities
{
    /// <summary>
    /// Helper to handle nulleables values.
    /// </summary>
    public class NulleableValue
    {

        /// <summary>
        /// Validate if the received string is a valid DateTime value, if not, return null.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime? NulleableDateTime(string dateTime)
        {
            DateTime? retorno = null;

            if (DateTime.TryParse(dateTime, out DateTime resultado))
                retorno = resultado;

            return retorno;
        }

        /// <summary>
        /// Validate if the received object is a valid nulleable long. if not, return null.
        /// Util to get SelectedValue form comboBox. 
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
