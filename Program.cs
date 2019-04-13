using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Redis.Hello.Models;

namespace Redis.Hello
{
    class Program
    {
        static IConfigurationRoot configuration;
        static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            Console.WriteLine("> starting...");

            Startup();

            var redis = serviceProvider.GetRequiredService<IRedisService>();

            var data = new Person()
            {
                Age = 20,
                Id = 1,
                Name = "Joe Soap",
                Sex = Gender.Male
            };

            redis.Push(data);

            Console.WriteLine($"> redis tx: {data}");

            var result = redis.Pull();

            Console.WriteLine($"redis rx: {result}");

            Console.ReadLine();

            serviceProvider = null;
            configuration = null;

            Console.WriteLine("> stopped.");
        }

        private static void Startup()
        {
            configuration = new ConfigurationBuilder().AddJsonFile("settings.json", true, true).Build();

            var redisConfiguration = new Configuration.RedisConfiguration();
            configuration.Bind("redis", redisConfiguration);

            serviceProvider = new ServiceCollection()
                .AddOptions()
                .Configure<Configuration.RedisConfiguration>((x) => configuration.GetSection("redis"))
                .AddDistributedRedisCache(options =>
                {
                    options.InstanceName = redisConfiguration.InstanceName;
                    options.Configuration = redisConfiguration.Host;
                })
                .AddTransient<IRedisService, RedisService>()
                .BuildServiceProvider();
        }
    }
}
