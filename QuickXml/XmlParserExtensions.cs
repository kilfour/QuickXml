using QuickXml.Speak;
using QuickXml.UnderTheHood;
using Sprache;

namespace QuickXml
{
	public static class XmlParserExtensions
	{
		public static T Parse<T>(this XmlParser<T> parser, string input)
		{
			var document = DocumentParser.Document.End().Parse(input);
			var state = new XmlParserState(document);
			var result = parser(state);
			return result.Value;
		}
	}
}