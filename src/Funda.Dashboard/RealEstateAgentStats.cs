using System;

namespace Funda.Dashboard
{
    public class RealEstateAgentStats
    {
        public string Name { get; }

        public int PropertiesCount { get; }

        public RealEstateAgentStats(string name, int propertiesCount)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (propertiesCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(propertiesCount));
            }

            Name = name;
            PropertiesCount = propertiesCount;
        }
    }
}
