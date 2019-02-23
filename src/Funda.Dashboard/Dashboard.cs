using System;
using System.Collections.Generic;

namespace Funda.Dashboard
{
    public class Dashboard
    {
        public List<RealEstateAgentStats> AmsterdamTop { get; }

        public List<RealEstateAgentStats> AmsterdamWithGardenTop { get; }

        public Dashboard(List<RealEstateAgentStats> amsterdamTop, List<RealEstateAgentStats> amsterdamWithGardenTop)
        {
            AmsterdamTop = amsterdamTop ?? throw new ArgumentNullException(nameof(amsterdamTop));
            AmsterdamWithGardenTop = amsterdamWithGardenTop ?? throw new ArgumentNullException(nameof(amsterdamWithGardenTop));
        }
    }
}
