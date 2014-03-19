using Xunit;

namespace QuickXml.Tests
{
	public class ChildTests
	{
		[Fact]
		public void One()
		{
			const string input = "<root><first>some text</first></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Content()
				select val;

			var result = xmlParser.Parse(input);
			Assert.Equal("some text", result);
		}

		[Fact]
		public void GrandChild()
		{
			const string input = "<root><first><second>some text</second></first></root>";

			var xmlParser =
				from first in XmlParse.Child("first")
				from second in first.Child("second").Content()
				select second;

			var result = xmlParser.Parse(input);
			Assert.Equal("some text", result);
		}

		[Fact]
		public void GrandChildChained()
		{
			const string input = "<root><first><second>some text</second></first></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Child("second").Content()
				select val;

			var result = xmlParser.Parse(input);
			Assert.Equal("some text", result);
		}
	}
}