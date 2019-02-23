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

        public override int GetHashCode()
        {
            return (Id, Name).GetHashCode();
        }

        public override bool Equals(object another)
        {
            if (another == null)
            {
                return false;
            }

            if (another.GetType() != GetType())
            {
                return false;
            }

            RealEstateAgent anotherAgent = (RealEstateAgent) another;

            return Id == anotherAgent.Id && Name == anotherAgent.Name;
        }
    }
}
