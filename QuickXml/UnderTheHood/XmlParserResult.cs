namespace QuickXml.UnderTheHood
{
	public interface IXmlParserResult<out T>
	{
		T Value { get; }
		XmlParserState State { get; }
		bool WasSuccessFull { get; }
	}

	public class XmlParserResult<T> : IXmlParserResult<T>
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
}