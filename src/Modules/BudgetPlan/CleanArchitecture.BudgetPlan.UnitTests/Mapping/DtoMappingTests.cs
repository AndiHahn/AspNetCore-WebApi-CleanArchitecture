using AutoMapper;
using CleanArchitecture.BudgetPlan.Application.FixedCost;
using CleanArchitecture.BudgetPlan.Application.Income;
using CleanArchitecture.BudgetPlan.Core;
using System;
using CleanArchitecture.BudgetPlan.Application.Mapping;
using Xunit;
using FluentAssertions;

namespace CleanArchitecture.BudgetPlan.UnitTests.Mapping
{
    public class DtoMappingTests
    {
        private readonly IMapper mapper;

        public DtoMappingTests()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            this.mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Map_ShouldMapIncomeEntityToDto_Correctly()
        {
            // Arrange
            var entity = new Income(Guid.NewGuid(), "name", 11.3, Duration.HalfYear);

            // Act
            var result = this.mapper.Map<IncomeDto>(entity);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<IncomeDto>();
        }

        [Fact]
        public void Map_ShouldMapFixedCostEntityToDto_Correctly()
        {
            // Arrange
            var entity = new FixedCost(Guid.NewGuid(), "name", 11.3, Duration.HalfYear, CostCategory.Saving);

            // Act
            var result = this.mapper.Map<FixedCostDto>(entity);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<FixedCostDto>();
        }
    }
}
