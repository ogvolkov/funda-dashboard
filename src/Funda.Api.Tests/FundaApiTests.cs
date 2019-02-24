using System.Reactive.Linq;
using System.Threading.Tasks;
using Funda.ApiClient;
using Funda.ApiClient.Contracts;
using Funda.Common;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Object = Funda.ApiClient.Contracts.Object;

namespace Funda.Api.Tests
{
    [TestFixture]
    public class FundaApiTests
    {
        private ApiClientStub _apiClientStub;

        private FundaApi _fundaApi;

        [SetUp]
        public void SetUp()
        {
            _apiClientStub = new ApiClientStub();
            _fundaApi = new FundaApi(_apiClientStub, new FundaApiSettings(2, 1000), new NullLogger<FundaApi>());
        }

        [Test]
        public async Task GetsAllPages()
        {
            // arrange
            var object1 = CreateObject();
            var object2 = CreateObject();
            var object3 = CreateObject();

            _apiClientStub.Setup(
                new[] { object1, object2 },
                new[] { object3 }
            );

            // act 
            var properties = await _fundaApi.GetProperties(OfferType.Buy, new Filter("Amsterdam")).ToList();

            // assert
            Assert.That(properties.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task ReturnsPropertyAddress()
        {
            // arrange
            var object1 = CreateObject();

            _apiClientStub.Setup(new[] { object1 });

            // act 
            var properties = await _fundaApi.GetProperties(OfferType.Buy, new Filter("Amsterdam")).ToList();

            // assert
            Assert.That(properties[0].Address, Is.EqualTo(object1.Adres));
        }

        [Test]
        public async Task ReturnsRealEstateAgent()
        {
            // arrange
            var object1 = CreateObject();

            _apiClientStub.Setup(new[] { object1 });

            // act 
            var properties = await _fundaApi.GetProperties(OfferType.Buy, new Filter("Amsterdam")).ToList();

            // assert
            Assert.That(properties[0].RealEstateAgent, Is.Not.Null);
            Assert.That(properties[0].RealEstateAgent.Id, Is.EqualTo(object1.MakelaarId));
            Assert.That(properties[0].RealEstateAgent.Name, Is.EqualTo(object1.MakelaarNaam));
        }

        private class ApiClientStub : IFundaApiClient
        {
            private Object[][] _pagedProperties;

            public void Setup(params Object[][] pagedProperties)
            {
                _pagedProperties = pagedProperties;
            }

            public Task<OffersPage> GetOffers(int page, int pageSize, OfferType offerType, Filter filter)
            {
                OffersPage resultPage = new OffersPage
                {
                    Objects = _pagedProperties[page - 1],
                    Paging = new Paging
                    {
                        AantalPaginas = _pagedProperties.Length
                    }
                };

                return Task.FromResult(resultPage);
            }
        }

        private static int _agentId = 1;

        private Object CreateObject()
        {
            int id = _agentId++;

            return new Object
            {
                MakelaarId = id++,
                MakelaarNaam = $"Makelaar {id}",
                Adres = $"Adres {id}"
            };
        }
    }
}
