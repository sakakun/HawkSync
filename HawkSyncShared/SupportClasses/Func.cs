using System;
using System.Collections.Generic;
using System.Text;

namespace HawkSyncShared.SupportClasses
{
    public static class Func
    {
        private const string Base64Marker = "__B64__";

        public static string TB64(string input)
        {
            if (input == null) return string.Empty;
            var bytes = Encoding.GetEncoding(1252).GetBytes(input);
            return Base64Marker + Convert.ToBase64String(bytes);
        }

        public static string FB64(string input)
        {
            if (input == null) return string.Empty;
            if (input.StartsWith(Base64Marker))
            {
                var base64Part = input.Substring(Base64Marker.Length);
                var bytes = Convert.FromBase64String(base64Part);
                return Encoding.GetEncoding(1252).GetString(bytes);
            }
            // Not marked as Base64, return as-is
            return input;
        }

        public static bool IsMarkedBase64(string input)
        {
            return input != null && input.StartsWith(Base64Marker);
        }
    }
}
