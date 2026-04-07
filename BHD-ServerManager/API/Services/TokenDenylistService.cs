using System.Collections.Concurrent;

namespace ServerManager.API.Services;

/// <summary>
/// Thread-safe in-memory denylist for revoked JWT tokens.
/// Tokens are identified by their jti (JWT ID) claim and are automatically
/// eligible for purge once their expiry time has passed.
/// </summary>
public static class TokenDenylistService
{
    // jti → UTC expiry of the original token (used for cleanup)
    private static readonly ConcurrentDictionary<string, DateTime> _denylist = new();

    /// <summary>
    /// Revokes a token by its jti. The utcExpiry is used to allow safe purging
    /// of entries that are already past their natural expiry.
    /// </summary>
    public static void Revoke(string jti, DateTime utcExpiry)
    {
        _denylist[jti] = utcExpiry;
    }

    /// <summary>
    /// Returns true if the given jti has been explicitly revoked.
    /// </summary>
    public static bool IsRevoked(string jti)
    {
        return _denylist.ContainsKey(jti);
    }

    /// <summary>
    /// Removes all denylist entries whose tokens have already naturally expired.
    /// Call this on a periodic basis (e.g., from the SessionCleanup ticker).
    /// </summary>
    public static void PurgeExpired()
    {
        var now = DateTime.UtcNow;
        foreach (var kvp in _denylist.Where(e => e.Value <= now).ToList())
        {
            _denylist.TryRemove(kvp.Key, out _);
        }
    }

    /// <summary>
    /// Current number of active denylist entries (for diagnostics).
    /// </summary>
    public static int Count => _denylist.Count;
}

