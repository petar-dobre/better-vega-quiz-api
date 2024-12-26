using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace QuizWebApp.Services;

public class PasswordHasher
{
    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));

        return hashedPassword;
    }

    public bool VerifyHashedPassword(string hashedPasswordWithSalt, string password)
    {
        var parts =  hashedPasswordWithSalt.Split(":");
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid hashed password format.");
        }

        byte[] salt = Convert.FromBase64String(parts[0]);

        string storedHash = parts[1];

        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));

        return hashedPassword == storedHash;
    }
}