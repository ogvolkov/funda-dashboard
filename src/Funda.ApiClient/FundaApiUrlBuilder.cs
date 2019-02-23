﻿using System;
using System.Text;

namespace Funda.ApiClient
{
    public class FundaApiUrlBuilder
    {
        private const string API_ROOT = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/";

        public string BuildUri(string key, int page, int pageSize, OfferType offerType, Filter filter)
        {
            var uriBuilder = new StringBuilder();
            uriBuilder.Append(API_ROOT);
            uriBuilder.Append(key);

            // QueryHelpers.AddQueryString escapes slashes in zo, so construct the url manually
            uriBuilder.AppendFormat("/?type={0}", ConvertOfferTypeToQueryParameter(offerType));
            uriBuilder.AppendFormat("&zo={0}", ConvertFilterToQueryParameter(filter));
            uriBuilder.AppendFormat("&page={0}", page);
            uriBuilder.AppendFormat("&pagesize={0}", pageSize);

            return uriBuilder.ToString();
        }

        private string ConvertFilterToQueryParameter(Filter filter)
        {
            var filterBuilder = new StringBuilder();
            filterBuilder.Append("/");
            filterBuilder.Append(filter.Place.ToLower());

            if (filter.Garden)
            {
                filterBuilder.Append("/tuin");
            }

            filterBuilder.Append("/");

            return filterBuilder.ToString();
        }

        private string ConvertOfferTypeToQueryParameter(OfferType offerType)
        {
            switch (offerType)
            {
                case OfferType.Buy:
                    return "koop";
                // etc.

                default:
                    throw new NotImplementedException($"Offer type {offerType} is not supported as a query string parameter");
            }
        }
    }
}