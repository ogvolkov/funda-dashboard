using System;
using System.Net.Http;
using System.Threading.Tasks;
using Funda.ApiClient.Contracts;

namespace Funda.ApiClient
{
    public class FundaApiClient : IFundaApiClient
    {
        private readonly FundaApiUrlBuilder _apiUrlBuilder;

        private readonly HttpClient _httpClient;

        private readonly string _apiKey;

        public FundaApiClient(HttpClient httpClient, FundaApiUrlBuilder apiUrlBuilder, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiKey));
            }

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiUrlBuilder = apiUrlBuilder ?? throw new ArgumentNullException(nameof(apiUrlBuilder));
            _apiKey = apiKey;
        }

        public Task<OffersPage> GetOffers(int page, int pageSize, OfferType offerType, Filter filter)
        {
            throw new NotImplementedException();
        }

    }
}
