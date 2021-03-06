﻿using System.Linq;
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

		[Fact]
		public void ManyOr()
		{
			const string input = "<root><first>some text</first><second>some other text</second></root>";

			var xmlParser =
				from content in XmlParse.Child("first").Or(XmlParse.Child("second")).Content().Many()
				select content;

			var result = xmlParser.Parse(input).ToArray();
			Assert.Equal("some text", result[0]);
			Assert.Equal("some other text", result[1]);
		}
	}
}