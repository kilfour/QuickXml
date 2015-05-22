using System;

namespace QuickXml.UnderTheHood
{
	public static class Result
	{
		public static XmlParserResult<T> Success<T>(T value, XmlParserState state)
		{
			return new XmlParserResult<T>(value, state, true);
		}

		public static XmlParserResult<T> Failure<T>(XmlParserState state)
		{
			return new XmlParserResult<T>(default(T), state, false);
		}

        public static XmlParserResult<T> NextFailure<T>(XmlParserState state)
        {
            return new XmlParserResult<T>(default(T), state, false, true);
        }

		public static IXmlParserResult<TOut> IfSuccessfull<TIn, TOut>(
			this IXmlParserResult<TIn> result,
			Func<IXmlParserResult<TIn>, IXmlParserResult<TOut>> func)
		{
            if (!result.WasSuccessFull)
                return NextFailure<TOut>(result.State);

            return func(result);
		}
	}
}