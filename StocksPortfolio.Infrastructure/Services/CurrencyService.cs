using StocksPortfolio.Infrastructure.Model;
using System.Text.Json;

namespace StocksPortfolio.Infrastructure.Services
{
    public interface ICurrencyService
    {
        Task<Dictionary<string, decimal>> GetCurrencyRates();
        decimal GetExchangeRate(string sourceCurrency, string targetCurrency, Dictionary<string, decimal> quotes);
    }

    public class CurrencyService : ICurrencyService
    {
        private readonly string _apiAccessKey;
        private Dictionary<string, decimal> _cachedCurrencyRates;
        private DateTime _lastFetchTime;

        public CurrencyService()
        {
            _apiAccessKey = "78c057e28b2abf54f48110356bb9d1ce";
            _cachedCurrencyRates = new Dictionary<string, decimal>();
            _lastFetchTime = DateTime.MinValue;
        }

        public async Task<Dictionary<string, decimal>> GetCurrencyRates()
        {
            var timeSinceLastRefresh = DateTime.Now - _lastFetchTime;

            if (timeSinceLastRefresh.TotalHours >= 24 || _cachedCurrencyRates.Count == 0)
            {
                await RefreshCurrencyRates();
            }

            return _cachedCurrencyRates;
        }

        private async Task RefreshCurrencyRates()
        {
            using var httpClient = new HttpClient { BaseAddress = new Uri("http://api.currencylayer.com/") };
            try
            {
                var response = await httpClient.GetAsync($"live?access_key={_apiAccessKey}");
                response.EnsureSuccessStatusCode();
                //line 43 and 44 served to see the full transmitted string before deserialisation. This told me that the subscription had reached its limit and therefore data was not being passed despite a successful status of 200
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseContent);
                var data = await JsonSerializer.DeserializeAsync<Quote>(response.Content.ReadAsStream());
                _cachedCurrencyRates = data.Quotes;
                _lastFetchTime = DateTime.Now;
            } 
            catch (HttpRequestException e)
            {
                throw new ApplicationException($"Error fetching currency rates: {e.Message}", e);
            }
        }

        public decimal GetExchangeRate(string sourceCurrency, string targetCurrency, Dictionary<string, decimal> quotes)
        {
            if (sourceCurrency == targetCurrency)
                return 1m;

            var rateSourceToUSD = sourceCurrency == "USD" ? 1m : quotes["USD" + sourceCurrency];
            var rateUSDToTarget = targetCurrency == "USD" ? 1m : quotes["USD" + targetCurrency];

            return rateUSDToTarget / rateSourceToUSD;
        }
    }
}
