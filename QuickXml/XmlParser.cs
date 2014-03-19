using QuickXml.UnderTheHood;

namespace QuickXml
{
	public delegate IXmlParserResult<T> XmlParser<out T>(XmlParserState state);
}