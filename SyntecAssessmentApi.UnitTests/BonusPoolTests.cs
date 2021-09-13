using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Services;
using System;
using Xunit;

namespace SynetecAssessmentApi.Tests
{
    public class BonusPoolTests
    {

        [Theory]
        [InlineData(10000, 0.1, 1000)]
        [InlineData(67891, 0.0234, 1588)]
        public void GetBonusAllocation(int bonusPoolAmount, decimal bonusPercentage, decimal expectedBonusAllocation)
        {
            var bonusAllocation = BonusPoolService.GetBonusAllocation(bonusPoolAmount, bonusPercentage);

            Assert.Equal(expectedBonusAllocation, bonusAllocation);
        }

        [Theory]
        [InlineData(34567, 987654, 0.035)]
        [InlineData(67891, 12345678, 0.005)]
        [InlineData(35000, 621987, 0.056)]
        public void GetBonusPercentage(decimal employeeSalary, decimal salariesTotal, decimal expectedBonusPercentage)
        {
            var bonusPercentage = BonusPoolService.GetBonusPercentage(employeeSalary, salariesTotal);

            Assert.Equal(expectedBonusPercentage, bonusPercentage);
        }

        [Theory]
        [InlineData(765467, 34567, 987654, 26791)]
        [InlineData(876543, 67891, 12345678, 4382)]
        [InlineData(675235, 35000, 621987, 37813)]
        public void GetBonusAmount(int bonusPoolAmount, decimal employeeSalary, decimal salariesTotal, decimal expectedBonus)
        {
            var bonusPercentage = BonusPoolService.GetBonusPercentage(employeeSalary, salariesTotal);
            var bonusAllocation = BonusPoolService.GetBonusAllocation(bonusPoolAmount, bonusPercentage);

            Assert.Equal(expectedBonus, bonusAllocation);
        }

        [Fact]
        public async void CheckInvalidGetBonusAsyncInputsAsync()
        {
            BonusPoolService _bonusPoolService = new BonusPoolService();

            await Assert.ThrowsAsync<ExpectedException>(() => _bonusPoolService.GetBonusAsync(0, 0));
        }
    }
}
