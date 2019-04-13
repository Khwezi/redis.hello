using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Redis.Hello.Models;

namespace Redis.Hello
{
    public class RedisService : IRedisService
    {
        public IDistributedCache Cache { get; set; }

        public Configuration.RedisConfiguration Configuration { get; set; }

        public RedisService(IDistributedCache cache, IConfigurationRoot configuration)
        {
            Cache = cache;
            configuration.Bind("redis", Configuration);
        }

        public Person Pull() => JsonConvert.DeserializeObject<Person>(Cache.GetString(Configuration.RedisKey));

        public void Push(Person data) => Cache.SetString(Configuration.RedisKey, JsonConvert.SerializeObject(data));
    }
}
