using System.Net.Http;
using System.Threading.Tasks;
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
            _apiClient = new FundaApiClient(new HttpClient(), new FundaApiUrlBuilder(), "xxx");
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
            Assert.That(results.Objects.Length, Is.Not.Empty);
        }
    }
}
