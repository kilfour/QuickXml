using System;
using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<Guid> Guid(this XmlParser<string> parser)
		{
			return state => parser(state).IfSuccessfull(result => TryParseGuid(result, state));
		}

		private static IXmlParserResult<Guid> TryParseGuid(IXmlParserResult<string> result, XmlParserState state)
		{
			Guid value;
			return System.Guid.TryParse(result.Value, out value) ? Result.Success(value, state) : Result.Failure<Guid>(state);
		}
	}
}
