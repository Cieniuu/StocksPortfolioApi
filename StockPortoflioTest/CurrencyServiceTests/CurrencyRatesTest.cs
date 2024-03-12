using StocksPortfolio.Infrastructure.Services;
using Xunit;

namespace StockPortoflioTest.CurrencyServiceTests
{
    public class CurrencyRatesTest
    {
        private readonly CurrencyService _currencyService;

        public CurrencyRatesTest() 
        {
            _currencyService = new CurrencyService();
        }

        [Fact]
        public void GetExchangeRate_ReturnOneForSameCurrency()
        {
            // Act
            var exchangeRate = _currencyService.GetExchangeRate("USD", "USD", []);

            // Assert
            Assert.Equal(1m, exchangeRate);
        }

        [Fact]
        public void GetExchangeRate_ReturnsCorrectRateAfterExchange()
        {
            // Arrange
            var quotes = new Dictionary<string, decimal>
            {
                { "USDEUR", 0.85m },
                { "USDGBP", 0.75m }
            };

            // Act
            var exchangeRateEURtoUSD = _currencyService.GetExchangeRate("EUR", "USD", quotes);
            var exchangeRateUSDtoGBP = _currencyService.GetExchangeRate("USD", "GBP", quotes);
            var exchangeRateEURtoGBP = _currencyService.GetExchangeRate("EUR", "GBP", quotes);

            // Assert
            Assert.Equal(1.17647m, exchangeRateEURtoUSD, 5);
            Assert.Equal(0.75m, exchangeRateUSDtoGBP);
            Assert.Equal(0.88235m, exchangeRateEURtoGBP, 5);
        }
    }
}