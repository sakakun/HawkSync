using System.Globalization;
using HawkSyncShared.SupportClasses;

namespace ServerManager.Classes.SupportClasses
{
    public static class ServerSettings
    {
        private static readonly Dictionary<string, object> _cache = new();
        private static readonly object _lock = new();

        public static T Get<T>(string key, T defaultValue)
        {
            lock (_lock)
            {
                if (_cache.TryGetValue(key, out var cached))
                    return (T)cached;

                string raw = DatabaseManager.GetSetting(
                    key,
                    ConvertToString(defaultValue)
                );

                T value = ConvertFromString(raw, defaultValue);

                _cache[key] = value!;
                return value;
            }
        }

        public static void Set<T>(string key, T value)
        {
            lock (_lock)
            {
                _cache[key] = value!;
                DatabaseManager.SetSetting(key, ConvertToString(value));
            }
        }

        // ---------- Conversion helpers ----------

        private static string ConvertToString<T>(T value)
        {
            return value switch
            {
                bool b    => b.ToString().ToLowerInvariant(),
                int i     => i.ToString(CultureInfo.InvariantCulture),
                long l    => l.ToString(CultureInfo.InvariantCulture),
                decimal d => d.ToString(CultureInfo.InvariantCulture),
                string s  => s,
                _ => throw new NotSupportedException(
                    $"Type {typeof(T)} is not supported"
                )
            };
        }


        private static T ConvertFromString<T>(string value, T defaultValue)
        {
            try
            {
                if (typeof(T) == typeof(bool) &&
                    bool.TryParse(value, out var b))
                    return (T)(object)b;

                if (typeof(T) == typeof(int) &&
                    int.TryParse(value, out var i))
                    return (T)(object)i;

                if (typeof(T) == typeof(long) &&
                    long.TryParse(value, out var l))
                    return (T)(object)l;

                if (typeof(T) == typeof(decimal) &&
                    decimal.TryParse(value, out var d))
                    return (T)(object)d;

                if (typeof(T) == typeof(string))
                    return (T)(object)value;

            }
            catch (Exception ex)
            {
                AppDebug.Log($"Error converting setting.", AppDebug.LogLevel.Error, ex);
            }

            return defaultValue;
        }
    }

}
