using Xunit;

namespace QuickXml.Tests.Speak
{
    public class InvalidXml
    {
        [Fact]
        public void WeAcceptTagsInContent()
        {
            const string input = "<root>t<u>es</u>t</root>";
            var xmlParser = XmlParse.Root().Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("t<u>es</u>t", result);
        }

        [Fact]
        public void EvenStartingOnes()
        {
            const string input = "<root><u>tes</u>t</root>";
            var xmlParser = XmlParse.Root().Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("<u>tes</u>t", result);
        }
    }
}