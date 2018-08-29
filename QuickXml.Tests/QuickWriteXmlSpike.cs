using System.IO;
using QuickXml.Speak;
using Sprache;
using Xunit;

namespace QuickXml.Tests
{
    public class QuickWriteXmlSpike
    {
        [Fact]
        public void Content()
        {
            var input = File.ReadAllText("Test.xml");
            var document = DocumentParser.Document.Parse(input);
            File.WriteAllText("code.txt", document.Root.ToQuickXmlWrite(null, 0));
        }
    }
}