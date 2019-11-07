using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Format.AS400
{
    /// <summary>
    /// Helper to manage AS400 Numbers
    /// </summary>
    public static class HelperAS400
    {
        /// <summary>
        /// AS400 number to decimal.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="ints">The ints.</param>
        /// <param name="decimals">The decimals.</param>
        /// <returns>
        /// A decimal number.
        /// </returns>
        public static decimal AS400NumberToDecimal(string number, int ints, int decimals)
        {
            return decimal.Parse(number.Substring(0, ints)) + (decimal.Parse(number.Substring(ints, decimals)) / 100);
        }


        /// <summary>
        /// Decimals to a S400 number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="ints">Integers positions.</param>
        /// <param name="decimals">Decimals positions.</param>
        /// <returns>
        /// A number in AS400 format (string).
        /// </returns>
        public static string DecimalToAS400Number(decimal number, int ints, int decimals)
        {
            string returnValue = "";

            var formatInts = new string('0', ints);
            var formatDecimals = new string('0', decimals);

            formatInts = "{0:" + formatInts + "}";
            formatDecimals = "{0:" + formatDecimals + "}";

            decimal decimalPart = (number - Math.Truncate(number)) * 100;


            returnValue += string.Format(formatInts, Math.Truncate(number)) + string.Format(formatDecimals, decimalPart)  ;

            return returnValue;
        }

        public static DateTime AS400DDMMYYYYToDateTime(string AS400_DDMMYYYY_Date)
        {
            int year = int.Parse(AS400_DDMMYYYY_Date.Substring(4, 4));
            int month = int.Parse(AS400_DDMMYYYY_Date.Substring(2, 2));
            int day = int.Parse(AS400_DDMMYYYY_Date.Substring(0, 2));

            return new DateTime(year, month, day);
        }

        public static DateTime AS400YYYYMMDDToDateTime(string AS400_YYYYMMDD_Date)
        {
            int year = int.Parse(AS400_YYYYMMDD_Date.Substring(0, 4));
            int month = int.Parse(AS400_YYYYMMDD_Date.Substring(4, 2));
            int day = int.Parse(AS400_YYYYMMDD_Date.Substring(6, 2));

            return new DateTime(year, month, day);
        }
    }
}
