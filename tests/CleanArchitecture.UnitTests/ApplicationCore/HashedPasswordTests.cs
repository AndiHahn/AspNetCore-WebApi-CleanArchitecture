using CleanArchitecture.Core.Models.Common;
using Xunit;

namespace CleanArchitecture.UnitTests.ApplicationCore
{
    public class HashedPasswordTests
    {
        [Fact]
        public void CreateSalt_ShouldCreateSalt_Correctly()
        {
            var hashedPassword = new HashedPassword();
            hashedPassword.WithPlainPasswordAndSaltSize("password", 5);

            Assert.True(!string.IsNullOrEmpty(hashedPassword.Salt));
        }

        [Fact]
        public void GenerateHash_ShouldCreateHash_Correctly()
        {
            var hashedPassword = new HashedPassword();
            hashedPassword.WithPlainPasswordAndSalt("password", "salt");

            Assert.True(!string.IsNullOrEmpty(hashedPassword.Hash));
        }

        [Fact]
        public void CompareHashedPassword_ShouldReturnTrue_IfEqual()
        {
            string salt = "salt";
            string password = "password";
            var hashedPassword1 = new HashedPassword();
            hashedPassword1.WithPlainPasswordAndSalt(password, salt);
            var hashedPassword2 = new HashedPassword();
            hashedPassword2.WithPlainPasswordAndSalt(password, salt);

            Assert.True(hashedPassword1.Equals(hashedPassword2));
        }

        [Fact]
        public void CompareHashedPassword_ShouldReturnFalse_IfSaltWrong()
        {
            string hash = "hash";
            var hashedPassword1 = new HashedPassword();
            hashedPassword1.WithHashAndSalt(hash, "salt1");
            var hashedPassword2 = new HashedPassword();
            hashedPassword2.WithHashAndSalt(hash, "salt2");

            Assert.False(hashedPassword1.Equals(hashedPassword2));
        }

        [Fact]
        public void CompareHashedPassword_ShouldReturnFalse_IfHashWrong()
        {
            string salt = "salt";
            var hashedPassword1 = new HashedPassword();
            hashedPassword1.WithHashAndSalt("hash1", salt);
            var hashedPassword2 = new HashedPassword();
            hashedPassword2.WithHashAndSalt("hash2", salt);

            Assert.False(hashedPassword1.Equals(hashedPassword2));
        }
    }
}
