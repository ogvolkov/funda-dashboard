﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.Api;
using Funda.ApiClient;
using Microsoft.Extensions.DependencyInjection;

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
                var dashboard = await dashboardBuilder.Build(topSize);

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
            Console.WriteLine("-------------------------------------------------------------");
            foreach (var realEstateAgent in agents)
            {
                Console.WriteLine($"{realEstateAgent.Name}\t\t\t{realEstateAgent.PropertiesCount} properties");
            }
        }

        private static ServiceProvider Configure()
        {
            string apiKey = Environment.GetEnvironmentVariable("FUNDA_API_KEY");
            var fundaApiClientSettings = new FundaApiClientSettings(apiKey);

            const int BATCH_SIZE = 25;
            var fundaApiSettings = new FundaApiSettings(BATCH_SIZE);

            var services = new ServiceCollection();

            services.AddSingleton(fundaApiClientSettings);

            services.AddTransient<FundaApiUrlBuilder>();

            services.AddTransient<IFundaApiClient, FundaApiClient>();

            services.AddHttpClient<IFundaApiClient, FundaApiClient>();

            services.AddSingleton(fundaApiSettings);

            services.AddTransient<IFundaApi, FundaApi>();

            services.AddTransient<DashboardBuilder>();

            return services.BuildServiceProvider();
        }
    }
}