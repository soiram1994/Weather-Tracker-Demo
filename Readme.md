# Weather Tracker

Weather Tracker is a .NET 8 application designed to track weather data. It includes an API for retrieving weather data
and a worker service for synchronizing weather information from an external source.

## Table of Contents

- [Requirements](#requirements)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)
- [Troubleshooting](#troubleshooting)

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Installation

1. **Clone the repository:**

   ```sh
   git clone https://github.com/yourusername/weather-tracker.git
   cd weather-tracker
   ```

2. **Restore the dependencies:**

   ```sh
    dotnet restore
    ```

3. **Build the project:**
4. **Run the project:**

   ```sh
   dotnet run --project Weather.Tracker.Host
   ```

*Page under construction... Run the Weather.Tracker.Host project for overview of the project, currently the data stored
are from London. The target city can be changed by settings the 'CITY_NAME' environment variable in the SyncWorker*
