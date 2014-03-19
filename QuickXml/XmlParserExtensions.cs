using Sprache;

namespace QuickXml
{
	public static class XmlParserExtensions
	{
		public static T Parse<T>(this XmlParser<T> parser,string input)
		{
			var document = DocumentParser.Document.Parse(input);
			var state = new XmlParserState {Document = document, Current = document.Root};
			var result = parser(state);
			return result.Value;
		}
	}
}