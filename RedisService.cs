using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Redis.Hello.Configuration;
using Redis.Hello.Models;

namespace Redis.Hello
{
    public class RedisService : IRedisService
    {
        public readonly IDistributedCache Cache;

        public readonly RedisConfiguration Configuration;

        public RedisService(IDistributedCache cache, IOptions<RedisConfiguration> config)
        {
            Cache = cache;
            Configuration = config.Value;
        }

        public Person Pull() => JsonConvert.DeserializeObject<Person>(Cache.GetString(Configuration.RedisKey));

        public void Push(Person data) => Cache.SetString(Configuration.RedisKey, JsonConvert.SerializeObject(data));
    }
}
