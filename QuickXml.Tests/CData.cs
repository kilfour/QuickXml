using Xunit;

namespace QuickXml.Tests
{
    public class CData
    {
        [Fact]
        public void Content()
        {
            const string input = "<root><![CDATA[test]]></root>";
            var xmlParser = XmlParse.Root().Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("test", result);
        }

        [Fact]
        public void WithTags()
        {
            const string input = "<root><![CDATA[t<b>es</b>t]]></root>";
            var xmlParser = XmlParse.Root().Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("t<b>es</b>t", result);
        }
    }
}