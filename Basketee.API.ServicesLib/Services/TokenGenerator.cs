using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Basketee.API.Services
{
    public class TokenGenerator
    {
        private const string ALGORITHM = "HmacSHA256";
        private const string SALT = "yz7LWQtFBXpMj9W8fvUh";
        private const string CHARS = "abcdefghiABCDEFGHIjklmnopqrstuvwxyzJKLMNOPQRSTUVWXYZ";
        private const string NUMS = "1234567890";
        private const string RESET_PASSWORD_DEFAULT = "1111";
        private const int RESET_PASSWORD_LENGTH = 4;

        public static string GenerateToken(string username, string password, string phoneNumber)
        {
            string text = string.Join("#$#", new string[] { username, phoneNumber, DateTime.Now.AddMinutes(1.1234).ToLongDateString() });
            string token = "";
            using (HMAC hmac = HMACSHA256.Create(ALGORITHM))
            {
                hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(password));
                hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
                token = Convert.ToBase64String(hmac.Hash);
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
        }

        public static string GetHashedPassword(string password, int maxPasswordength = -1)
        {
            string key = string.Join(":|:", new string[] { password, SALT });
            string hashPassword = null;
            using (HMAC hmac = HMACSHA256.Create(ALGORITHM))
            {
                hmac.Key = Encoding.UTF8.GetBytes(ALGORITHM);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashPassword = Convert.ToBase64String(hmac.Hash);
            }
            if (maxPasswordength > 0 && hashPassword.Length > maxPasswordength)
            {
                hashPassword = hashPassword.Substring(0, maxPasswordength);
            }
            return hashPassword;
        }

        public static string GenerateResetPassword()
        {
            string password = string.Empty;
            password = RESET_PASSWORD_DEFAULT;
            //Random rnd = new Random();
            //while(password.Length < RESET_PASSWORD_LENGTH)
            //{
            //    password += CHARS[rnd.Next(CHARS.Length)];
            //    password += NUMS[rnd.Next(NUMS.Length)];
            //}
            return password;
        }
    }
}