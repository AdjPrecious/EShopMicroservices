namespace Ordering.Domain.Abstractions
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public String? LastModifiedBy { get; set; }
    }
}
