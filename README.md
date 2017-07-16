# StooqApi
[![Build status](https://ci.appveyor.com/api/projects/status/d0c3l3kuj0yxmuq4?svg=true)](https://ci.appveyor.com/project/lppkarl/stooqapi)
[![NuGet](https://img.shields.io/nuget/v/StooqApi.svg)](https://www.nuget.org/packages/StooqApi/)
[![license](https://img.shields.io/github/license/lppkarl/StooqApi.svg)](https://github.com/lppkarl/StooqApi/blob/master/LICENSE)

A .NET wrapper for Stooq, based on .NET Standard 1.4

## Note
Query of older historical price is not working properly because stooq is returning empty csv when an older start date is used. Stooq's query system is bugged.

## Feature
* Get historical data from Stooq.com

## Installation
    PM> Install-Package StooqApi

## Usage
### Add Reference
    using StooqApi;

### Get Historical Data
    var candles = await Stooq.GetHistoricalAsync("aapl", Period.Daily, new DateTime(2017, 1, 3));
    Console.WriteLine($"DateTime: {candles[0].DateTime}, Open: {candles[0].Open}, High: {candles[0].High}, Low: {candles[0].Low}, Close: {candles[0].Close}");

### Powered by
* [Flurl](https://github.com/tmenier/Flurl) ([@tmenier](https://github.com/tmenier)) - A simple & elegant fluent-style REST api library 
