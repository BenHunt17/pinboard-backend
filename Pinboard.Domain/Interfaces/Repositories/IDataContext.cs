namespace Pinboard.Domain.Interfaces.Repositories
{
    public interface IDataContext
    {
        INoteRepository NoteRepository { get; }
    }
}
