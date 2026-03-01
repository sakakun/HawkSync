using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using HawkSyncShared.SupportClasses;

namespace BHD_ServerManager.Classes.Helpers
{
    /// <summary>
    /// Helper class to fetch and cache country flag images by ISO code
    /// </summary>
    public static class FlagHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly ConcurrentDictionary<string, Image?> _flagCache = new();
        private const string FlagCdnUrl = "https://flagcdn.com/w40/{0}.png";

        static FlagHelper()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        /// <summary>
        /// Get flag image by ISO 3166-1 alpha-2 country code (async)
        /// </summary>
        /// <param name="countryCode">Two-letter ISO country code (e.g., "US", "GB", "JP")</param>
        /// <returns>Flag image or null if not found</returns>
        public static async Task<Image?> GetFlagAsync(string? countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
                return null;

            var normalizedCode = countryCode.ToLowerInvariant();

            // Check cache first - return a clone so each caller owns an independent copy
            if (_flagCache.TryGetValue(normalizedCode, out var cachedFlag))
                return cachedFlag != null ? new Bitmap(cachedFlag) : null;

            try
            {
                var url = string.Format(FlagCdnUrl, normalizedCode);
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    Bitmap flag;
                    using (var ms = new MemoryStream(imageBytes))
                    using (var tempImage = Image.FromStream(ms))
                    {
                        // Create an independent Bitmap copy not tied to the MemoryStream
                        flag = new Bitmap(tempImage);
                    }

                    // Cache the independent copy
                    _flagCache[normalizedCode] = flag;

                    // Return a clone so each caller owns their own copy
                    return new Bitmap(flag);
                }
                else
                {
                    AppDebug.Log("FlagHelper", $"Failed to fetch flag for {countryCode}: {response.StatusCode}");
                    _flagCache[normalizedCode] = null;
                    return null;
                }
            }
            catch (Exception ex)
            {
                AppDebug.Log("FlagHelper", $"Error fetching flag for {countryCode}: {ex.Message}");
                _flagCache[normalizedCode] = null;
                return null;
            }
        }

        /// <summary>
        /// Clear the flag cache
        /// </summary>
        public static void ClearCache()
        {
            foreach (var flag in _flagCache.Values)
            {
                flag?.Dispose();
            }
            _flagCache.Clear();
        }

        /// <summary>
        /// Get cached flag count
        /// </summary>
        public static int CachedFlagCount => _flagCache.Count;
    }
}
