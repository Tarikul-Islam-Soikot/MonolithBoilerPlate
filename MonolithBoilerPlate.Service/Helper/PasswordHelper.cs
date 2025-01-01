using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace MonolithBoilerPlate.Service.Helper
{
    public static class PasswordHelper
    {
        public static string GenerateRandomPassword(string passwordChars, int length = 12)
        {
            if (length < 8)
            {
                throw new ArgumentException("Password length should be at least 8 characters.");
            }

            var byteBuffer = new byte[length];
            RandomNumberGenerator.Fill(byteBuffer);

            var charBuffer = new char[length];
            for (int i = 0; i < length; i++)
            {
                charBuffer[i] = passwordChars[byteBuffer[i] % passwordChars.Length];
            }

            return new string(charBuffer);
        }

        public static string HashPassword(string encryptionKey, string password)
        {

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(encryptionKey)))
            {
                var hashedBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyPassword(string encryptionKey, string enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(encryptionKey, enteredPassword);
            return enteredHash == storedHash;
        }
    }

}



