namespace Geekhub.Backend.Domain.Adapters;

public interface IRedisAdapter
{
    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T?>> factory, TimeSpan? expiration = null);
}
