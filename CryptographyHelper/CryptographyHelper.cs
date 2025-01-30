using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyHelper
{
    public static class CryptographyHelper
    {
        private const int SaltByteSizes = 16; // 128 bits for a strong salt
        private const int Pbkdf2Iterations = 10000; // Increased iterations for stronger key derivation
        private const int Pbkdf2SubkeyLength = 32; // 256 bits for AES key

        public static string Encrypt(string clearText, string password)
        {
            byte[] salt = GenerateSaltBytes();
            byte[] iv = GenerateIv(); // Generate a unique IV for each encryption
            byte[] cipherBytes;

            using (Aes aes = Aes.Create())
            using (var derivedKey = new Rfc2898DeriveBytes(password, salt, Pbkdf2Iterations, HashAlgorithmName.SHA256)) // SHA256!
            {
                aes.Key = derivedKey.GetBytes(Pbkdf2SubkeyLength);
                aes.IV = iv;

                using MemoryStream ms = new();
                using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
                byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.FlushFinalBlock();

                cipherBytes = ms.ToArray();
            }

            // Combine salt, IV, and ciphertext for storage/transmission
            byte[] combined = new byte[salt.Length + iv.Length + cipherBytes.Length];
            Array.Copy(salt, 0, combined, 0, salt.Length);
            Array.Copy(iv, 0, combined, salt.Length, iv.Length);
            Array.Copy(cipherBytes, 0, combined, salt.Length + iv.Length, cipherBytes.Length);

            return Convert.ToBase64String(combined);
        }

        public static string Decrypt(string cipherText, string password)
        {
            byte[] combined = Convert.FromBase64String(cipherText);

            byte[] salt = new byte[SaltByteSizes];
            byte[] iv = new byte[Aes.Create().BlockSize / 8]; // Get IV size from AES
            byte[] cipherBytes = new byte[combined.Length - salt.Length - iv.Length];

            Array.Copy(combined, 0, salt, 0, salt.Length);
            Array.Copy(combined, salt.Length, iv, 0, iv.Length);
            Array.Copy(combined, salt.Length + iv.Length, cipherBytes, 0, cipherBytes.Length);


            using Aes aes = Aes.Create();
            using var derivedKey = new Rfc2898DeriveBytes(password, salt, Pbkdf2Iterations, HashAlgorithmName.SHA256); // SHA256!
            aes.Key = derivedKey.GetBytes(Pbkdf2SubkeyLength);
            aes.IV = iv;

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltByteSizes];
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);  // Return salt as Base64 string
        }
        private static byte[] GenerateSaltBytes()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltByteSizes];
            rng.GetBytes(salt);
            return salt;
        }

        private static byte[] GenerateIv()
        {
            using Aes aes = Aes.Create();
            aes.GenerateIV();
            return aes.IV;
        }

        public static string ComputeSha256Hash(string text)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            byte[] hashBytes = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}