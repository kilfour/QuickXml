using System;

namespace QuickXml.UnderTheHood
{
	public class XmlParserException : Exception
	{
		public XmlParserException(string message)
			:base(message) { }

		public XmlParserException(string message, Exception innerException)
			: base(message, innerException) { }
	}
}