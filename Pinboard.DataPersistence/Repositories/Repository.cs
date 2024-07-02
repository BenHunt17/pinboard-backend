using MongoDB.Driver;
using Pinboard.DataPersistence.Models;

namespace Pinboard.DataPersistence.Repositories
{
    public abstract class Repository<TDomainModel, TDbModel>
        where TDbModel : IDatabaseModel<TDomainModel>
    {
        private readonly IMongoClient _client;

        protected Repository(IMongoClient mongoClient, string collectionName)
        {
            _client = mongoClient;
            Collection = _client.GetDatabase("Pinboard")
                .GetCollection<TDbModel>(collectionName);
        }

        protected IMongoCollection<TDbModel> Collection { get; }

        protected FilterDefinitionBuilder<TDbModel> FilterBuilder = Builders<TDbModel>.Filter;

        protected SortDefinitionBuilder<TDbModel> SortBuilder = Builders<TDbModel>.Sort;

        protected UpdateDefinitionBuilder<TDbModel> UpdateBuilder = Builders<TDbModel>.Update;
    }
}
