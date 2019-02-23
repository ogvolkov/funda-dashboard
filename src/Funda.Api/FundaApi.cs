using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Funda.ApiClient;
using Funda.ApiClient.Contracts;
using Funda.Common;
using Object = Funda.ApiClient.Contracts.Object;

namespace Funda.Api
{
    public class FundaApi: IFundaApi
    {
        // artificial limit to make sure retrieval stops even if the returned page data is invalid
        private const int MAX_PAGE_COUNT = 5000;

        private readonly IFundaApiClient _fundaApiClient;

        public FundaApi(IFundaApiClient fundaApiClient)
        {
            _fundaApiClient = fundaApiClient ?? throw new ArgumentNullException(nameof(fundaApiClient));
        }

        public IObservable<Property> GetProperties(OfferType offerType, Filter filter, int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            return Observable.Create<Property>(async observer =>
            {
                int pageCount = MAX_PAGE_COUNT;

                for (int page = 1; page <= pageCount; page++)
                {
                    var pageResults = await _fundaApiClient.GetOffers(page, pageSize, offerType, filter);

                    foreach (Object o in pageResults.Objects)
                    {
                        var realEstateAgent = new RealEstateAgent(o.MakelaarId, o.MakelaarNaam);
                        var property = new Property(o.Adres, realEstateAgent);

                        observer.OnNext(property);
                    }

                    pageCount = pageResults.Paging.AantalPaginas;
                }
            });
        }
    }
}
