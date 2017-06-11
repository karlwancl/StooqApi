using System;
namespace StooqApi
{
    public class Candle
    {
        public DateTime DateTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }

        public Candle(DateTime dateTime, decimal open, decimal high, decimal low, decimal close, decimal volume)
        {
            Volume = volume;
            Close = close;
            Low = low;
            High = high;
            Open = open;
            DateTime = dateTime;
        }
    }
}
