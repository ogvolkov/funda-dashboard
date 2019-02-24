using System;
using Funda.Api.Contracts;
using Funda.Common;

namespace Funda.Api
{
    /// <summary>
    /// High-level API for Funda.
    /// </summary>
    public interface IFundaApi
    {
        /// <summary>
        /// Returns all the offered properties by the given offer type and filter.
        /// </summary>
        /// <returns>
        /// Observable to be populated with all properties.
        /// As we need both multiple results and async, IObservable is the best bet before the arrival of async streams.
        /// </returns>
        IObservable<Property> GetProperties(OfferType offerType, Filter filter);
    }
}
