using System;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Retry;

namespace Funda.ApiClient
{
    public class RetryPolicyProvider
    {
        private static readonly Random _jitterer = new Random();

        private readonly RetryPolicySettings _settings;

        public RetryPolicyProvider(RetryPolicySettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public AsyncRetryPolicy<HttpResponseMessage> Get()
        {
            var policy = Policy
                .HandleResult<HttpResponseMessage>(ShouldRetryRequest)
                .WaitAndRetryAsync(_settings.RetryCount, CalculateDelay);

            return policy;
        }

        private bool ShouldRetryRequest(HttpResponseMessage response)
        {
            return response.StatusCode == HttpStatusCode.Unauthorized && response.ReasonPhrase == "Request limit exceeded";
        }

        private TimeSpan CalculateDelay(int retryAttempt)
        {
            TimeSpan exponentialBackOff = TimeSpan.FromSeconds(_settings.Delay * Math.Pow(2, retryAttempt - 1));
            TimeSpan jitter = TimeSpan.FromMilliseconds(_jitterer.NextDouble() * _settings.Jitter);

            return exponentialBackOff + jitter;
        }
    }
}
