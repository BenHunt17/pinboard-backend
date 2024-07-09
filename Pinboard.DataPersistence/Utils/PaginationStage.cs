using MongoDB.Driver;
using Pinboard.DataPersistence.Models;
using Pinboard.Domain.Model;

namespace Pinboard.DataPersistence.Utils
{
    public static class PaginationStage
    {
        public static PaginatedItems<TDbModel> Paginate<TDbModel>(this IAggregateFluent<TDbModel> aggregation, string? cursor, int limit)
            where TDbModel : IDatabaseModel
        {
            var sortDefinition = Builders<TDbModel>.Sort.Descending(x => x.Id);

            var filterDefinition = string.IsNullOrEmpty(cursor) ?
                Builders<TDbModel>.Filter.Empty :
                Builders<TDbModel>.Filter.Lte(x => x.Id, cursor);

            var items = aggregation
                .Sort(sortDefinition)
                .Match(filterDefinition)
                .Limit(limit + 1)
                .ToEnumerable();

            string? nextCursor = null;
            if (items.Count() == limit + 1)
            {
                nextCursor = items.Last().Id;
            }

            return new PaginatedItems<TDbModel>
            {
                Items = items.SkipLast(1),
                NextCursor = nextCursor,
            };
        }
    }
}
