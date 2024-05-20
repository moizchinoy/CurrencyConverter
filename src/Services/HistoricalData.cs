using Infrastructure;

namespace Services
{
    public class HistoricalData(IFrankfurterApi api)
    {
        public async Task<HistoricalExchangeRates> GetRatesAsync(
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
                    dateRatesDict.Add(item.Key, item.Value.ToRates());
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
