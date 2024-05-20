namespace Services
{
    public interface IConverter
    {
        Task<Result<LatestRates>> GetRatesAsync(Currency currency, CancellationToken cancellationToken);

        Task<Result<LatestRates>> ConvertAsync(Currency currency, decimal amount, CancellationToken cancellationToken);

        Task<Result<HistoricalRates>> GetHistoricalRatesAsync(
            Currency currency, DateOnly fromDate, DateOnly toDate, int page, int size, CancellationToken cancellationToken);
    }
}
