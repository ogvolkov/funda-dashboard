using System.Threading.Tasks;
using Funda.ApiClient.Contracts;
using Funda.Common;

namespace Funda.ApiClient
{
    /// <summary>
    /// Low-level Funda API client.
    /// Methods and contracts correspond closely to the actual HTTP API.
    /// </summary>
    public interface IFundaApiClient
    {
        Task<OffersPage> GetOffers(int page, int pageSize, OfferType offerType, Filter filter);
    }
}
