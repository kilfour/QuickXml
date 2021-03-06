﻿using Xunit;

namespace QuickXml.Tests.Speak
{
	public class StringWithXmlHeader
	{
		[Fact]
		public void CanBeParsed()
		{
			const string input = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root>test</root>";
			var xmlParser = XmlParse.Root().Content();
			var result = xmlParser.Parse(input);
			Assert.Equal("test", result);
		}
	}
}
