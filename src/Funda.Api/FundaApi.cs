using System;
using System.Reactive.Linq;
using Funda.ApiClient;
using Funda.Common;
using Microsoft.Extensions.Logging;
using Object = Funda.ApiClient.Contracts.Object;

namespace Funda.Api
{
    public class FundaApi: IFundaApi
    {
        private readonly ILogger<FundaApi> _logger;

        private readonly FundaApiSettings _settings;

        private readonly IFundaApiClient _fundaApiClient;

        public FundaApi(IFundaApiClient fundaApiClient, FundaApiSettings settings, ILogger<FundaApi> logger)
        {
            _fundaApiClient = fundaApiClient ?? throw new ArgumentNullException(nameof(fundaApiClient));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger;
        }

        public IObservable<Property> GetProperties(OfferType offerType, Filter filter)
        {
            return Observable.Create<Property>(async observer =>
            {
                int pageCount = _settings.MaxPageCountToRetrieve;

                for (int page = 1; page <= pageCount; page++)
                {
                    _logger.LogInformation("Getting page {0}, page size {1}", page, _settings.BatchSize);

                    var pageResults = await _fundaApiClient.GetOffers(page, _settings.BatchSize, offerType, filter);
                    int totalPages = pageResults.Paging.AantalPaginas;

                    _logger.LogInformation("Received page {0}, total pages {1}", page, totalPages);
                    _logger.LogInformation("Received {0} properties", pageResults.Objects.Length);

                    foreach (Object o in pageResults.Objects)
                    {
                        var realEstateAgent = new RealEstateAgent(o.MakelaarId, o.MakelaarNaam);
                        var property = new Property(o.Adres, realEstateAgent);

                        observer.OnNext(property);
                    }

                    pageCount = Math.Min(totalPages, _settings.MaxPageCountToRetrieve);
                }
            });
        }
    }
}
