using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<XmlParserNode> Child(string tagName)
		{
			return state => 
                state.Current.Child(tagName)(state);
		}

		public static XmlParser<XmlParserNode> Child(this XmlParser<XmlParserNode> parser, string tagName)
		{
			return state => 
                parser(state).IfSuccessfull(result => result.Value.Child(tagName)(state));
		}
	}
}
