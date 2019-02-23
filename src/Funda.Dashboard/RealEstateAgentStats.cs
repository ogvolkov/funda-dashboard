namespace Funda.Dashboard
{
    public class RealEstateAgentStats
    {
        public string Name { get; }

        public int PropertiesCount { get; }

        public RealEstateAgentStats(string name, int propertiesCount)
        {
            Name = name;
            PropertiesCount = propertiesCount;
        }
    }
}
