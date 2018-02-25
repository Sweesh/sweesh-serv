using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Sweesh.Core.Managers
{
    using Abstract.Managers;

    public class HashManager : IHashManager
    {
        private static Random Rnd = new Random();

        public string BasicHash(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data.ToLower());
            using (var md5 = MD5.Create())
            using (var sha256 = SHA256.Create())
            {
                var hash = md5.ComputeHash(sha256.ComputeHash(bytes));
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        private string DrowssapHash(string data, string salt)
        {
            var drowssap = new string(data.Reverse().ToArray());
            return BasicHash(drowssap + salt);
        }

        public string PasswordHash(string data, string salt)
        {
            
            return BCrypt.Net.BCrypt.HashPassword(DrowssapHash(data, salt));
        }

        public string GenerateSalt(int length)
        {
            string output = "";
            for (var i = 0; i < length; i++)
            {
                output += Convert.ToChar((byte)Rnd.Next(25, 256)).ToString();
            }
            return output;
        }

        public bool VerifyHash(string data, string salt, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(DrowssapHash(data, salt), hash);
        }
    }
}
