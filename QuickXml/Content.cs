using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<string> Content(this XmlParser<XmlParserNode> parser)
		{
			return state => parser(state).IfSuccessfull(result => result.Value.GetContent(state));
		}
	}
}
