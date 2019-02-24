using System;
using Funda.Api;
using Funda.ApiClient;

namespace Funda.Dashboard.Runner
{
    public class Settings
    {
        public FundaApiClientSettings ApiClientSettings { get; }

        public FundaApiSettings ApiSettings { get; }

        public RetryPolicySettings RetrySettings { get; }

        public int TopSize { get; }

        public Settings(
            int topSize,
            FundaApiClientSettings apiClientSettings,
            FundaApiSettings apiSettings,
            RetryPolicySettings retrySettings
        )
        {
            TopSize = topSize;
            ApiClientSettings = apiClientSettings ?? throw new ArgumentNullException(nameof(apiClientSettings));
            ApiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));
            RetrySettings = retrySettings ?? throw new ArgumentNullException(nameof(retrySettings));
        }
    }
}
