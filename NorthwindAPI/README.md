# Northwind API

This project is an ASP.NET Core Web API that provides endpoints for querying the Northwind database and cat facts.

## Setup

1. Clone the repository
2. Update the connection string in `appsettings.json` to point to your Northwind database
3. Run `dotnet restore` to restore NuGet packages
4. Run `dotnet run` to start the application

## API Endpoints

### Northwind Orders

- GET /api/orders/recent/{customerId} - Get recent orders for a customer
- GET /api/orders/recent/{customerId}/csv - Get recent orders for a customer in CSV format

### Cat Facts

- GET /api/catfacts - Get all cat facts
- GET /api/catfacts/verified - Get verified cat facts
- GET /api/catfacts/created?date=YYYY-MM-DD - Get facts created on a specific date
- GET /api/catfacts/updated?date=YYYY-MM-DD - Get facts updated on a specific date

## Technologies Used

- ASP.NET Core 8
- Entity Framework Core
- Newtonsoft.Json