using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using StocksPortfolio.Infrastructure.Data.Model;

namespace StocksPortfolio.Infrastructure.Services
{
    public class DataProviderService
    {
        private readonly IMongoCollection<Portfolio> _portfolioCollection;
        private static readonly MongoDbRunner _runner = MongoDbRunner.Start();

        static DataProviderService()
        {
            _runner.Import("portfolioDb", "Portfolios", Path.Combine("Data", "portfolios.json"), true);
        }

        public DataProviderService()
        {
            var client = new MongoClient(_runner.ConnectionString);
            _portfolioCollection = client.GetDatabase("portfolioDb").GetCollection<Portfolio>("Portfolios");
        }

        public async Task<Portfolio> GetPortfolio(ObjectId id)
        {
            var idFilter = Builders<Portfolio>.Filter.Eq(portfolio => portfolio.Id, id);

            return await _portfolioCollection.Find(idFilter).FirstOrDefaultAsync();
        }

        public async Task DeletePortfolio(ObjectId id)
        {
            // I decided to add a propertion DeleteOnUtc, whose default value is NULL and which, after deleting a given form, changes to the current time, which additionally informs us when a given form was deleted. An alternative would be to add a flag such as IsDeleted, which would set the value to true after deletion.
            var filter = Builders<Portfolio>.Filter.Eq(portfolio => portfolio.Id, id);
            var update = Builders<Portfolio>.Update.Set(portofilio => portofilio.DeletedOnUtc, DateTime.UtcNow);

            await _portfolioCollection.UpdateOneAsync(filter, update);
        }
    }
}
