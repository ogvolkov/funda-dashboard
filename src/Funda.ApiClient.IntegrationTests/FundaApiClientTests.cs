using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Funda.Common;
using NUnit.Framework;

namespace Funda.ApiClient.IntegrationTests
{
    [TestFixture]
    public class FundaApiClientTests
    {
        private FundaApiClient _apiClient;

        [SetUp]
        public void SetUp()
        {
            string apiKey = Environment.GetEnvironmentVariable("FUNDA_API_KEY");
            _apiClient = new FundaApiClient(new HttpClient(), new FundaApiUrlBuilder(), apiKey);
        }

        [Test]
        public async Task GetsResultsPage()
        {
            // arrange 
            var offerType = OfferType.Buy;
            var filters = new Filter("Amsterdam");

            // act
            var results = await _apiClient.GetOffers(1, 10, offerType, filters);

            // assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Objects, Is.Not.Null);
            Assert.That(results.Objects, Is.Not.Empty);
        }

        [Test]
        public async Task RealEstateAgentInformationIsPresent()
        {
            // arrange 
            var offerType = OfferType.Buy;
            var filters = new Filter("Amsterdam");

            // act
            var results = await _apiClient.GetOffers(1, 10, offerType, filters);

            // assert
            var sampleProperty = results.Objects.First();
            Assert.That(sampleProperty.MakelaarId, Is.GreaterThan(0));
            Assert.That(sampleProperty.MakelaarNaam, Is.Not.Null.Or.Empty);
        }

        [Test]
        public async Task PagingInformationIsPresent()
        {
            // arrange 
            var offerType = OfferType.Buy;
            var filters = new Filter("Amsterdam") { Garden = true };

            // act
            var results = await _apiClient.GetOffers(2, 10, offerType, filters);

            // assert
            Assert.That(results.Paging, Is.Not.Null);
            Assert.That(results.Paging.AantalPaginas, Is.GreaterThan(0));
        }
    }
}
