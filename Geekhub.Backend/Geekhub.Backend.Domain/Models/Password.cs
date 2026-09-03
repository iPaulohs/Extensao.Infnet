using System.Security.Cryptography;

namespace Geekhub.Backend.Domain.Models;

public class Password
{
    public string Hash { get; private set; } = string.Empty;

    private Password() { }

    public Password(string hash)
    {
        var parts = hash.Split(':');

        if (parts.Length != 2)
            throw new FormatException("Formato inválido para a senha hasheada.");

        Hash = hash;
    }

    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            35000,
            HashAlgorithmName.SHA512,
            32
        );

        return string.Join(":", Convert.ToHexString(salt), Convert.ToHexString(hash));
    }

    public bool Verify(string plainTextPassword)
    {
        return Verify(Hash, plainTextPassword);
    }

    public static bool Verify(string storedHash, string plainTextPassword)
    {
        var parts = storedHash.Split(':');

        if (parts.Length != 2)
            throw new FormatException("Formato inválido para a senha hasheada.");

        byte[] salt = Convert.FromHexString(parts[0]);
        byte[] expectedHash = Convert.FromHexString(parts[1]);

        byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            plainTextPassword,
            salt,
            35000,
            HashAlgorithmName.SHA512,
            32
        );

        return CryptographicOperations.FixedTimeEquals(expectedHash, hashToCompare);
    }
}