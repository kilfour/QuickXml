using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<T> OrDefault<T>(this XmlParser<T> parser)
		{
			return state => parser(state).WhenFailed(result => Result.Success(default(T), state));
		}
	}
}
