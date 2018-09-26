using Xunit;

namespace QuickXml.Tests
{
    public class Content
    {
        [Fact]
        public void WhenItsThere()
        {
            const string input = "<root><child>test</child></root>";
            var xmlParser = XmlParse.Child("child").Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("test", result);
        }

        [Fact]
        public void WhenItsNotThere()
        {
            const string input = "<root><child /></root>";
            var xmlParser = XmlParse.Root().Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("", result);
        }

        [Fact]
        public void WhenItsNotThereAgain()
        {
            const string input = "<root><child></child></root>";
            var xmlParser = XmlParse.Root().Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("", result);
        }

        [Fact]
        public void ParserNodeContent()
        {
            const string input = "<root><child><text>test</text></child></root>";
            var xmlParser = XmlParse.Root().Child("child").Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("", result);
        }

        [Fact]
        public void NodeContent()
        {
            const string input = "<root><child>test</child></root>";
            var xmlParser =
                from child in XmlParse.Root().Child("child")
                from content in child.Content()
                select content;
            var result = xmlParser.Parse(input);
            Assert.Equal("test", result);
        }

        [Fact]
        public void WhiteSpace()
        {
            const string input = "<root><child> test  </child></root>";
            var xmlParser = XmlParse.Root().Child("child").Content();
            var result = xmlParser.Parse(input);
            Assert.Equal(" test  ", result);
        }
    }
}