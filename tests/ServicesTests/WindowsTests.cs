using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests
{
    public class WindowsTests
    {
        [Theory]
        [InlineData("2020-01-01", "2020-01-01", "2020-01-01", "2020-01-05")]
        [InlineData("2020-01-05", "2020-01-05", "2020-01-01", "2020-01-05")]
        [InlineData("2020-01-31", "2020-01-31", "2020-01-26", "2020-01-31")]
        [InlineData("2024-02-29", "2024-02-29", "2024-02-26", "2024-02-29")]
        // TODO: Add more cases
        public void Verify_SingleDate(string strStartDate, string strEndDate, 
            string strExpectedStartDate, string strExpectedEndDate)
        {
            // Arrange
            var startDate = DateOnly.Parse(strStartDate);
            var endDate = DateOnly.Parse(strEndDate);

            // Act
            var result = Windows.GetWindows(startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(DateOnly.Parse(strExpectedStartDate), result.ToList()[0].FromDate);
            Assert.Equal(DateOnly.Parse(strExpectedEndDate), result.ToList()[0].ToDate);
        }
    }
}
