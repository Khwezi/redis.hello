using Redis.Hello.Models;

namespace Redis.Hello
{
    public interface IRedisService
    {
        void Push(Person data);

        Person Pull();
    }
}
