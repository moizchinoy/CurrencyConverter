﻿using Infrastructure;
using Services.Extensions;
using Services.Helpers;
using Services.Models;

namespace Services
{
    public class HistoricalData(IFrankfurterApi api)
    {
        public async Task<HistoricalRates> GetRatesAsync(
            Currency currency, DateOnly fromDate, DateOnly toDate, int page, int size,
            CancellationToken cancellationToken)
        {
            var pageDetails = Pager.GetPageDetails(fromDate, toDate, page, size);

            var windows = Windows.GetWindows(pageDetails.PageFromDate, pageDetails.PageToDate);

            var dateRatesDict = new Dictionary<DateOnly, IEnumerable<CurrencyRate>>();

            foreach (var (windowFromDate, windowToDate) in windows)
            {
                var windowData = await api.GetHistoricalRatesAsync(
                        currency, windowFromDate, windowToDate, cancellationToken)
                    .ConfigureAwait(false);

                foreach (var item in windowData.Rates)
                {
                    dateRatesDict.Add(item.Key, item.Value.ToRates());
                }
            }

            return new HistoricalRates
            {
                Amount = 1,
                BaseCurrency = currency,
                FromDate = fromDate,
                ToDate = toDate,
                DailyRates = dateRatesDict,
                PreviousPage = pageDetails.PreviousPage,
                NextPage = pageDetails.NextPage,
            };
        }
    }
}
