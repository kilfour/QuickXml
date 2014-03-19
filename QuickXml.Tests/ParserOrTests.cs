using QuickXml.UnderTheHood;
using Xunit;

namespace QuickXml.Tests
{
	public class ParserOrTests
	{
		[Fact]
		public void FirstOneSucceeds()
		{
			const string input = "<root><first>some text</first></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Or(XmlParse.Child("second")).Content()
				select val;

			var result = xmlParser.Parse(input);
			Assert.Equal("some text", result);
		}

		[Fact]
		public void SecondOneSucceeds()
		{
			const string input = "<root><second>some text</second></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Or(XmlParse.Child("second")).Content()
				select val;

			var result = xmlParser.Parse(input);
			Assert.Equal("some text", result);
		}

		[Fact]
		public void BothFail()
		{
			const string input = "<root><third>some text</third></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Or(XmlParse.Child("second")).Content()
				select val;

			Assert.Throws<XmlParserException>(() => xmlParser.Parse(input));
		}

		[Fact]
		public void ContentParserOr()
		{
			const string input = "<root><second>some text</second></root>";

			var xmlParser =
				from val in XmlParse.Child("first").Content().Or(XmlParse.Child("second").Content())
				select val;

			var result = xmlParser.Parse(input);
			Assert.Equal("some text", result);
		}

		[Fact]
		public void NullReferenceBug()
		{
			const string input = "<root><second>some text</second></root>";

			var nullParser =
				from el1 in XmlParse.Child("first")
				from content in el1.Child("somethingElse").Content()
				select content;


			var xmlParser =
				from val in nullParser.Or(XmlParse.Child("second").Content())
				select val;

			var result = xmlParser.Parse(input);
			Assert.Equal("some text", result);
		}
	}
}