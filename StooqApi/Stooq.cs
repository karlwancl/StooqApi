using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using Flurl;
using Flurl.Http;

namespace StooqApi
{
    public static class Stooq
    {
        const string StooqUrl = "https://stooq.com/q/d/l";
        const string SymbolTag = "s";
        const string PeriodTag = "i";
        const string StartTimeTag = "d1";
        const string EndTimeTag = "d2";
        const string SkipTag = "o";

        public static async Task<IList<Candle>> GetHistoricalAsync(string symbol, Period period = Period.Daily, DateTime? startTime = null, DateTime? endTime = null, SkipOption skipOption = SkipOption.None, bool ascending = false, CancellationToken token = default(CancellationToken))
        {
            var candles = new List<Candle>();
            using (var s = await GetResponseStreamAsync(symbol, period, startTime, endTime, skipOption, token).ConfigureAwait(false))
            using (var sr = new StreamReader(s))
            using (var csvReader = new CsvReader(sr))
            {
                while (csvReader.Read())
                {
                    string[] row = csvReader.CurrentRecord;
                    try
                    {
                        candles.Add(new Candle(
                            Convert.ToDateTime(row[0]),
                            Convert.ToDecimal(row[1]),
                            Convert.ToDecimal(row[2]),
                            Convert.ToDecimal(row[3]),
                            Convert.ToDecimal(row[4]),
                            Convert.ToDecimal(row[5])
                        ));
                    }
                    catch
                    {
                        // Intentionally blank, skip invalid records
                    }
                }
            }
            return ascending ? candles.OrderBy(c => c.DateTime).ToList() : candles.OrderByDescending(c => c.DateTime).ToList();
        }

        static async Task<Stream> GetResponseStreamAsync(string symbol, Period period = Period.Daily, DateTime? startTime = null, DateTime? endTime = null, SkipOption skipOption = SkipOption.None, CancellationToken token = default(CancellationToken))
            => await StooqUrl
                .SetQueryParam(SymbolTag, symbol)
                .SetQueryParam(PeriodTag, period.Name())
                .SetQueryParam(StartTimeTag, startTime?.ToString("yyyyMMdd"))
                .SetQueryParam(EndTimeTag, endTime?.ToString("yyyyMMdd"))
                .SetQueryParam(SkipTag, skipOption == SkipOption.None ? null : Convert.ToString((int)skipOption, 2))
                .SetQueryParamBySkipOption(skipOption)
                .GetAsync(token)
                .ReceiveStream()
                .ConfigureAwait(false);


        static Url SetQueryParamBySkipOption(this Url url, SkipOption skipOption)
        {
            Enum.GetValues(typeof(SkipOption))
                .Cast<SkipOption>()
                .Where(so => !so.Equals(SkipOption.None) && skipOption.HasFlag(so))
                .ToList()
                .ForEach(so => url.SetQueryParam(so.Name(), "1"));

            return url;
        }

        static string Name<T>(this T @enum)
		{
			string name = @enum.ToString();
			if (typeof(T).GetMember(name).First().GetCustomAttribute(typeof(EnumMemberAttribute)) is EnumMemberAttribute attr && attr.IsValueSetExplicitly)
				name = attr.Value;
			return name;
		}
	}
}
