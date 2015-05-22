namespace QuickXml.UnderTheHood
{
	public interface IXmlParserResult<out T>
	{
		T Value { get; }
		XmlParserState State { get; }
		bool WasSuccessFull { get; }
        bool NotFirstFailure { get; }
        bool IsOption { get; }
	}

	public class XmlParserResult<T> : IXmlParserResult<T>
	{
	    public T Value { get; private set; }
		public XmlParserState State { get; private set; }
		public bool WasSuccessFull { get; private set; }
        public bool NotFirstFailure { get; private set; }
        public bool IsOption { get; set; }

	    public XmlParserResult(T value, XmlParserState state, bool success)
		{
			WasSuccessFull = success;
			Value = value;
			State = state;
		}

        public XmlParserResult(T value, XmlParserState state, bool success, bool notFirstFailure)
        {
            WasSuccessFull = success;
            Value = value;
            State = state;
            NotFirstFailure = notFirstFailure;
        }

	    public XmlParserResult<T> WithOption(bool isOption)
	    {
	        IsOption = isOption;
	        return this;
	    }
	}
}