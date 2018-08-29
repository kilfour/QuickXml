using QuickXml.Speak;
using Sprache;
using Xunit;

namespace QuickXml.Tests.Speak
{
    public class HeaderTests
    {
        [Fact]
        public void NoHeader()
        {
            const string input = "<root/>";
            var document = DocumentParser.Document.Parse(input);

            Assert.Null(document.Header);
        }

        [Fact]
        public void Standard()
        {
            const string input = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root/>";
            var document = DocumentParser.Document.Parse(input);

            Assert.NotNull(document.Header);
            Assert.Equal("1.0", document.Header.Version);
            Assert.Equal("utf-8", document.Header.Encoding);
            Assert.Null(document.Header.StandAlone);
        }
    }
}