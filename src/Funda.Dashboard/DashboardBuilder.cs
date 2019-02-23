﻿using System;
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

        private readonly int _pageSize;

        public DashboardBuilder(IFundaApi fundaApi, int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            _fundaApi = fundaApi ?? throw new ArgumentNullException(nameof(fundaApi));
            _pageSize = pageSize;
        }

        public async Task<Dashboard> Build(int topSize)
        {
            var amsterdamTop = await GetTop(new Filter("Amsterdam"), topSize);

            var amsterdamGardenTop = await GetTop(new Filter("Amsterdam") { Garden = true }, topSize);

            return new Dashboard(amsterdamTop, amsterdamGardenTop);
        }

        private async Task<List<RealEstateAgentStats>> GetTop(Filter filter, int topSize)
        {
            var properties = await _fundaApi.GetProperties(OfferType.Buy, filter, _pageSize).ToList();
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
