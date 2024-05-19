namespace Services
{
    public class FilteredExchangeRatesManager(IExchangeRatesManager exchangeRatesManager) : IExchangeRatesManager
    {
        private readonly List<string> restrictedCurrencies = new List<string>();

        public async Task<ExchangeRates> GetRates(string currency)
        {
            return await exchangeRatesManager.GetRates(currency);
        }

        public async Task<ExchangeRates> Convert(string currency, decimal amount)
        {
            if (restrictedCurrencies.Contains(currency))
            {
                return null;
            }

            var response = await exchangeRatesManager.Convert(currency, amount);

            foreach(var restrictedCurrency in restrictedCurrencies)
            {
                response.Rates.Remove(restrictedCurrency);
            }

            return response;
        }

        public async Task<IEnumerable<ExchangeRates>> GetHistoricalRates(string currency, DateOnly fromDate, DateOnly toDate)
        {
            return await exchangeRatesManager.GetHistoricalRates(currency, fromDate, toDate);
        }
    }
}
