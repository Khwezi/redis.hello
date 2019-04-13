using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration.FileExtensions;
using System;
using Redis.Hello.Models;
using System.IO;
using Redis.Hello.Configuration;

namespace Redis.Hello
{
    class Program
    {
        static IConfigurationRoot configuration;
        static IServiceCollection services;
        static IServiceProvider servicesProvider;

        static void Main(string[] args)
        {
            Console.WriteLine("> starting...");

            Startup();

            Console.WriteLine("> online.");

            var redis = servicesProvider.GetService<IRedisService>();

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

            servicesProvider = null;
            configuration = null;

            Console.WriteLine("> stopped.");
        }

        private static void Startup()
        {
            configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddEnvironmentVariables()
               .AddJsonFile("settings.json", false, true)
               .Build();

            services = new ServiceCollection()
                .AddOptions()
                .AddLogging(l => l.AddConsole())
                .Configure<RedisConfiguration>(configuration.GetSection(nameof(RedisConfiguration)))
                .AddDistributedRedisCache(c =>
                {
                    c.InstanceName = configuration.GetSection(nameof(RedisConfiguration)).Get<RedisConfiguration>().InstanceName;
                    c.Configuration = configuration.GetSection(nameof(RedisConfiguration)).Get<RedisConfiguration>().Host;
                })
                .AddScoped<IRedisService, RedisService>();

            servicesProvider = services.BuildServiceProvider();
        }
    }
}
