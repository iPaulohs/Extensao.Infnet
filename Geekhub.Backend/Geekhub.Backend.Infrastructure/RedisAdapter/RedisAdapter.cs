using Geekhub.Backend.Domain.Adapters;
using StackExchange.Redis;
using System.Text.Json;

namespace Geekhub.Backend.Infrastructure.RedisAdapter;

public class RedisAdapter(IConnectionMultiplexer connection) : IRedisAdapter
{
    private IDatabase Database => connection.GetDatabase();

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T?>> factory, TimeSpan? expiration = null)
    {
        var value = await Database.StringGetAsync(key);

        if (value.HasValue)
        {
            return JsonSerializer.Deserialize<T>(value.ToString());
        }

        var factoryResult = await factory();

        if (factoryResult != null)
        {
            await Database.StringSetAsync(key, JsonSerializer.Serialize(factoryResult), expiration, When.NotExists);
        }

        return factoryResult;
    }
}
