namespace StockPortoflioTest.StocksServiceTests
{
    public class StockPriceTest
    {
        private static readonly string[] collection = ["PLN", "EUR", "USD", "JPY", "GBP"];

        [Fact]
        public async Task GetStockPrice_ReturnsValidStockModel()
        {
            // Arrange
            var service = new StocksService.StocksService();

            // Act
            var stock = await service.GetStockPrice("AAPL");

            // Assert
            Assert.NotNull(stock);
            Assert.InRange(stock.Price, 10, 1000);
            Assert.Contains(stock.Currency, collection);
        }
    }
}
