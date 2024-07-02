using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Pinboard.Domain.Model;

namespace Pinboard.DataPersistence.Models
{
    public class NoteModel : IDatabaseModel<Note>
    {
        public NoteModel(Note note)
        {
            Title = note.Title;
            Content = note.Content;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement]
        public string Title { get; set; }

        [BsonElement]
        public string Content { get; set; }

        public Note ToDomainModel()
        {
            return new Note
            {
                Id = Id,
                Title = Title,
                Content = Content
            };
        }
    }
}
