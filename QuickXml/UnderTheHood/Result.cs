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

		public static IXmlParserResult<TOut> IfSuccessfull<TIn, TOut>(
			this IXmlParserResult<TIn> result,
			Func<IXmlParserResult<TIn>, IXmlParserResult<TOut>> func)
		{
			if (result.WasSuccessFull)
			{
				return func(result);
			}
			return Failure<TOut>(result.State);
		}

		public static IXmlParserResult<T> WhenFailed<T>(
			this IXmlParserResult<T> result,
			Func<IXmlParserResult<T>, IXmlParserResult<T>> func)
		{
			if (result.WasSuccessFull)
				return result;
			return func(result);
		} 
	}
}