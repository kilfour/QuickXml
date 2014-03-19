using System;
using System.Collections.Generic;
using System.Linq;
using QuickXml.UnderTheHood;
using Xunit;

namespace QuickXml.Tests
{
	public class Example
	{
		[Fact]
		public void ParseCorrectSuccess()
		{
			const string xml = 
@"<ScheduleTimeSeries>
    <InArea codingScheme='A01' v='10YFR-RTE------C'/>
    <OutArea codingScheme='A01' v='10YBE----------2'/>
    <Period>
        <TimeInterval v='2014-01-29T23:00Z/2014-01-30T23:00Z'/>
    </Period>
</ScheduleTimeSeries>";

			var periodParser =
				from period in XmlParse.Child("Period")
				from interval in period.Child("TimeInterval")
				from date in interval.Attribute("v").DateSplit()
				select
					new InputLTAXmlPeriodDto
						{
							Date = date
						};

			var aParser =
				from root in XmlParse.Root()
				from inArea in root.Child("InArea").Attribute("v")
				from outArea in root.Child("OutArea").Attribute("v")
				from period in root.Apply(periodParser)
				select
					new InputLTAXmlScheduleTimeSeriesDto
						{
							InAreaHubEIC = inArea,
							OutAreaHubEIC = outArea,
							Period = period
						};

			var result = aParser.Parse(xml);
			Assert.Equal("10YFR-RTE------C", result.InAreaHubEIC);
			Assert.Equal("10YBE----------2", result.OutAreaHubEIC);
			Assert.Equal(new DateTime(2014, 01, 29, 23, 0, 0, DateTimeKind.Utc), result.Period.Date.ToUniversalTime());
		}

		public class InputLTAXmlScheduleTimeSeriesDto
		{
			public string InAreaHubEIC { get; set; }
			public string OutAreaHubEIC { get; set; }
			public InputLTAXmlPeriodDto Period { get; set; }
		}

		public class InputLTAXmlPeriodDto
		{
			public InputLTAXmlPeriodDto()
			{
				Intervals = new List<InputLTAXmlIntervalDto>();
			}

			public List<InputLTAXmlIntervalDto> Intervals { get; set; }
			public DateTime Date { get; set; }
		}

		public class InputLTAXmlIntervalDto
		{
			public int Position { get; set; }
			public decimal LTA { get; set; }
		}
	}

	public static class CustomExtension
	{
		public static XmlParser<DateTime> DateSplit(this XmlParser<string> parser)
		{
			return
				state =>
				{
					var result = parser(state);
					if (result.WasSuccessFull)
					{
						var value = DateTime.Parse(result.Value.Split('/').First()).ToUniversalTime();
						return Result.Success(value, state);
					}
					return Result.Failure<DateTime>(state);
				};
		}
	}
}