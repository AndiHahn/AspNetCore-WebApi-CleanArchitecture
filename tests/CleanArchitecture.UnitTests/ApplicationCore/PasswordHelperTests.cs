using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Helper;
using Xunit;

namespace CleanArchitecture.UnitTests.ApplicationCore
{
    public class PasswordHelperTests
    {
        [Fact]
        public void CreateSalt_ShouldCreateSalt_Correctly()
        {
            string salt = PasswordHelper.CreateSalt(5);
            Assert.False(string.IsNullOrEmpty(salt));
        }

        [Fact]
        public void GenerateHash_ShouldCreateHash_Correctly()
        {
            string salt = "Salt";
            string password = "PasswordToBeHashed";
            string hash = PasswordHelper.GenerateHash(password, salt);
            Assert.False(string.IsNullOrEmpty(hash));
        }

        [Fact]
        public void ComparePasswordWithHash_ShouldReturnTrue_IfEqual()
        {
            string salt = PasswordHelper.CreateSalt(5);
            string password = "PasswordToBeHashed";
            string hash = PasswordHelper.GenerateHash(password, salt);
            Assert.True(PasswordHelper.IsEqual(password, hash, salt));
        }

        [Fact]
        public void ComparePasswordWithHash_ShouldReturnFalse_IfSaltWrong()
        {
            string salt = PasswordHelper.CreateSalt(5);
            string password = "PasswordToBeHashed";
            string hash = PasswordHelper.GenerateHash(password, salt);
            Assert.False(PasswordHelper.IsEqual(password, hash, "AnySalt"));
        }

        [Fact]
        public void ComparePasswordWithHash_ShouldReturnFalse_IfHashWrong()
        {
            string salt = PasswordHelper.CreateSalt(5);
            string password = "PasswordToBeHashed";
            Assert.False(PasswordHelper.IsEqual(password, "AnyHash", salt));
        }
    }
}
