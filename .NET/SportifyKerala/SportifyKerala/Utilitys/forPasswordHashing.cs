using System.Security.Cryptography;
using System.Text;

namespace SportifyKerala.Utilitys
{
    public class forPasswordHashing
    {
        private const int KeySize = 64;
        private const int Iterations = 350000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

        /// <summary>
        /// Hashes the password and generates a salt.
        /// </summary>
        public static string HashPassword(string password, out string salt)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(KeySize);
            salt = Convert.ToBase64String(saltBytes);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                Iterations,
                HashAlgorithm,
                KeySize);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Verifies the password against the stored hash and salt.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                Iterations,
                HashAlgorithm,
                KeySize);

            return Convert.ToBase64String(hash) == storedHash;
        }
    }

}
