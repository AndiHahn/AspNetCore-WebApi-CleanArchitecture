using CleanArchitecture.Application.Validations;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using Xunit;

namespace CleanArchitecture.UnitTests.ApplicationCore
{
    public class NotFoundValidationTests
    {
        [Fact]
        public void EnsureFound_ShouldThrowException_IfEntityIsNull()
        {
            BillEntity billEntity = null;
            Assert.Throws<NotFoundException>(() => billEntity.AssertEntityFound());
        }

        [Fact]
        public void EnsureFound_ShouldNotThrowException_IfEntityIsNotNull()
        {
            BillEntity billEntity = new BillEntity();
            billEntity.AssertEntityFound();
            Assert.True(true);
        }

        [Fact]
        public void EnsureFound_ShouldContainIdInMessage_IfEntityIsNull()
        {
            int testId = 2;
            BillEntity billEntity = null;
            try
            {
                billEntity.AssertEntityFound(testId);
            }
            catch (NotFoundException ex)
            {
                ex.Message.Contains(testId.ToString());
            }
        }
    }
}
