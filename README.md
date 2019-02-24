# Funda dashboard
Aggregate some data from Funda partner API.

Determine top 10 real estate agents in Amsterdam having the most properties listed for sale; the same for properties with a garden.

## Prerequisites
.NET Core 2.2.

Put your API key into the FUNDA_API_KEY environment variable (required both for running and the API integration tests).

## Running
```
dotnet run --project src/Funda.Dashboard.Runner
```