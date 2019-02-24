using System;
using System.Reactive.Linq;
using Funda.ApiClient;
using Funda.Common;
using Object = Funda.ApiClient.Contracts.Object;

namespace Funda.Api
{
    public class FundaApi: IFundaApi
    {
        // artificial limit to make sure retrieval stops even if the returned page data is invalid
        private const int MAX_PAGE_COUNT = 5000;

        private readonly FundaApiSettings _fundaApiSettings;

        private readonly IFundaApiClient _fundaApiClient;

        public FundaApi(IFundaApiClient fundaApiClient, FundaApiSettings fundaApiSettings)
        {
            _fundaApiClient = fundaApiClient ?? throw new ArgumentNullException(nameof(fundaApiClient));
            _fundaApiSettings = fundaApiSettings ?? throw new ArgumentNullException(nameof(fundaApiSettings));
        }

        public IObservable<Property> GetProperties(OfferType offerType, Filter filter)
        {
            return Observable.Create<Property>(async observer =>
            {
                int pageCount = MAX_PAGE_COUNT;

                for (int page = 1; page <= pageCount; page++)
                {
                    var pageResults = await _fundaApiClient.GetOffers(page, _fundaApiSettings.BatchSize, offerType, filter);

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
