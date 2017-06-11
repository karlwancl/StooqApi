# StooqApi
A .NET wrapper for Stooq, based on .NET Standard 1.4

## Feature
* Get historical data from Stooq.com

## Installation (Through Nuget)
    PM > Install-Package StooqApi

## Usage
### Add Reference
    Using StooqApi;

### Get Historical Data
    var candles = await Stooq.GetHistoricalAsync("aapl", Period.Daily, new DateTime(2017, 1, 3), new DateTime(2017, 1, 4));
    Console.WriteLine($"DateTime: {candles[0].DateTime}, Open: {candles[0].Open}, High: {candles[0].High}, Low: {candles[0].Low}, Close: {candles[0].Close}");

### Powered by
* [Flurl](https://github.com/tmenier/Flurl) ([@tmenier](https://github.com/tmenier)) - A simple & elegant fluent-style REST api library 

### License
This library is under [MIT License](https://github.com/lppkarl/StooqApi/blob/master/LICENSE)

