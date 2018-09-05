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
		    {
		        var result = parser(state);
		        if (result.Value == null)
		            return Result.Failure<string>(state);
		        return result.Value.Attribute(attributeName)(state);
		    };
		}
	}
}
