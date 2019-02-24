
using System;

namespace Funda.ApiClient
{
    public class FundaApiClientSettings
    {
        public string ApiKey { get; }

        public FundaApiClientSettings(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiKey));
            }

            ApiKey = apiKey;
        }
    }
}
