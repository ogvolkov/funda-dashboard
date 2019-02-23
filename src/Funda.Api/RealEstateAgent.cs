using System;

namespace Funda.Api
{
    public class RealEstateAgent
    {
        public int Id { get; }

        public string Name { get; }

        public RealEstateAgent(int id, string name)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            Id = id;
            Name = name;
        }
    }
}
