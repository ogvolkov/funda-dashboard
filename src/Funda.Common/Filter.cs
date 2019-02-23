using System;

namespace Funda.Common
{
    public class Filter
    {
        public string Place { get; }

        public bool Garden { get; set; }

        public Filter(string place)
        {
            if (string.IsNullOrWhiteSpace(place))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(place));
            }

            Place = place;
        }
    }
}
