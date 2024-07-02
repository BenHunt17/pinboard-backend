namespace Pinboard.DataPersistence.Models
{
    public interface IDatabaseModel<TDomainModel>
    {
        public string Id { get; set; }

        public TDomainModel ToDomainModel();
    }
}
