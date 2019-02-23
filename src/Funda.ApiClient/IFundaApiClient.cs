using System.Threading.Tasks;
using Funda.ApiClient.Contracts;

namespace Funda.ApiClient
{
    public interface IFundaApiClient
    {
        Task<OffersPage> GetOffers(int page, int pageSize, OfferType offerType, Filter filter);
    }
}
