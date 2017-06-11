using System;
using Xunit;
using StooqApi;
using System.Linq;

namespace StooqApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void HkTest()
        {
            var candle = Stooq.GetHistoricalAsync("1111.HK", Period.Daily, new DateTime(2017, 1, 3), new DateTime(2017, 1, 3)).Result.First();
            Assert.Equal(candle.Open, 14.696m);
            Assert.Equal(candle.High, 14.93m);
            Assert.Equal(candle.Low, 14.696m);
            Assert.Equal(candle.Close, 14.735m);
            Assert.Equal(candle.Volume, 48101);
        }

        [Fact]
        public void SpxTest()
        {
			var candle = Stooq.GetHistoricalAsync("^SPX", Period.Daily, new DateTime(2017, 1, 3), new DateTime(2017, 1, 3)).Result.First();
            Assert.Equal(candle.Open, 2251.57m);
            Assert.Equal(candle.High, 2263.88m);
            Assert.Equal(candle.Low, 2245.13m);
            Assert.Equal(candle.Close, 2257.83m);
            Assert.Equal(candle.Volume, 644_640_832);
		}
    }
}
