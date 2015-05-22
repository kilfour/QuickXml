using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
        public static XmlParser<string> Attribute(string attributeName)
        {
            return state =>
                state.Current.Attribute(attributeName)(state);
        }

		public static XmlParser<string> Attribute(this XmlParser<XmlParserNode> parser, string attributeName)
		{
			return state => 
                parser(state).Value.Attribute(attributeName)(state);
		}
	}
}
