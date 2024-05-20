using Moq;
using Services;

namespace ServicesTests
{
    public class FilteredConverterTests
    {
        [Theory]
        [InlineData("TRY", false, true)]
        [InlineData("EUR", true, false)]
        public async Task Verify_Convert_Input(string currency, bool isSuccess, bool isResultNull)
        {
            // Arrange
            var restrictedCurrencies = new List<Currency>
            {
                new("TRY"),
                new("PLN"),
                new("THB"),
                new("MXN"),
            };

            var apiResult = Result<LatestRates>.GetSuccess(
                new LatestRates
                {
                    Amount = 1,
                    BaseCurrency = new Currency(currency),
                    Date = DateOnly.MaxValue,
                    Rates = []
                });

            var mockConverter = new Mock<IConverter>();
            mockConverter
                .Setup(x => x.ConvertAsync(
                    It.IsAny<Currency>(),
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResult);

            var sut = new FilteredConverter(mockConverter.Object, restrictedCurrencies);

            // Act
            var result = await sut.ConvertAsync(new Currency(currency), 1, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(isSuccess, result.IsSuccess);
            Assert.Equal(isResultNull, result.Value is null);
        }

        [Theory]
        [InlineData("TRY", false)]
        [InlineData("EUR", true)]
        public async Task Verify_Convert_Output(string currency, bool isAllowed)
        {
            // Arrange
            var restrictedCurrencies = new List<Currency> { new("TRY"), new("PLN"), new("THB"), new("MXN"), };

            var baseCurrency = new Currency("USD");

            var apiResult = Result<LatestRates>.GetSuccess(new LatestRates
            {
                Amount = 1,
                BaseCurrency = baseCurrency,
                Date = DateOnly.MaxValue,
                Rates = [
                    new CurrencyRate
                    {
                        Currency = new Currency(currency),
                        Value = 1
                    }
                ]
            });

            var mockConverter = new Mock<IConverter>();
            mockConverter
                .Setup(x => x.ConvertAsync(
                    It.IsAny<Currency>(),
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResult);

            var sut = new FilteredConverter(mockConverter.Object, restrictedCurrencies);

            // Act
            var result = await sut.ConvertAsync(baseCurrency, 1, CancellationToken.None);

            // Assert
            Assert.Equal(isAllowed, result.Value.Rates.Any(x => x.Currency == new Currency(currency)));
        }
    }
}