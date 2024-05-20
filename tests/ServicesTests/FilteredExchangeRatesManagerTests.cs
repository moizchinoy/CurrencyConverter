using Moq;
using Services;

namespace ServicesTests
{
    public class FilteredExchangeRatesManagerTests
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

            var apiResult = Result<ExchangeRates>.GetSuccess(
                new ExchangeRates
                {
                    Amount = 1,
                    BaseCurrency = new Currency(currency),
                    Date = DateOnly.MaxValue,
                    Rates = []
                });

            var mockExchangeRatesManager = new Mock<IExchangeRatesManager>();
            mockExchangeRatesManager
                .Setup(x => x.ConvertAsync(
                    It.IsAny<Currency>(),
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResult);

            var sut = new FilteredExchangeRatesManager(mockExchangeRatesManager.Object, restrictedCurrencies);

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

            var apiResult = Result<ExchangeRates>.GetSuccess(new ExchangeRates
            {
                Amount = 1,
                BaseCurrency = baseCurrency,
                Date = DateOnly.MaxValue,
                Rates = [
                    new Rate
                    {
                        Currency = new Currency(currency),
                        Value = 1
                    }
                ]
            });

            var mockExchangeRatesManager = new Mock<IExchangeRatesManager>();
            mockExchangeRatesManager
                .Setup(x => x.ConvertAsync(
                    It.IsAny<Currency>(),
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResult);

            var sut = new FilteredExchangeRatesManager(mockExchangeRatesManager.Object, restrictedCurrencies);

            // Act
            var result = await sut.ConvertAsync(baseCurrency, 1, CancellationToken.None);

            // Assert
            Assert.Equal(isAllowed, result.Value.Rates.Any(x => x.Currency == new Currency(currency)));
        }
    }
}