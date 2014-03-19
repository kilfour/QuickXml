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
				from date in period.Child("TimeInterval").Attribute("v").DateSplit()
				select
					new InputLtaXmlPeriodDto
						{
							Date = date
						};

			var aParser =
				from root in XmlParse.Root()
				from inArea in root.Child("InArea").Attribute("v")
				from outArea in root.Child("OutArea").Attribute("v")
				from period in root.Apply(periodParser)
				select
					new InputLtaXmlScheduleTimeSeriesDto
						{
							InAreaHubEic = inArea,
							OutAreaHubEic = outArea,
							Period = period
						};

			var result = aParser.Parse(xml);
			Assert.Equal("10YFR-RTE------C", result.InAreaHubEic);
			Assert.Equal("10YBE----------2", result.OutAreaHubEic);
			Assert.Equal(new DateTime(2014, 01, 29, 23, 0, 0, DateTimeKind.Utc), result.Period.Date.ToUniversalTime());
		}

		public class InputLtaXmlScheduleTimeSeriesDto
		{
			public string InAreaHubEic { get; set; }
			public string OutAreaHubEic { get; set; }
			public InputLtaXmlPeriodDto Period { get; set; }
		}

		public class InputLtaXmlPeriodDto
		{
			public InputLtaXmlPeriodDto()
			{
				Intervals = new List<InputLtaXmlIntervalDto>();
			}

			public List<InputLtaXmlIntervalDto> Intervals { get; set; }
			public DateTime Date { get; set; }
		}

		public class InputLtaXmlIntervalDto
		{
			public int Position { get; set; }
			public decimal Lta { get; set; }
		}
	}

	public static class CustomExtension
	{
		public static XmlParser<DateTime> DateSplit(this XmlParser<string> parser)
		{
			return state =>
			       parser(state)
			       	.IfSuccessfull(
			       		result => Result.Success(DateTime.Parse(result.Value.Split('/').First()).ToUniversalTime(), state));
		}
	}
}