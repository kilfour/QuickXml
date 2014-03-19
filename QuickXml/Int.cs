using System;
using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<int> Int(this XmlParser<string> parser)
		{
			return state => parser(state).IfSuccessfull(result => TryParseInt(result, state));
		}

		private static IXmlParserResult<int> TryParseInt(IXmlParserResult<string> result, XmlParserState state)
		{
			int value;
			return int.TryParse(result.Value, out value) ? Result.Success(value, state) : Result.Failure<int>(state);
		}
	}
}
