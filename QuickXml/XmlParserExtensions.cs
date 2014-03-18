using Sprache;

namespace QuickXml
{
	public static class XmlParserExtensions
	{
		public static T Parse<T>(this XmlParser<T> parser, string input)
		{
			var state = new XmlParserState();
			var result = parser(new Input(input), state);
			state.End(result.Remainder);
			return result.Value;
		}
	}
}