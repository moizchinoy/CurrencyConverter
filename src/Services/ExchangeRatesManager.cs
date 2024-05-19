using Infrastructure;

namespace Services
{
    public interface IExchangeRatesManager
    {
        Task<ExchangeRates> GetRates(string currency);

        Task<ExchangeRates> Convert(string currency, decimal amount);

        Task<IEnumerable<ExchangeRates>> GetHistoricalRates(string currency, DateOnly fromDate, DateOnly toDate);
    }

    public class ExchangeRatesManager : IExchangeRatesManager
    {
        private readonly IFrankfurterApi _frankfurterApi;

        private readonly Dictionary<string, SortedDictionary<DateOnly, ExchangeRates>> _dict = [];

        public ExchangeRatesManager(IFrankfurterApi frankfurterApi)
        {
            _frankfurterApi = frankfurterApi;
        }

        private void UpdateDict(ExchangeRates exchangeRates)
        {
            if (!_dict.TryGetValue(exchangeRates.BaseCurrency, out SortedDictionary<DateOnly, ExchangeRates> value))
            {
                _dict.Add(exchangeRates.BaseCurrency, new SortedDictionary<DateOnly, ExchangeRates>
                {
                    { exchangeRates.Date, exchangeRates }
                });
            }
            else if (!value.ContainsKey(exchangeRates.Date))
            {
                value.Add(exchangeRates.Date, exchangeRates);
            }
        }

        public async Task<ExchangeRates> GetRates(string currency)
        {
            var apiResponse = await _frankfurterApi.GetLatestRates(currency);

            var exchangeRates = new ExchangeRates(
                apiResponse.Amount,
                apiResponse.Base,
                apiResponse.Date,
                apiResponse.Rates.GetDictionary());

            UpdateDict(exchangeRates);

            return exchangeRates;
        }

        public async Task<ExchangeRates> Convert(string currency, decimal amount)
        {
            var latestRates = await GetRates(currency);

            return latestRates.Convert(amount);
        }

        public Task<IEnumerable<ExchangeRates>> GetHistoricalRates(string currency, DateOnly fromDate, DateOnly toDate)
        {
            var exchangeRatesList = new List<ExchangeRates>();

            var currencyHistory = _dict[currency];

            while (fromDate <= toDate)
            {
                var exchangeRates = currencyHistory[fromDate];
                exchangeRatesList.Add(exchangeRates);
                fromDate = fromDate.AddDays(1);
            }

            return Task.FromResult<IEnumerable<ExchangeRates>>(exchangeRatesList);
        }
    }
}
