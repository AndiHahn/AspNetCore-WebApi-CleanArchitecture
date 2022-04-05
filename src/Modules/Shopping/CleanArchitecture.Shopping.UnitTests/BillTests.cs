using System;
using CleanArchitecture.Shopping.Core;
using CleanArchitecture.Shopping.UnitTests.Builder;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Shopping.UnitTests
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

            // Act
            bool result = bill.HasCreated(user.Id);

            // Assert
            result.Should().BeTrue();
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
            bill.SharedWithUsers.Should().Contain(userBill => userBill.User == anotherUser)
                .And.HaveCount(1);
        }
    }
}
