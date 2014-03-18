using Sprache;

namespace QuickXml
{
	public delegate IXmlResult<TOut> XmlParser<out TOut>(Input input, XmlParserState state);
}
