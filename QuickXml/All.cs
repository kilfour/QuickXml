using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<XmlParserNode> All(string tagName)
		{
			return state => 
                state.Current.All(tagName)(state);
		}

		public static XmlParser<XmlParserNode> All(this XmlParser<XmlParserNode> parser, string tagName)
		{
			return state => 
                parser(state).IfSuccessfull(result => result.Value.All(tagName)(state));
		}
	}
}
