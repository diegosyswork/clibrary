using System.Text.RegularExpressions;

namespace SysWork.Forms.Utilities
{
    public static class DataEntryUtil
    {
        public static bool IsValidEmail(string mail)
        {
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Match match = Regex.Match(mail, pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }
        public static bool IsValidUrl(string url)
        {
            string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }
        public static bool IsNumeric(string text)
        {
            string pattern = @"[0-9]{1,9}(\.[0-9]{0,2})?$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(text);
        }

    }
}
