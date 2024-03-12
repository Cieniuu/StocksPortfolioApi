using StocksPortfolio.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace StocksPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly DataProviderService _dataService;
        private readonly ICurrencyService _currencyService;

        public PortfolioController()
        {
            _dataService = new DataProviderService();
            _currencyService = new CurrencyService();
        }

        [HttpGet("{id}")]
        public IActionResult GetPortfolio(string id)
        {
            var portfolio = _dataService.GetPortfolio(ObjectId.Parse(id)).Result;
            return Ok(portfolio);
        }

        [HttpGet("/value")]
        public async Task<IActionResult> GetTotalPortfolioValue(string portfolioId, string currency = "USD")
        {
            var portfolio = _dataService.GetPortfolio(ObjectId.Parse(portfolioId)).Result;
            var totalAmount = 0m;

            var quotes = await _currencyService.GetCurrencyRates();

            foreach (var stock in portfolio.Stocks)
            {
                var stockService = new StocksService.StocksService();
                var stockPrice = await stockService.GetStockPrice(stock.Ticker);
                var exchangeRate = _currencyService.GetExchangeRate(stock.Currency, currency, quotes);
                totalAmount += stockPrice.Price * stock.NumberOfShares * exchangeRate;
            }

            portfolio.CurrentTotalValue = (float)totalAmount;

            return Ok(totalAmount);
        }

        [HttpGet("/delete")]
        public async Task<IActionResult> DeletePortfolio(string portfolioId)
        {
            await _dataService.DeletePortfolio(ObjectId.Parse(portfolioId));

            return Ok();
        }
    }
}
