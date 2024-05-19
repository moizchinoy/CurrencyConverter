using Infrastructure;

namespace Services
{
    public class HistoricalData(IFrankfurterApi api)
    {
        public async Task<HistoricalExchangeRates> GetHistoricalRatesAsync(
            Currency currency, DateOnly fromDate, DateOnly toDate, int page, int size, 
            CancellationToken cancellationToken)
        {
            var pageDetails = Pager.GetPageDetails(fromDate, toDate, page, size);

            var windows = Windows.GetWindows(pageDetails.PageFromDate, pageDetails.PageToDate);

            var dateRatesDict = new Dictionary<DateOnly, IEnumerable<Rate>>();

            foreach (var (windowFromDate, windowToDate) in windows)
            {
                var windowData = await api.GetHistoricalRatesAsync(
                        currency.ToString(), windowFromDate, windowToDate, cancellationToken)
                    .ConfigureAwait(false);

                foreach (var item in windowData.Rates)
                {
                    var rates = item.Value.Select(x => new Rate(new Currency(x.Key), x.Value));
                    dateRatesDict.Add(item.Key, rates);
                }
            }

            return new HistoricalExchangeRates
            {
                Amount = 1,
                BaseCurrency = currency,
                FromDate = fromDate,
                ToDate = toDate,
                Rates = dateRatesDict,
                PreviousPage = pageDetails.PreviousPage,
                NextPage = pageDetails.NextPage,
            };
        }
    }
}
