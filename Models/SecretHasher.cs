using System.Security.Cryptography;
using System.Text;

namespace garagebackend.Models
{
    public class SecretHasher
    {
        public static string HashSecret(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[32];
                rng.GetBytes(salt);

                using (var hmac = new HMACSHA256(salt))
                {
                    {
                        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
                    }
                }
            }
        }


        public static bool VerifySecret(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;
            var salt = Convert.FromBase64String(parts[0]);
            var storedPasswordHash = Convert.FromBase64String(parts[1]);

            using (var hmac = new HMACSHA256(salt))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash) == Convert.ToBase64String(storedPasswordHash);
            }

        }
    }
}
