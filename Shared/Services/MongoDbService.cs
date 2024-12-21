using MongoDB.Driver;
using Shared.Services.Abstractions;


namespace Shared.Services
{
    public class MongoDbService : IMongoDbService
    {
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            IMongoDatabase database = GetDatabase();
            return database.GetCollection<T>(collectionName);
        }

        public IMongoDatabase GetDatabase(string databaseName = "ProductDB", string conneectionString = "mongodb://localhost:27017")
        {
            MongoClient mongoClient = new(conneectionString);
            return mongoClient.GetDatabase(databaseName);
        }
    }
}
