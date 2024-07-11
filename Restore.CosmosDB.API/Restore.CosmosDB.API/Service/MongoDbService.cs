using Microsoft.Extensions.Options;
using MongoDB.Driver;



namespace Restore.CosmosDB.API.Service
{
    public class MongoDbService
    {
        private readonly IMongoCollection<ObservationsModel> _collection;

        public MongoDbService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<ObservationsModel>(settings.Value.CollectionName);
        }

        public async Task InsertRecordsAsync(IEnumerable<ObservationsModel> records)
        {
            InsertManyOptions options = new InsertManyOptions();
            //options.BypassDocumentValidation = true;
            options.IsOrdered = false;
            await _collection.InsertManyAsync(records, options);
        }
    }
}