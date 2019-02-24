using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Funda.Api;
using Funda.ApiClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Funda.Dashboard.Runner
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var settings = GetSettings();

            Dashboard dashboard;

            using (var serviceProvider = Configure(settings))
            {
                var dashboardBuilder = serviceProvider.GetRequiredService<DashboardBuilder>();

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                dashboard = await dashboardBuilder.Build(settings.TopSize);

                Console.WriteLine("Finished building dashboard in {0} ms", stopwatch.ElapsedMilliseconds);
            }

            OutputRealEstateAgentsTable(
                $"Top {settings.TopSize} real estate agents in Amsterdam by number of properties",
                dashboard.AmsterdamTop
            );

            Console.WriteLine();

            OutputRealEstateAgentsTable(
                $"Top {settings.TopSize} real estate agents in Amsterdam by number of properties with a garden",
                dashboard.AmsterdamWithGardenTop
            );

#if DEBUG
            Console.ReadKey();
#endif
        }

        private static void OutputRealEstateAgentsTable(string header, IEnumerable<RealEstateAgentStats> agents)
        {
            Console.WriteLine(header);
            string format = "{0,-40} {1, 10}";
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(format, "Real estate agent", "Number of properties");
            Console.WriteLine("-------------------------------------------------------------");
            foreach (var realEstateAgent in agents)
            {
                Console.WriteLine(format, realEstateAgent.Name, realEstateAgent.PropertiesCount);
            }
        }

        private static Settings GetSettings()
        {
            string apiKey = Environment.GetEnvironmentVariable("FUNDA_API_KEY");

            var apiClientSettings = new FundaApiClientSettings(apiKey);

            const int BATCH_SIZE = 25;
            const int MAX_PAGES_TO_RETRIEVE = 5000;

            var apiSettings = new FundaApiSettings(BATCH_SIZE, MAX_PAGES_TO_RETRIEVE);

            const float DELAY = 2.0f;
            const float JITTER = 0.5f;
            const int RETRY_COUNT = 7;

            var retryPolicySettings = new RetryPolicySettings(DELAY, JITTER, RETRY_COUNT);

            const int TOP_SIZE = 10;

            return new Settings(TOP_SIZE, apiClientSettings, apiSettings, retryPolicySettings);
        }

        private static ServiceProvider Configure(Settings settings)
        {
            var services = new ServiceCollection();

            services.AddSingleton(settings.ApiClientSettings);
            services.AddSingleton(settings.ApiSettings);
            services.AddSingleton(settings.RetrySettings);

            services.AddTransient<FundaApiUrlBuilder>();

            services.AddTransient<IFundaApiClient, FundaApiClient>();

            services.AddSingleton<RetryPolicyProvider>();

            services.AddHttpClient<IFundaApiClient, FundaApiClient>()
                .AddPolicyHandler((provider, message) => provider.GetService<RetryPolicyProvider>().Get());

            services.AddTransient<IFundaApi, FundaApi>();

            services.AddTransient<DashboardBuilder>();

            services.AddLogging(builder => builder.AddConsole());

            return services.BuildServiceProvider();
        }
    }
}
