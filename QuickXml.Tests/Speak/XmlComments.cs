using Xunit;

namespace QuickXml.Tests.Speak
{
    public class XmlComments
    {
        [Fact]
        public void GetIgnoredOnParse()
        {
            const string input = "<!-- comment --><root>test</root>";
            var xmlParser = XmlParse.Root().Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("test", result);
        }

        [Fact(Skip="Not Yet")]
        public void InnerOnesToo()
        {
            const string input = "<root><!-- c1 -->te<!-- c2 -->st<!-- c3 --></root>";
            var xmlParser = XmlParse.Root().Content();
            var result = xmlParser.Parse(input);
            Assert.Equal("test", result);
        }
    }
}