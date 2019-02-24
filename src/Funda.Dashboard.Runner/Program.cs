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

            using (var serviceProvider = Configure())
            {
                var dashboardBuilder = serviceProvider.GetRequiredService<DashboardBuilder>();

                int topSize = 10;

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var dashboard = await dashboardBuilder.Build(topSize);

                Console.WriteLine("Finished building dashboard in {0} ms", stopwatch.ElapsedMilliseconds);

                OutputRealEstateAgentsTable(
                    $"Top {topSize} real state agents in Amsterdam by number of properties",
                    dashboard.AmsterdamTop
                );

                OutputRealEstateAgentsTable(
                    $"Top {topSize} real state agents in Amsterdam by number of properties with garden",
                    dashboard.AmsterdamWithGardenTop
                );
            }

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
            Console.WriteLine();
        }

        private static ServiceProvider Configure()
        {
            string apiKey = Environment.GetEnvironmentVariable("FUNDA_API_KEY");
            var fundaApiClientSettings = new FundaApiClientSettings(apiKey);

            const int BATCH_SIZE = 25;
            const int MAX_PAGES_TO_RETRIEVE = 5000;
            var fundaApiSettings = new FundaApiSettings(BATCH_SIZE, MAX_PAGES_TO_RETRIEVE);

            var services = new ServiceCollection();

            services.AddSingleton(fundaApiClientSettings);

            services.AddTransient<FundaApiUrlBuilder>();

            services.AddTransient<IFundaApiClient, FundaApiClient>();

            services.AddHttpClient<IFundaApiClient, FundaApiClient>();

            services.AddSingleton(fundaApiSettings);

            services.AddTransient<IFundaApi, FundaApi>();

            services.AddTransient<DashboardBuilder>();

            services.AddLogging(builder => builder.AddConsole());

            return services.BuildServiceProvider();
        }
    }
}
