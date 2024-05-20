using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly IConverter _converter;

        public CurrencyController(IConverter converter)
        {
            _converter = converter;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<Response>> Get(string currency, CancellationToken cancellationToken)
        {
            var result = await _converter.GetRatesAsync(new Currency(currency), cancellationToken);
            return (result.IsSuccess) ? Ok(new Response(result.Value)) : BadRequest(result.Error);
        }

        [HttpGet("convert")]
        public async Task<ActionResult<Response>> Convert(string currency, decimal amount, CancellationToken cancellationToken)
        {
            var result = await _converter.ConvertAsync(new Currency(currency), amount, cancellationToken);
            return (result.IsSuccess) ? Ok(new Response(result.Value)) : BadRequest(result.Error);
        }

        [HttpGet("history")]
        public async Task<ActionResult<HistoricalRates>> GetHistoricalRates(
            string currency, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken, 
            int size = 10, int page = 1)
        {
            var result = await _converter.GetHistoricalRatesAsync(
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
