using System.Security.Cryptography;

namespace BHD_ServerManager.API.Services;

/// <summary>
/// Provides a cryptographically secure JWT signing key that is generated once at application startup
/// and shared between the API host and controllers for the lifetime of the application.
/// </summary>
public static class JwtKeyProvider
{
    private static readonly Lazy<string> _jwtKey = new(() => GenerateSecureKey());

    /// <summary>
    /// Gets the JWT signing key. Generated once on first access and cached for the application lifetime.
    /// </summary>
    public static string JwtKey => _jwtKey.Value;

    /// <summary>
    /// Generates a cryptographically secure random key for JWT signing.
    /// </summary>
    private static string GenerateSecureKey()
    {
        var keyBytes = new byte[64]; // 512 bits for strong security
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }
        return Convert.ToBase64String(keyBytes);
    }
}
