using System;
using QuickXml.Speak;
using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static class XmlParserToLinq
	{
		public static XmlParser<TValueTwo> Select<TValueOne, TValueTwo>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, TValueTwo> selector)
		{
			return state =>
			       	{
			       		var result = xmlParser(state);
						if (!result.WasSuccessFull)
						{
							if (state.UseNullNode)
							{
								if (typeof(TValueOne) == typeof(Node))
								{
									var value = ((TValueOne)((object)new NullNode()));
									return new XmlParserResult<TValueTwo>(selector(value), state, false);
								}
							}
							else if (!state.DontThrowFailures)
								throw new XmlParserException("Catastrophic Failure");
						}
			       		return new XmlParserResult<TValueTwo>(selector(result.Value), state, result.WasSuccessFull);
			       	};
		}

		public static XmlParser<TValueTwo> SelectMany<TValueOne, TValueTwo>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, XmlParser<TValueTwo>> selector)
		{
			return state =>
			       	{
			       		var result = xmlParser(state);
						if (!result.WasSuccessFull && !state.DontThrowFailures)
						{
							if(state.UseNullNode)
							{
								if(typeof(TValueOne) == typeof(Node))
								{
									var value = ((TValueOne) ((object) new NullNode()));
									return selector(value)(state);
								}
							}
							else if (!state.DontThrowFailures)
								throw new XmlParserException("Catastrophic Failure");	
						}
			       		return selector(result.Value)(state);
			       	};
		}

		public static XmlParser<TValueThree> SelectMany<TValueOne, TValueTwo, TValueThree>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, XmlParser<TValueTwo>> selector,
			Func<TValueOne, TValueTwo, TValueThree> projector)
		{
			return xmlParser.SelectMany(x => selector(x).Select(y => projector(x, y)));
		}
	}
}