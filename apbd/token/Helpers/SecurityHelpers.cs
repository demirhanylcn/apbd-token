using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace token.Helpers;

public class SecurityHelpers
{
    public static (string hashedPassword, string salt) GetHashedPasswordAndSalt(string password)
    {
        var salt = new byte[128 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        var hashedPassword = HashPassword(password, salt);
        var saltBase64 = Convert.ToBase64String(salt);

        return new ValueTuple<string, string>(hashedPassword, saltBase64);
    }

    public static string GetHashedPasswordWithSalt(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);

        var currentHashedPassword = HashPassword(password, saltBytes);

        return currentHashedPassword;
    }

    private static string HashPassword(string password, byte[] saltBytes)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password,
            saltBytes,
            KeyDerivationPrf.HMACSHA1,
            10000,
            256 / 8));
    }
}