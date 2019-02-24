using System;

namespace Funda.Api
{
    public class FundaApiSettings
    {
        private const int MAX_BATCH_SIZE = 25;

        public int BatchSize { get; }

        // artificial limit to make sure the retrieval eventually stops even if the returned paging data is invalid
        public int MaxPageCountToRetrieve { get; }

        public FundaApiSettings(int batchSize, int maxPageCountToRetrieve)
        {
            if (batchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            if (batchSize > MAX_BATCH_SIZE)
            {
                // Funda API accepts any given page size, but returns 25 results max.
                // However, the paging info in the response is calculated using the given page size, not the actual one,
                // so relying on that might result in incomplete results,
                // We prefer to detect the potential issue earlier, so throw.
                throw new ArgumentOutOfRangeException(nameof(batchSize), $"{MAX_BATCH_SIZE} results max are returned by the HTTP API, make sure batch size is smaller");
            }

            BatchSize = batchSize;
            MaxPageCountToRetrieve = maxPageCountToRetrieve;
        }
    }
}
