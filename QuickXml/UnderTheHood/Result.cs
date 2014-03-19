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
	}
}