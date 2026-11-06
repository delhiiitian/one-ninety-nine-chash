using LibrarySystem.Domain;

namespace LibrarySystem.Infrastructure;

/// <summary>Storage contract. Services depend on this, not on a concrete class.</summary>
public interface IRepository<T> where T : IEntity
{
    void Add(T entity);
    T? Find(string id);
    IEnumerable<T> All();
}

/// <summary>A generic in-memory store. One class serves items, members and anything else with an Id.</summary>
public sealed class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly Dictionary<string, T> _byId = new(StringComparer.OrdinalIgnoreCase);

    public void Add(T entity)
    {
        if (!_byId.TryAdd(entity.Id, entity))
            throw new InvalidOperationException($"{typeof(T).Name} with id '{entity.Id}' already exists.");
    }

    public T? Find(string id) => _byId.GetValueOrDefault(id);

    public IEnumerable<T> All() => _byId.Values;
}
