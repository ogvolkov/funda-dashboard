using System;

namespace Funda.Api
{
    public class Property
    {
        public string Address { get; }

        public RealEstateAgent RealEstateAgent { get; }

        // and so on
        
        public Property(string address, RealEstateAgent realEstateAgent)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(address));
            }

            Address = address;
            RealEstateAgent = realEstateAgent ?? throw new ArgumentNullException(nameof(realEstateAgent));
        }
    }
}
