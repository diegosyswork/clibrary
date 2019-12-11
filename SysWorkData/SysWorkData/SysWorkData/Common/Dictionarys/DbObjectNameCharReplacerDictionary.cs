using System.Collections.Generic;

namespace SysWork.Data.Common.Dictionarys
{
    /// <summary>
    /// Dictionary to replace special characters in database object names.
    /// </summary>
    public static class DbObjectNameCharReplacerDictionary
    {
        /// <summary>
        /// Character equivalence.
        /// </summary>
        public static readonly Dictionary<string, string> CharacterEquivalence = new Dictionary<string, string>()
        {
            { " ",  "____01____"},
            { "-",  "____02____"},
            { ".",  "____03____"},
            { "/",  "____04____"},
            { @"\", "____05____"}
        };

    }
}
