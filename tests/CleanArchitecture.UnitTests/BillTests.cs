using CleanArchitecture.Core;
using CleanArchitecture.Tests.Shared.Builder;
using System;
using Xunit;

namespace CleanArchitecture.UnitTests
{
    public class BillTests
    {
        [Fact]
        public void HasCreated_ShouldReturnTrue_IfUserHasCreatedBill()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "John");
            var account = user.CreateAccount("Checking account");
            var bill = new BillBuilder().CreatedByUser(user).WithAccount(account).Build();

            // Act && Assert
            Assert.True(bill.HasCreated(user.Id));
        }

        [Fact]
        public void ShareWithUser_ShouldAddUserToBills_Correctly()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "John");
            var anotherUser = new User(Guid.NewGuid(), "Phil");
            var account = user.CreateAccount("Checking account");
            var bill = new BillBuilder().CreatedByUser(user).WithAccount(account).Build();

            // Act
            bill.ShareWithUser(anotherUser);

            // Assert
            Assert.Collection(bill.SharedWithUsers, b =>
            {
                Assert.Equal(anotherUser, b.User);
            });
        }
    }
}
