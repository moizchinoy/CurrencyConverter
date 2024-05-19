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
        public async Task<ActionResult<ExchangeRates>> Get(string currency)
        {
            var result = await _exchangeRatesManager.GetRates(currency);
        
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpGet("convert")]
        public async Task<ActionResult<ExchangeRates>> Convert(string currency, decimal amount)
        {
            var result = await _exchangeRatesManager.Convert(currency, amount);

            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<ExchangeRates>>> GetHistoricalRates(string currency, DateOnly fromDate, DateOnly toDate)
        {
            var result = await _exchangeRatesManager.GetHistoricalRates(currency, fromDate, toDate);
            
            return (result != null) ? Ok(result) : NotFound();
        }
    }
}
