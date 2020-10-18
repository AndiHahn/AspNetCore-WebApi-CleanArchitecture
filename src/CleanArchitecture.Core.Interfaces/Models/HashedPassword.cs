using System;
using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Models
{
    public class HashedPassword : IEquatable<HashedPassword>
    {
        public string Salt { get; private set; }
        public string Hash { get; private set; }

        public void WithPlainPasswordAndSaltSize(string plainPassword, int saltSize)
        {
            Salt = CreateSalt(saltSize);
            Hash = GenerateHash(plainPassword, Salt);
        }

        public void WithPlainPasswordAndSalt(string plainPassword, string salt)
        {
            Salt = salt;
            Hash = GenerateHash(plainPassword, salt);
        }

        public void WithHashAndSalt(string hash, string salt)
        {
            Salt = salt;
            Hash = hash;
        }

        public bool Equals(HashedPassword other)
        {
            return Hash == other?.Hash && Salt == other?.Salt;
        }

        private string CreateSalt(int saltSize)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[saltSize];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        private string GenerateHash(string plainPassword, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainPassword + salt);
            SHA256Managed sHa256ManagedString = new SHA256Managed();
            byte[] hash = sHa256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
