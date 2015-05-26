using QuickXml.Speak;
using QuickXml.UnderTheHood;
using Sprache;

namespace QuickXml
{
	public static class XmlParserExtensions
	{
		public static T Parse<T>(this XmlParser<T> parser, string input)
		{
			var document = DocumentParser.Document.Parse(input);
			var state = new XmlParserState(document);
			var result = parser(state);
            if(!result.WasSuccessFull)
                throw new XmlParserException("Xml Parsing Failed");
		    if (result.Value is XmlParserOptionalNode)
                return NullOrDefault.For<T>();
			return result.Value;
		}
	}
}