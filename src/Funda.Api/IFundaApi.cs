using System;
using Funda.Common;

namespace Funda.Api
{
    public interface IFundaApi
    {
        IObservable<Property> GetProperties(OfferType offerType, Filter filter);
    }
}
