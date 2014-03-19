namespace QuickXml
{
	public class XmlParserResult<T>
	{
		public T Value { get; private set; }
		public XmlParserState State { get; private set; }
		public bool WasSuccessFull { get; private set; }
		public XmlParserResult(T value, XmlParserState state, bool success)
		{
			WasSuccessFull = success;
			Value = value;
			State = state;
		}
	}

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