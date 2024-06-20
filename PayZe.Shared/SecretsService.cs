using System.Security.Cryptography;

namespace PayZe.Shared;

public static class SecurityService
{
    public static string GenerateApiSecret()
    {
        var secretKey = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(secretKey);
        return Convert.ToBase64String(secretKey);
    }
    public static string GenerateApiKey(string companyName)
    {
        byte[] secretKey = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(secretKey);
        }
        return string.Concat(companyName, Guid.NewGuid());
    }
    public static (string Secret, string Salt) GenerateHash(string secret)
    {
        var salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(secret, salt, 10000, HashAlgorithmName.SHA256);

        var hash = pbkdf2.GetBytes(32);
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));

    }

    public static bool CheckIfSecretHashEquals(string secret, string hashedSecret, string storedSalt)
    {
        var salt = Convert.FromBase64String(storedSalt);

        using var pbkdf2 = new Rfc2898DeriveBytes(secret, salt, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash) == hashedSecret;
    }
}