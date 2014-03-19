using QuickXml.Speak;
using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<string> Attribute(this XmlParser<Node> parser, string attributeName)
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
