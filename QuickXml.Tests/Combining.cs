using System.Linq;
using Xunit;

namespace QuickXml.Tests
{
	public class Combining
	{
		private readonly XmlParser<string> parserOne =
			from first in XmlParse.Child("first").Content()
			select first;

		private readonly XmlParser<string> parserTwo =
			from first in XmlParse.Child("second").Content()
			select first;

		[Fact]
		public void First()
		{
			const string input = "<root><first>test</first></root>";
			var result = parserOne.Parse(input);
			Assert.Equal("test", result);
		}

		[Fact]
		public void Second()
		{
			const string input = "<root><second>test</second></root>";
			var result = parserTwo.Parse(input);
			Assert.Equal("test", result);
		}

		[Fact]
		public void Combined()
		{
			const string input = "<root><first>test</first><second>test again</second></root>";
			var parser =
				from root in XmlParse.Root()
				from first in root.Apply(parserOne)
				from second in root.Apply(parserTwo)
				select new[] { first, second };

			var result = parser.Parse(input);

			Assert.Equal("test", result[0]);
			Assert.Equal("test again", result[1]);
		}

		[Fact]
		public void CombinedAgain()
		{
			const string input = "<root><first>test</first><aha><second>test again</second></aha></root>";
			var parser =
				from root in XmlParse.Root()
				from first in root.Apply(parserOne)
				from aha in root.Apply(XmlParse.Child("aha"))
				from second in aha.Apply(parserTwo)
				select new[] { first, second };

			var result = parser.Parse(input);

			Assert.Equal("test", result[0]);
			Assert.Equal("test again", result[1]);
		}

		[Fact]
		public void WithAMany()
		{
			const string input = "<root><first>test</first><first>test again</first></root>";
			var parser =
				from root in XmlParse.Root()
				from first in root.Apply(parserOne.Many())
				select first.ToArray();

			var result = parser.Parse(input);

			Assert.Equal("test", result[0]);
			Assert.Equal("test again", result[1]);
		}
	}
}