using System.Collections.Generic;
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

	    [Fact]
	    public void Complex()
	    {
	        const string input =
@"<root>
    <first>
        <second><third>some text 1</third></second>
        <second><third>some text 2</third></second>
    </first>
    <first><second><third>some text 3</third></second></first>
</root>";
	        var contentParser =
	            from second in XmlParse.Child("second")
	            from third in second.Child("third").Content()
	            select third;

            var xmlParser =
	            from first in XmlParse.Child("first")
	            from content in first.Apply(contentParser.Many())
                select content;

	        var result = xmlParser.Many().Parse(input).SelectMany(x => x).ToArray();

	        Assert.Equal("some text 1", result[0]);
	        Assert.Equal("some text 2", result[1]);
	        Assert.Equal("some text 3", result[2]);
        }

        [Fact]
        public void Example()
        {
            const string input = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<GeefProductResponse><product><productId>148</productId><links><link><url><![CDATA[http://stuff]]></url><titel><![CDATA[titel]]></titel><type>hyperlink</type></link></links></product></GeefProductResponse>";

            var linkParser =
                from link in XmlParse.Child("link")
                from url in link.Child("url").Content()
                from titel in link.Child("titel").Content()
                select new Dictionary<string, string>
                {
                    {"url", url},
                    {"titel", titel}
                };

            var xmlParser =
                from root in XmlParse.Root()
                from product in root.Child("product")
                from links in product.Child("links").Apply(linkParser.Many())
                select links;

            var result = xmlParser.Parse(input).ToArray();

            Assert.Equal(1, result.Count());
            Assert.Equal("http://stuff", result[0]["url"]);
            Assert.Equal("titel", result[0]["titel"]);
        }
	}
}