using MongoDB.Driver;
using Pinboard.DataPersistence.Repositories;
using Pinboard.Domain.Interfaces;
using Pinboard.Domain.Interfaces.Repositories;

namespace Pinboard.DataPersistence
{
    public class DataContext : IDataContext
    {
        public DataContext(IMongoClient mongoClient)
        {
            NoteRepository = new NoteRepository(mongoClient, "Notes");
        }

        public INoteRepository NoteRepository { get; }
    }
}
