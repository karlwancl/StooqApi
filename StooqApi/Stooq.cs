﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Flurl;
using Flurl.Http;

namespace StooqApi
{
    public static class Stooq
    {
        static readonly DateTime DefaultStartTime = new DateTime(1000, 1, 1);
        const string StooqUrl = "https://stooq.com/q/d/l/";
        const string SymbolTag = "s";
        const string PeriodTag = "i";
        const string StartTimeTag = "d1";
        const string EndTimeTag = "d2";
        const string SkipTag = "o";
        public static CultureInfo Culture = CultureInfo.InvariantCulture;

        public static async Task<IList<Candle>> GetHistoricalAsync(string symbol, Period period = Period.Daily, DateTime? startTime = null, DateTime? endTime = null, SkipOption skipOption = SkipOption.None, bool ascending = false, CancellationToken token = default(CancellationToken))
        {
			string text;

			using (var s = await GetResponseStreamAsync(symbol, period, startTime, endTime, skipOption, token).ConfigureAwait(false))
            using (var sr = new StreamReader(s))
            {
				text = await sr.ReadToEndAsync();

				const string validHeader = "Date";
                if (!text.StartsWith(validHeader, StringComparison.OrdinalIgnoreCase))
                    throw new Exception(string.IsNullOrEmpty(text) ? "Source csv does not contain any text" : text);
            }

			var candles = new List<Candle>();

            //bool isBypassedHeader = false;
			using (var tsr = new StringReader(text))
            using (var csvReader = new CsvReader(tsr, new CsvConfiguration(Culture) { HasHeaderRecord = true }))
            {
                while (csvReader.Read())
                {
                    // CsvHelper's HasHeaderRecord not working v6.0.3
                    //if (!isBypassedHeader)
                    //{
                    //    isBypassedHeader = true;
                    //    continue;
                    //}

                    string[] row = csvReader.Context.Record;
                    try
                    {
                        decimal volume = (row.Count() <= 5 || string.IsNullOrWhiteSpace(row[5])) ? 0 : Convert.ToDecimal(row[5], CultureInfo.InvariantCulture);
                        candles.Add(new Candle(
                            Convert.ToDateTime(row[0], CultureInfo.InvariantCulture),
                            Convert.ToDecimal(row[1], CultureInfo.InvariantCulture),
                            Convert.ToDecimal(row[2], CultureInfo.InvariantCulture),
                            Convert.ToDecimal(row[3], CultureInfo.InvariantCulture),
                            Convert.ToDecimal(row[4], CultureInfo.InvariantCulture),
                            volume
                        ));
                    }
                    catch (Exception ex)
                    {
                        // Intentionally blank, skip invalid records
                    }
                }
            }

            return candles.OrderBy(c => c.DateTime, new DateTimeComparer(ascending)).ToList();
        }

        static async Task<Stream> GetResponseStreamAsync(string symbol, Period period = Period.Daily, DateTime? startTime = null, DateTime? endTime = null, SkipOption skipOption = SkipOption.None, CancellationToken token = default(CancellationToken))
            => await StooqUrl
                .SetQueryParam(SymbolTag, symbol, true)
                .SetQueryParam(StartTimeTag, (startTime ?? DefaultStartTime).ToString("yyyyMMdd"))
                .SetQueryParam(EndTimeTag, (endTime ?? DateTime.Now).ToString("yyyyMMdd"))
                .SetQueryParam(PeriodTag, period.Name())
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
