using System.Threading.Tasks;
using Funda.ApiClient.Contracts;
using Funda.Common;

namespace Funda.ApiClient
{
    public interface IFundaApiClient
    {
        Task<OffersPage> GetOffers(int page, int pageSize, OfferType offerType, Filter filter);
    }
}
