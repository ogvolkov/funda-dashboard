using System;
using System.Net.Http;
using System.Threading.Tasks;
using Funda.ApiClient.Contracts;
using Newtonsoft.Json;

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

        public async Task<OffersPage> GetOffers(int page, int pageSize, OfferType offerType, Filter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            string url = _apiUrlBuilder.BuildUri(_apiKey, page, pageSize, offerType, filter);

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<OffersPage>(content);
        }

    }
}
