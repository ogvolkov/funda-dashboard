
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Funda.Api;
using Funda.Common;
using NUnit.Framework;

namespace Funda.Dashboard.Tests
{
    [TestFixture]
    public class DashboardBuilderTests
    {
        private FundaApiStub _fundaApiStub;

        private DashboardBuilder _dashboardBuilder;

        [SetUp]
        public void SetUp()
        {
            _fundaApiStub = new FundaApiStub();
            _dashboardBuilder = new DashboardBuilder(_fundaApiStub, 2);
        }

        [Test]
        public async Task CalculatesTopCorrectly()
        {
            // arrange
            int topSize = 2;

            var properties = new List<Property>
            {
                new Property("A1", new RealEstateAgent(1, "Agent 1")), 
                new Property("A2", new RealEstateAgent(2, "Agent 2")), 
                new Property("A3", new RealEstateAgent(3, "Agent 3")), 
                new Property("A4", new RealEstateAgent(3, "Agent 3")), 
                new Property("A5", new RealEstateAgent(1, "Agent 1")), 
                new Property("A6", new RealEstateAgent(1, "Agent 1"))
            };

            _fundaApiStub.Setup[(OfferType.Buy, new Filter("Amsterdam"))] = properties;
            _fundaApiStub.Setup[(OfferType.Buy, new Filter("Amsterdam") { Garden = true })] = new List<Property>();

            // act
            var dashboard = await _dashboardBuilder.Build(topSize);

            // assert
            Assert.That(dashboard.AmsterdamTop, Is.Not.Null);
            Assert.That(dashboard.AmsterdamTop.Count, Is.EqualTo(2));

            Assert.That(dashboard.AmsterdamTop[0].Name, Is.EqualTo("Agent 1"));
            Assert.That(dashboard.AmsterdamTop[0].PropertiesCount, Is.EqualTo(3));

            Assert.That(dashboard.AmsterdamTop[1].Name, Is.EqualTo("Agent 3"));
            Assert.That(dashboard.AmsterdamTop[1].PropertiesCount, Is.EqualTo(2));
        }

        [Test]
        public async Task CalculatesGardenTopCorrectly()
        {
            // arrange
            int topSize = 1;

            var properties = new List<Property>
            {
                new Property("A1", new RealEstateAgent(1, "Agent 1")),
                new Property("A4", new RealEstateAgent(3, "Agent 3")),
                new Property("A5", new RealEstateAgent(2, "Agent 2")),
                new Property("A6", new RealEstateAgent(1, "Agent 1"))
            };

            _fundaApiStub.Setup[(OfferType.Buy, new Filter("Amsterdam"))] = new List<Property>();
            _fundaApiStub.Setup[(OfferType.Buy, new Filter("Amsterdam") { Garden = true })] = properties;

            // act
            var dashboard = await _dashboardBuilder.Build(topSize);

            // assert
            Assert.That(dashboard.AmsterdamWithGardenTop, Is.Not.Null);
            Assert.That(dashboard.AmsterdamWithGardenTop.Count, Is.EqualTo(1));

            Assert.That(dashboard.AmsterdamWithGardenTop[0].Name, Is.EqualTo("Agent 1"));
            Assert.That(dashboard.AmsterdamWithGardenTop[0].PropertiesCount, Is.EqualTo(2));
        }


        private class FundaApiStub : IFundaApi
        {
            public IDictionary<(OfferType, Filter), IEnumerable<Property>> Setup { get; }
                = new Dictionary<(OfferType, Filter), IEnumerable<Property>>();

            public IObservable<Property> GetProperties(OfferType offerType, Filter filter)
            {
                var cannedValue = Setup.FirstOrDefault(
                    it => it.Key.Item1 == offerType
                          && it.Key.Item2.Place == filter.Place
                          && it.Key.Item2.Garden == filter.Garden)
                    .Value;

                return cannedValue.ToObservable();
            }
        }
    }
}
