using Services.Models;

namespace ServicesTests.Models
{
    public class CurrencyTests
    {
        [Theory]
        [InlineData("EUR", "EUR", true)]
        [InlineData("EUR", "eur", true)]
        [InlineData("EUR", "USD", false)]
        public void Currency_CheckObjectEquality(string a, string b, bool expectedResult)
        {
            // Arrange

            // Act
            var result = new Currency(a) == new Currency(b);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("EUR", "EUR", true)]
        [InlineData("EUR", "eur", true)]
        [InlineData("EUR", "USD", false)]
        public void Currency_CheckEquals(string a, string b, bool expectedResult)
        {
            // Arrange

            // Act
            var result = new Currency(a).Equals(new Currency(b));

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}