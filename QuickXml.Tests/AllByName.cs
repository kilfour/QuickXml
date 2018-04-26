using System.Linq;
using Xunit;

namespace QuickXml.Tests
{
    public class AllByName
    {
        [Fact]
        public void Content()
        {
            const string input =
@"<root>
    <tag1><target>target1</target></tag1>
    <tag2><target>target2</target></tag2>
    <tag2>
        <tag3><target>target3</target></tag3>
    </tag2>
</root>";
            var xmlParser = XmlParse.All("target").Content().Many();
            var result = xmlParser.Parse(input).ToArray();
            Assert.Equal("target1", result[0]);
            Assert.Equal("target2", result[1]);
            Assert.Equal("target3", result[2]);
        }
    }
}