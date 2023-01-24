using bCrypt = BCrypt.Net.BCrypt;

namespace MainAPI.Services
{
    public class EncryptionService
    {
        public static string Encrypt(string plainPassword)
        {
            var salt = bCrypt.GenerateSalt();
            return bCrypt.HashPassword(plainPassword, salt);
        }

        public static bool Validate(string plainPassword, string encryptedPassword)
        {
            return bCrypt.Verify(plainPassword, encryptedPassword);
        }
    }
}
