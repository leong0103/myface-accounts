using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MyFace.Helpers.PasswordHelper
{
    public static class PasswordHelper
    {   

        public static (string userName, string password) GetUserDetails(string authorization)
        {
            string encodedData = Encoding.UTF8.GetString(Convert.FromBase64String(authorization.Substring("Base ".Length)));

            string[] userNamePassword = encodedData.Split(":");
            string userName = userNamePassword[0];
            string password = userNamePassword[1];
            
            return (userName, password);

        }
        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            return salt;
        }
        public static string GetHash(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2
                (
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                ));

            return hashed;
        }
    }
}