using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SysWork.Format.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns "n" characters to the right of a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The Lenght.</param>
        /// <returns>
        /// The Right part of string.
        /// </returns>
        /// <example>
        /// var name = "DIEGO MARTINEZ";
        /// var lastFourLetters = name.Right(4);
        /// //The value of lastFourLetters is "INEZ";
        /// </example>
        public static string Right(this string value, int length)
        {
            // if length is greater than "size" resets "size"
            length = (value.Length < length ? value.Length : length);
            string newValue = value.Substring(value.Length - length);
            return newValue;
        }

        /// <summary>
        /// Returns "n" characters to the left of a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The Lenght.</param>
        /// <returns>
        /// The left part of string
        /// </returns>
        /// <example>
        /// var name = "DIEGO MARTINEZ";
        /// var onlyFirstsThreeLetters = name.Left(3);
        /// //The value of onlyFirstsThreeLetters is "DIE";
        /// </example>
        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return value;
            length = Math.Abs(length);

            return (value.Length <= length ? value : value.Substring(0, length));
        }

        /// <summary>
        /// Align a value to the right or left, depending on whether they are only numbers.
        /// </summary>
        /// <remarks>
        /// used to store numerical and alphanumeric data and then be able to sort them alphanumeric. 
        /// ex: "0001", "0500", "AR", "EN".
        /// </remarks>
        /// <param name="value">The value</param>
        /// <param name="length">The length</param>
        /// <returns>
        /// String aligned value.
        /// </returns>
        /// <example>
        /// <code>
        /// var codProduct = "0001";
        /// var alignedCode = codProduct.AlignCodeValue(11);
        /// // the value of alignedCode is "       0001"
        /// 
        /// var country = "AR";
        /// var alignedCountryCpde = country.AlignCodeValue(4);
        /// // the value of alignedCountryCpde is "AR  "
        /// 
        /// </code>
        /// </example>
        public static string AlignCodeValue(this string value, int length)
        {
            string result = value;

            if (result.Length > length)
                result = result.Substring(0, length);

            Regex reg = new Regex("^\\d+$");
            if (reg.IsMatch(result))
            {
                result = result.PadLeft(length);
            }
            else
            {
                result = result.PadRight(length);
            }
            return result;
        }
    }
}
