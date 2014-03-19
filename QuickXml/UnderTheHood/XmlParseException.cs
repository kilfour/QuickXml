using System;

namespace QuickXml.UnderTheHood
{
	public class XmlParseException : Exception
	{
		public XmlParseException(string message)
			:base(message) { }

		public XmlParseException(string message, Exception innerException)
			: base(message, innerException) { }
	}
}