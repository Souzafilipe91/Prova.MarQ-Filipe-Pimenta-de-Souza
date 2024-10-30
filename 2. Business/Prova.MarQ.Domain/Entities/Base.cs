namespace Prova.MarQ.Domain.Entities
{
    public abstract class Base
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
