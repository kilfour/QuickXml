using System.Linq;
using Xunit;

namespace QuickXml.Tests
{
	public class ManyTests
	{
		[Fact]
		public void ChildContent()
		{
			const string input = "<root><first>some text</first><first>some other text</first></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Content().Many()
				select val;

			var result = xmlParser.Parse(input).ToArray();
			Assert.Equal("some text", result[0]);
			Assert.Equal("some other text", result[1]);
		}

		[Fact]
		public void GrandChildContent()
		{
			const string input = "<root><first><second>some text</second></first><first><second>some other text</second></first></root>";

			var xmlParser =
				from first in XmlParse.Child("first")
				from second in first.Child("second").Content()
				select second;

			var result = xmlParser.Many().Parse(input).ToArray();

			Assert.Equal("some text", result[0]);
			Assert.Equal("some other text", result[1]);
		}
	}
}