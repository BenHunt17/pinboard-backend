using MongoDB.Driver;
using MongoDB.Driver.Search;
using Pinboard.DataPersistence.Models;
using Pinboard.DataPersistence.Utils;
using Pinboard.Domain.Inputs;
using Pinboard.Domain.Interfaces.Repositories;
using Pinboard.Domain.Model;

namespace Pinboard.DataPersistence.Repositories
{
    public class NoteRepository : Repository<NoteModel>, INoteRepository
    {
        private const string SearchIndexName = "NotesSearchIndex";

        public NoteRepository(IMongoClient mongoClient, string collectionName)
            : base(mongoClient, collectionName)
        {
        }

        public Note Get(string id)
        {
            var note = Collection.Find(x => x.Id == id).SingleOrDefault();

            if (note == null)
            {
                throw new KeyNotFoundException($"Note with Id {id} does not exist");
            }

            return note.ToDomainModel();
        }

        public IEnumerable<Note> GetAll()
        {
            var notes = Collection.Find(FilterBuilder.Empty)
                .ToEnumerable();

            return notes.Select(x => x.ToDomainModel()).ToList();
        }

        public IEnumerable<Note> GetByIds(IEnumerable<string> ids)
        {
            var filterDefinition = FilterBuilder.In(x => x.Id, ids);

            var notes = Collection.Find(filterDefinition)
                .ToEnumerable();

            return notes.Select(x => x.ToDomainModel()).ToList();
        }

        public PaginatedItems<Note> Search(NoteSearchInput input, string authorIdFilter)
        {
            var searchDefinition = SearchBuilder.Compound();

            var aggregation = Collection.Aggregate();

            if (!string.IsNullOrEmpty(input.SearchTerm)) 
            {
                searchDefinition
                    .Should(SearchBuilder.Autocomplete(x => x.Title, input.SearchTerm))
                    .Should(SearchBuilder.Text(x => x.Content, input.SearchTerm))
                    .MinimumShouldMatch(1);

                aggregation = aggregation.Search(
                    searchDefinition,
                    null,
                    SearchIndexName);
            }

            var filterDefinition = FilterBuilder.Eq(x => x.AuthorId, authorIdFilter);

            var result = aggregation
                .Match(filterDefinition)
                .Paginate(input.Cursor, input.Limit);

            return new PaginatedItems<Note>
            {
                Items = result.Items.Select(x => x.ToDomainModel()),
                NextCursor = result.NextCursor,
            };
        }

        public Note Create(Note note)
        {
            //TODO - catch mongo duplictate exception and throw the duplicate key exception to make error response more user friendly 
            var noteModel = new NoteModel(note);

            Collection.InsertOne(noteModel);

            return noteModel.ToDomainModel();
        }

        public Note UpdateTitle(string id, string title)
        {
            var updateDefinition = UpdateBuilder
                .Set(x => x.Title, title);

            var note = UpdateNote(id, updateDefinition);

            return note.ToDomainModel();
        }

        public Note UpdateContent(string id, string content)
        {
            var updateDefinition = UpdateBuilder
                .Set(x => x.Content, content);

            var note = UpdateNote(id, updateDefinition);

            return note.ToDomainModel();
        }

        public void DeleteByIds(IEnumerable<string> ids)
        {
            var filterDefinition = FilterBuilder.In(x => x.Id, ids);

            Collection.DeleteMany(filterDefinition);
        }

        public void DeleteByAuthorId(string authorId)
        {
            var filterDefinition = FilterBuilder.Eq(x => authorId, authorId);

            Collection.DeleteMany(filterDefinition);
        }

        private NoteModel UpdateNote(string id, UpdateDefinition<NoteModel> updateDefinition)
        {
            var filterDefintion = FilterBuilder.Eq(x => x.Id, id);

            var note = Collection.FindOneAndUpdate(
                filterDefintion,
                updateDefinition,
                new FindOneAndUpdateOptions<NoteModel>
                {
                    ReturnDocument = ReturnDocument.After
                });

            if (note == null)
            {
                throw new KeyNotFoundException($"Note with Id {id} does not exist");
            }

            return note;
        }
    }
}
