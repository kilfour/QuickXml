using QuickXml.UnderTheHood;
using Xunit;

namespace QuickXml.Tests
{
	public class ParserOrTests
	{
		[Fact]
		public void FirstOneSucceeds()
		{
			const string input = "<root><first>a first</first></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Or(XmlParse.Child("second")).Content()
				select val;

			var result = xmlParser.Parse(input);
			Assert.Equal("a first", result);
		}

		[Fact]
		public void SecondOneSucceeds()
		{
			const string input = "<root><second>a first</second></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Or(XmlParse.Child("second")).Content()
				select val;

			var result = xmlParser.Parse(input);
			Assert.Equal("a first", result);
		}

		[Fact]
		public void BothFail()
		{
			const string input = "<root><third>a first</third></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Or(XmlParse.Child("second")).Content()
				select val;

			Assert.Throws<XmlParserException>(() => xmlParser.Parse(input));
		}
	}
}