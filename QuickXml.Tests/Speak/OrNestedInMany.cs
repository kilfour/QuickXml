using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QuickXml.Tests.Speak
{
    public class OrNestedInMany
    {
        private static readonly XmlParser<string> ChildParser = 
            XmlParse.Child("child").Child("content").Optional().Content().Or("Nope");

        private static readonly XmlParser<IEnumerable<string>> XmlParser = 
            XmlParse.Root().Apply(ChildParser.Many());

        [Fact]
        public void ShouldNotCauseInfiniteLoop()
        {
            const string input = "<root><child><content>Yep</content></child></root>";
            var result = XmlParser.Parse(input).ToArray();
            Assert.Equal(1, result.Length);
            Assert.Equal("Yep", result[0]);
        }

        [Fact]
        public void ShouldNotCauseInfiniteLoopWhenMissing()
        {
            const string input = "<root><child></child></root>";
            var result = XmlParser.Parse(input).ToArray();
            Assert.Equal(1, result.Length);
            Assert.Equal("Nope", result[0]);
        }
    }
}