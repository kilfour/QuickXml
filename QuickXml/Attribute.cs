using QuickXml.Speak;
using QuickXml.UnderTheHood;
using QuickXml.XmlStructure;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<string> Attribute(this XmlParser<XmlParseNode> parser, string attributeName)
		{
			return
				state =>
					{
						var result = parser(state);
						return result.Value.Attribute(attributeName)(state);
					};
		}
	}
}
