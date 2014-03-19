using System;
using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static class XmlParserToLinq
	{
		public static XmlParser<TValueTwo> Select<TValueOne, TValueTwo>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, TValueTwo> selector)
		{
			return
				state =>
					{
						var result = xmlParser(state);
						var value = HandleFailure(state, result);
						return new XmlParserResult<TValueTwo>(selector(value), state, result.WasSuccessFull);
					};
		}

		public static XmlParser<TValueTwo> SelectMany<TValueOne, TValueTwo>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, XmlParser<TValueTwo>> selector)
		{
			return
				state =>
					{
						var result = xmlParser(state);
						var value = HandleFailure(state, result);
						return selector(value)(state);
					};
		}

		public static XmlParser<TValueThree> SelectMany<TValueOne, TValueTwo, TValueThree>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, XmlParser<TValueTwo>> selector,
			Func<TValueOne, TValueTwo, TValueThree> projector)
		{
			return xmlParser.SelectMany(x => selector(x).Select(y => projector(x, y)));
		}

		private static T HandleFailure<T>(
			XmlParserState state,
			IXmlParserResult<T> result)
		{
			if (!result.WasSuccessFull)
			{
				if (state.UseNullNode)
				{
					if (typeof(T) == typeof(XmlParseNode))
					{
						var value = ((T)((object)new XmlParseNullNode()));
						return value;
					}
				}
				else if (!state.DontThrowFailures)
					throw new XmlParserException("Catastrophic Failure");
			}
			return result.Value;
		}
	}
}