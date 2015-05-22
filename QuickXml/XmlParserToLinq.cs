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
						var value = HandleFailure(result, state);
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
                        var value = HandleFailure(result, state);
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
			IXmlParserResult<T> result,
            XmlParserState state)
		{
		    if (typeof (T) == typeof (XmlParserNode))
		    {
		        if (!result.WasSuccessFull)
		        {
                    return ((T)((object)new XmlParserNullNode()));
		        }
		    }

		    return result.Value;
		}
	}
}