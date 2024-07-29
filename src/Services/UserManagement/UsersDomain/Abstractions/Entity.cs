using static UsersDomain.Abstractions.IEntity;

namespace UsersDomain.Abstractions
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }        
    }
}
