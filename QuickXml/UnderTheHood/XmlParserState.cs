using System.Linq;
using QuickXml.XmlStructure;

namespace QuickXml.UnderTheHood
{
	public class XmlParserState
	{
		public Document Document { get; set; }
		public XmlParserNode Current { get; set; }

		public bool UseNullNode { get; set; }
		public bool DontThrowFailures { get; set; }

		public XmlParserState(Document document)
		{
			Document = document;
			Current = new XmlParserNode(document.Root);
		}
	}
}