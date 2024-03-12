using StocksPortfolio.Infrastructure.Services;

namespace StockPortoflioTest.CurrencyServiceTests
{
    public class ExchangeRateTest
    {
        private readonly CurrencyService _currencyService;

        public ExchangeRateTest()
        {
            _currencyService = new CurrencyService();
        }

        [Fact]
        public void GetExchangeRate_RetursOneForSameCurrency()
        {
            // Arrange
            var sourceCurrency = "USD";
            var targetCurrency = "USD";
            var quotes = new Dictionary<string, decimal>();

            // Act
            var exchangeRate = _currencyService.GetExchangeRate(sourceCurrency, targetCurrency, quotes);

            // Assert
            Assert.Equal(1m, exchangeRate);
        }

        [Fact]
        public void GetExchangeRate_ReturnsCorrectRate()
        {
            // Arrange
            var sourceCurrency = "EUR";
            var targetCurrency = "GBP";
            var quotes = new Dictionary<string, decimal>
            {
                { "USDEUR", 0.82m },
                { "USDGBP", 0.72m }
            };

            // Act
            var exchangeRate = _currencyService.GetExchangeRate(sourceCurrency, targetCurrency, quotes);

            // Assert
            Assert.Equal(0.87804878048m, exchangeRate, 10);
        }

    }
}
