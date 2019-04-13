using System;

namespace Redis.Hello.Models
{
    public enum Gender { Male, Female }

    [Serializable]
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Gender Sex { get; set; }
    }
}
