using NUnit.Framework;

namespace Funda.ApiClient.IntegrationTests
{
    [TestFixture]
    public class FundaApiUrlBuilderTests
    {
        private FundaApiUrlBuilder _urlBuilder;

        [SetUp]
        public void SetUp()
        {
            _urlBuilder = new FundaApiUrlBuilder();
        }

        [Test]
        public void BuildsCorrectUrl()
        {
            // arrange
            var offerType = OfferType.Buy;
            var filters = new Filter("Amsterdam");

            // act
            string url = _urlBuilder.BuildUri("key123", 1, 10, offerType, filters);

            // assert
            Assert.That(url, Is.EqualTo("http://partnerapi.funda.nl/feeds/Aanbod.svc/json/key123/?type=koop&zo=/amsterdam/&page=1&pagesize=10"));
        }

        [Test]
        public void BuildsCorrectUrlForGardenFilter()
        {
            // arrange
            var offerType = OfferType.Buy;
            var filters = new Filter("Amsterdam") { Garden = true };

            // act
            string url = _urlBuilder.BuildUri("key123", 3, 25, offerType, filters);

            // assert
            Assert.That(url, Is.EqualTo("http://partnerapi.funda.nl/feeds/Aanbod.svc/json/key123/?type=koop&zo=/amsterdam/tuin/&page=3&pagesize=25"));
        }
    }
}
