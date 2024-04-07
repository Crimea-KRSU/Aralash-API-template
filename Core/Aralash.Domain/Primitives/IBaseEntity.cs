namespace Aralash.Domain.Primitives;

public interface IBaseEntity;

public interface IBaseEntity<TKey> : IBaseEntity
{
    public TKey Id { get; set; }
}