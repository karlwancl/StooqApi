using System;
using Xunit;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace StooqApi.Tests
{
    public class UnitTest1
    {
        public UnitTest1()
        {
            // Test culture info
            //CultureInfo.CurrentCulture = new CultureInfo("nl-nl");
        }

        [Fact]
        public void IndexEodTest()
        {
            var candles = Stooq.GetHistoricalAsync("^SPX", Period.Daily, new DateTime(2017, 1, 3), ascending: true).Result;
            var candle = candles.First();
            Assert.Equal(candle.DateTime, new DateTime(2017, 1, 3));
            Assert.Equal(2251.57m, candle.Open);
            Assert.Equal(2263.88m, candle.High);
            Assert.Equal(2245.13m, candle.Low);
            Assert.Equal(2257.83m, candle.Close);
            Assert.Equal(644_640_832, candle.Volume);
		}

        [Fact]
        public void EodTest()
        {
			var candles = Stooq.GetHistoricalAsync("aapl.us", Period.Daily, new DateTime(2017, 1, 3), ascending: true).Result;
			var candle = candles.First();
			Assert.Equal(candle.DateTime, new DateTime(2017, 1, 3));
			Assert.Equal(110.36m, candle.Open);
			Assert.Equal(110.88m, candle.High);
			Assert.Equal(109.35m, candle.Low);
			Assert.Equal(110.7m, candle.Close);
			Assert.Equal(29_108_191, candle.Volume);
        }

        [Fact]
        public void PeriodTest()
        {
            const decimal open = 2251.57m;
            Enum.GetValues(typeof(Period)).Cast<Period>().Except(new Period[] { Period.Yearly }).ToList().ForEach(p =>
            {
                var candle = Stooq.GetHistoricalAsync("^SPX", p, new DateTime(2017, 1, 3), ascending: true).Result.First();
                Assert.Equal(candle.Open, open);
                Thread.Sleep(1000);
            });
        }

        [Fact]
        public void DateTimeTest()
        {
            // Bug: Stooq returns empty csv if start date < the particular date, no candle can be retrieved in this case
            var candle = Stooq.GetHistoricalAsync("^SPX", Period.Daily, ascending: true).Result;
            Assert.Equal(candle.First().DateTime, new DateTime(1789, 5, 1));
            Thread.Sleep(1000);

            var candle2 = Stooq.GetHistoricalAsync("^SPX", Period.Daily, new DateTime(2017, 1, 3), ascending: true).Result;
            Assert.Equal(candle2.First().DateTime, new DateTime(2017, 1, 3));
			Thread.Sleep(1000);

            var candle3 = Stooq.GetHistoricalAsync("^SPX", Period.Daily, new DateTime(2017, 1, 3), new DateTime(2017, 2, 3), ascending: true).Result;
            Assert.Equal(candle3.First().DateTime, new DateTime(2017, 1, 3));
            Assert.Equal(candle3.Last().DateTime, new DateTime(2017, 2, 3));
			Thread.Sleep(1000);

            // Bug: Stooq returns empty csv if start date < the particular date, no candle can be retrieved in this case
            var candle4 = Stooq.GetHistoricalAsync("^SPX", Period.Daily, endTime: new DateTime(2017, 2, 3), ascending: true).Result;
            Assert.Equal(candle4.Last().DateTime, new DateTime(2017, 2, 3));
        }
    }
}
