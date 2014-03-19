using System;

namespace QuickXml
{
	public static class XmlParserToLinq
	{
		public static XmlParser<TValueTwo> Select<TValueOne, TValueTwo>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, TValueTwo> selector)
		{
			//return s => new XmlParserResult<TValueTwo>(selector(xmlParser(s).Value), s);
			return s =>
			       	{
			       		var result = xmlParser(s);
			       		return new XmlParserResult<TValueTwo>(selector(result.Value), s, result.WasSuccessFull);
			       	};
		}

		public static XmlParser<TValueTwo> SelectMany<TValueOne, TValueTwo>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, XmlParser<TValueTwo>> selector)
		{
			return s => selector(xmlParser(s).Value)(s);
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