using System.Linq;
using QuickXml.UnderTheHood;
using Xunit;

namespace QuickXml.Tests
{
	public class Spike
	{
		[Fact]
		public void Content()
		{
			const string input = "<root>test</root>";
			var xmlParser = XmlParse.Root().Content();
			var result = xmlParser.Parse(input);
			Assert.Equal("test", result);
		}

		[Fact]
		public void Single()
		{
			const string input = "<root><first>a first</first></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content()
				select first;

			var result = xmlParser.Parse(input);
			Assert.Equal("a first", result);
		}

		[Fact]
		public void TwoNodes()
		{
			const string input = "<root><first>a first</first><second>a second</second></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content()
				from second in XmlParse.Child("second").Content()
				select new[] { first, second };

			var result = xmlParser.Parse(input);
			Assert.Equal("a first", result[0]);
			Assert.Equal("a second", result[1]);
		}

		public class Result
		{
			public string First { get; set; }
			public string Second { get; set; }
		}

		[Fact]
		public void TwoNodesWrongWayRound()
		{
			const string input = "<root><second>a second</second><first>a first</first></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content()
				from second in XmlParse.Child("second").Content()
				select
					new Result
					{
						First = first,
						Second = second
					};

			var result = xmlParser.Parse(input);
			Assert.Equal("a first", result.First);
			Assert.Equal("a second", result.Second);
		}

		[Fact]
		public void Nested()
		{
			const string input = "<root><first><second>a second</second></first></root>";

			var xmlParser =
				from first in XmlParse.Child("first")
				from second in first.Child("second").Content()
				select second;

			var result = xmlParser.Parse(input);
			Assert.Equal("a second", result);
		}

		[Fact]
		public void NestedAndContent()
		{
			const string input = "<root><first><second>a second</second></first><third>third</third></root>";

			var xmlParser =
				from root in XmlParse.Root()
				from first in root.Child("first")
				from second in first.Child("second").Content()
				from third in root.Child("third").Content()
				select new[] { second, third };

			var result = xmlParser.Parse(input);
			Assert.Equal("a second", result[0]);
			Assert.Equal("third", result[1]);
		}

		[Fact]
		public void Many()
		{
			const string input = "<root><first>a first</first><first>another first</first></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content().Many()
				select first.ToArray();

			var result = xmlParser.Parse(input);
			Assert.Equal("a first", result[0]);
			Assert.Equal("another first", result[1]);
		}

		[Fact]
		public void AttributeAlt()
		{
			const string input = "<root anAttribute=\"test\"></root>";

			var xmlParser =
				from attr in XmlParse.Attribute("anAttribute")
				select attr;

			var result = xmlParser.Parse(input);
			Assert.Equal("test", result);
		}

		[Fact]
		public void IntAttribute()
		{
			const string input = "<root anAttribute=\"42\"></root>";

			var xmlParser =
				from attr in XmlParse.Attribute("anAttribute").Int()
				select attr;

			var result = xmlParser.Parse(input);
			Assert.Equal(42, result);
		}

		[Fact]
		public void ShortNodeAttribute()
		{
			const string input = "<root anAttribute=\"test\" />";

			var xmlParser =
				from attr in XmlParse.Attribute("anAttribute")
				select attr;

			var result = xmlParser.Parse(input);
			Assert.Equal("test", result);
		}

		[Fact]
		public void ChildFailureThrows()
		{
			const string input = "<root></root>";

			var xmlParser =
				from first in XmlParse.Child("first")
				select first;

			Assert.Throws<XmlParserException>(() => xmlParser.Parse(input));
		}

		[Fact]
		public void ContentFailureThrows()
		{
			const string input = "<root></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content()
				select first;

			Assert.Throws<XmlParserException>(() => xmlParser.Parse(input));
		}

		[Fact]
		public void ChildOrDefault()
		{
			const string input = "<root></root>";

			var xmlParser =
				from first in XmlParse.Child("first").OrDefault()
				select first;

			var result = xmlParser.Parse(input);
			Assert.Null(result);
		}

		[Fact]
		public void ContentOrDefault()
		{
			const string input = "<root></root>";

			XmlParser<string> xmlParser =
				from first in XmlParse.Child("first").Content().OrDefault()
				select first;

			var result = xmlParser.Parse(input);
			Assert.Null(result);
		}

		[Fact]
		public void OrValue()
		{
			const string input = "<root></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content().Or("Test")
				select first;

			var result = xmlParser.Parse(input);
			Assert.Equal("Test", result);
		}

		[Fact]
		public void ContentAsInt()
		{
			const string input = "<root><first>42</first></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content().Int()
				select first;

			var result = xmlParser.Parse(input);
			Assert.Equal(42, result);
		}

		[Fact]
		public void ContentAsIntFails()
		{
			const string input = "<root><first>bla</first></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content().Int()
				select first;

			Assert.Throws<XmlParserException>(() => xmlParser.Parse(input));
		}

		[Fact]
		public void AsIntOrDefault()
		{
			const string input = "<root><first>bla</first></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content().Int().OrDefault()
				select first;

			var result = xmlParser.Parse(input);
			Assert.Equal(0, result);
		}

		[Fact]
		public void AsIntOrValue()
		{
			const string input = "<root><first>bla</first></root>";

			var xmlParser =
				from first in XmlParse.Child("first").Content().Int().Or(42)
				select first;

			var result = xmlParser.Parse(input);
			Assert.Equal(42, result);
		}
	}
}