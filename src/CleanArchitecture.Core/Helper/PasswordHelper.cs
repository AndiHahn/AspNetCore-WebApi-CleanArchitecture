using System;
using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.Core.Helper
{
    public static class PasswordHelper
    {
        public static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHa256ManagedString = new SHA256Managed();
            byte[] hash = sHa256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool IsEqual(string plainTextInput, string hashedInput, string salt)
        {
            string newHashedPin = GenerateHash(plainTextInput, salt);
            return newHashedPin.Equals(hashedInput);
        }
    }
}