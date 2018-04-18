using Xunit;

namespace QuickXml.Tests.Bugs
{
    public class WhitespaceAfterHeader
    {
        [Fact]
        public void CanParse()
        {
            const string input = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r<root>test</root>";

            var xmlParser =
                from root in XmlParse.Root()
                select 1;

            var result = xmlParser.Parse(input);
            Assert.Equal(1, result);
        }
    }
}
