using System;
using System.Text.RegularExpressions;


namespace Stub.Help
{
    class RegEx
    {
        public static bool Checker(string buf, string regexStr)
        {
            var date = buf.Trim();
            var regex = new Regex(regexStr);
            if (!regex.IsMatch(date)) return false;
            return true;
        }
    }
}
