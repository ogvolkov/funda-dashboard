using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Funda.Api;
using Funda.Common;

namespace Funda.Dashboard
{
    public class DashboardBuilder
    {
        private readonly IFundaApi _fundaApi;

        public DashboardBuilder(IFundaApi fundaApi)
        {
            _fundaApi = fundaApi ?? throw new ArgumentNullException(nameof(fundaApi));
        }

        public async Task<Dashboard> Build(int topSize)
        {
            var amsterdamTop = await GetTop(new Filter("Amsterdam"), topSize);

            var amsterdamGardenTop = await GetTop(new Filter("Amsterdam") { Garden = true }, topSize);

            return new Dashboard(amsterdamTop, amsterdamGardenTop);
        }

        private async Task<List<RealEstateAgentStats>> GetTop(Filter filter, int topSize)
        {
            var properties = await _fundaApi.GetProperties(OfferType.Buy, filter).ToList();
            return CalculateTop(properties, topSize);
        }

        private List<RealEstateAgentStats> CalculateTop(IEnumerable<Property> properties, int topSize)
        {
            return properties
                .GroupBy(property => property.RealEstateAgent)
                .Select(group => new RealEstateAgentStats(group.Key.Name, group.Count()))
                .OrderByDescending(it => it.PropertiesCount)
                .Take(topSize)
                .ToList();
        }
    }
}
