using System;

namespace QuickXml
{
	public static class XmlParserToLinq
	{
		public static XmlParser<TValueTwo> Select<TValueOne, TValueTwo>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, TValueTwo> selector)
		{
			if (xmlParser == null)
				throw new ArgumentNullException("xmlParser");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return
				(i, s) =>
					{
						var result = xmlParser(i, s);
						return new XmlResult<TValueTwo>(selector(result.Value), result.Remainder, result.WasSuccessful);
					};
		}

		public static XmlParser<TValueTwo> SelectMany<TValueOne, TValueTwo>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, XmlParser<TValueTwo>> selector)
		{
			if (xmlParser == null)
				throw new ArgumentNullException("xmlParser");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return (i, s) =>
			       	{
			       		var result = xmlParser(i, s);
						return selector(result.Value)(result.Remainder, s);
			       	};
		}

		public static XmlParser<TValueThree> SelectMany<TValueOne, TValueTwo, TValueThree>(
			this XmlParser<TValueOne> xmlParser,
			Func<TValueOne, XmlParser<TValueTwo>> selector,
			Func<TValueOne, TValueTwo, TValueThree> projector)
		{
			if (xmlParser == null)
				throw new ArgumentNullException("xmlParser");
			if (selector == null)
				throw new ArgumentNullException("selector");
			if (projector == null)
				throw new ArgumentNullException("projector");

			return xmlParser.SelectMany(x => selector(x).Select(y => projector(x, y)));
		}

	}
}