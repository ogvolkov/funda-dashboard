using System;

namespace Funda.Api
{
    public class FundaApiSettings
    {
        public int BatchSize { get; }

        public FundaApiSettings(int batchSize)
        {
            if (batchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            BatchSize = batchSize;
        }
    }
}
