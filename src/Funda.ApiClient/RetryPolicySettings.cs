using System;

namespace Funda.ApiClient
{
    public class RetryPolicySettings
    {
        public float Delay { get; }

        public float Jitter { get; }

        public int RetryCount { get; }

        public RetryPolicySettings(float delay, float jitter, int retryCount)
        {
            if (delay <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(delay));
            }

            if (jitter <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(jitter));
            }

            if (retryCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount));
            }

            Delay = delay;
            Jitter = jitter;
            RetryCount = retryCount;
        }
    }
}
