using System.Security.Cryptography;

namespace Geekhub.Backend.Domain.Models;

public class Password
{
    /// <summary>
    /// Valor do hash da senha do usuário
    /// </summary>
    public string Hash { get; set; } = string.Empty;

    private Password() { }

    public Password(string hash)
    {
        Hash = HashPassword(hash);
    }

    private static string HashPassword(string password)
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

    private bool Verify(string password)
    {
        var parts = Hash.Split(':');

        if (parts.Length != 2)
        {
            throw new FormatException("Invalid hashed password format.");
        }

        byte[] salt = Convert.FromHexString(parts[0]);
        byte[] hash = Convert.FromHexString(parts[1]);
        byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            35000,
            HashAlgorithmName.SHA512,
            32
        );
        return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
    }
}
