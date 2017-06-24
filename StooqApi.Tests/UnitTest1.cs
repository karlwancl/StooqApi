using System;
using Xunit;
using StooqApi;
using System.Linq;
using System.Collections.Generic;

namespace StooqApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void EodTest()
        {
            var candles = Stooq.GetHistoricalAsync("^SPX", Period.Daily, new DateTime(2017, 1, 3), ascending: true).Result;
            var candle = candles.First();
            Assert.Equal(candle.DateTime, new DateTime(2017, 1, 3));
            Assert.Equal(candle.Open, 2251.57m);
            Assert.Equal(candle.High, 2263.88m);
            Assert.Equal(candle.Low, 2245.13m);
            Assert.Equal(candle.Close, 2257.83m);
            Assert.Equal(candle.Volume, 644_640_832);
		}

        [Fact]
        public void PeriodTest()
        {
            const decimal open = 2251.57m;
            Enum.GetValues(typeof(Period)).Cast<Period>().ToList().ForEach(p =>
            {
                var candle = Stooq.GetHistoricalAsync("^SPX", p, new DateTime(2017, 1, 3), ascending: true).Result.First();
                Assert.Equal(candle.Open, open);
            });
        }

        [Fact]
        public void DateTimeTest()
        {
            var candle = Stooq.GetHistoricalAsync("^SPX", Period.Daily, ascending: true).Result;
            Assert.Equal(candle.First().DateTime, new DateTime(1789, 5, 1));

            var candle2 = Stooq.GetHistoricalAsync("^SPX", Period.Daily, new DateTime(2017, 1, 3), ascending: true).Result;
            Assert.Equal(candle2.First().DateTime, new DateTime(2017, 1, 3));

            var candle3 = Stooq.GetHistoricalAsync("^SPX", Period.Daily, new DateTime(2017, 1, 3), new DateTime(2017, 2, 3), ascending: true).Result;
            Assert.Equal(candle3.First().DateTime, new DateTime(2017, 1, 3));
            Assert.Equal(candle3.Last().DateTime, new DateTime(2017, 2, 3));

            var candle4 = Stooq.GetHistoricalAsync("^SPX", Period.Daily, endTime: new DateTime(2017, 2, 3), ascending: true).Result;
            Assert.Equal(candle4.Last().DateTime, new DateTime(2017, 2, 3));
        }
    }
}
