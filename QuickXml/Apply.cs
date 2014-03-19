using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<T> Apply<T>(this XmlParser<XmlParserNode> parser, XmlParser<T> other)
		{
			return state => parser(state).IfSuccessfull(result => result.Value.Apply(other)(state));
		}
	}
}
