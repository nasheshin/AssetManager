using System;
using System.Text.RegularExpressions;

namespace AssetManagerServer.Utils
{
    public static class PropertiesValidator
    {
        public static bool UsernameValidate(string username)
        {
            const string pattern = "^[a-z0-9_]{3,16}$";
            return username != null && Regex.IsMatch(username, pattern, RegexOptions.IgnoreCase);
        }
        
        public static bool PasswordValidate(string password)
        {
            const string pattern = "^[a-z0-9]{3,16}$";
            return password != null && Regex.IsMatch(password, pattern, RegexOptions.IgnoreCase);
        }
        
        public static bool AssetTickerValidate(string assetTicker)
        {
            const string pattern = "^[A-Z0-9]{1,50}$";
            return assetTicker != null && Regex.IsMatch(assetTicker, pattern);
        }

        public static bool DatetimeValidate(DateTime datetime)
        {
            return (datetime < DateTime.Now);
        }
    }
}