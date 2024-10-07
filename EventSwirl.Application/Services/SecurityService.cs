using EventSwirl.Application.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EventSwirl.Application.Services
{
    public class SecurityService: ISecurityService
    {
        private string _key = "q1w1e3r4";

        private int _saltSize = 16;

        public async Task<(string, string)> Encoder(string password, string str_salt)
        {
            return await Task.Run(() => DoEncoding(password, str_salt));
        }

        public async Task<(string, string)> Encoder(string password)
        {
            byte[] salt = new byte[_saltSize];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);

            return await Encoder(password, Encoding.Unicode.GetString(salt));
        }

        private (string, string) DoEncoding(string password, string str_salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] salt = Encoding.Unicode.GetBytes(str_salt);

            byte[] passwordWithSalt =
                new byte[passwordBytes.Length + salt.Length];

            for (int i = 0; i < passwordBytes.Length; i++)
                passwordWithSalt[i] = passwordBytes[i];

            for (int i = 0; i < salt.Length; i++)
                passwordWithSalt[passwordBytes.Length + i] = salt[i];

            HMACSHA384 hmac = new(Encoding.Unicode.GetBytes(_key));

            byte[] hashValue = hmac.ComputeHash(passwordWithSalt);

            return (Encoding.Unicode.GetString(salt), Encoding.Unicode.GetString(hashValue));
        }
    }
}
