using Microsoft.AspNetCore.Mvc;
using Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly IExchangeRatesManager _exchangeRatesManager;

        public CurrencyController(IExchangeRatesManager currencyConverter)
        {
            _exchangeRatesManager = currencyConverter;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<Response>> Get(string currency, CancellationToken cancellationToken)
        {
            var result = await _exchangeRatesManager.GetRatesAsync(new Currency(currency), cancellationToken);
            return (result.IsSuccess) ? Ok(new Response(result.Value)) : BadRequest(result.Error);
        }

        [HttpGet("convert")]
        public async Task<ActionResult<Response>> Convert(string currency, decimal amount, CancellationToken cancellationToken)
        {
            var result = await _exchangeRatesManager.ConvertAsync(new Currency(currency), amount, cancellationToken);
            return (result.IsSuccess) ? Ok(new Response(result.Value)) : BadRequest(result.Error);
        }

        [HttpGet("history")]
        public async Task<ActionResult<HistoricalExchangeRates>> GetHistoricalRates(
            string currency, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken, 
            int size = 10, int page = 1)
        {
            var result = await _exchangeRatesManager.GetHistoricalRatesAsync(
                new Currency(currency), fromDate, toDate, page, size, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(new HistoricalResponse(result.Value));
            }
            else
            {
                return BadRequest(result.Error);
            }
        }
    }
}
