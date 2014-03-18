using System;
using Sprache;

namespace QuickXml
{
	public interface IXmlResult<out TOut>
	{
		TOut Value { get; }
		Input Remainder { get; }
		bool WasSuccessful { get; }
	}

	public class XmlResult<TOut> : IXmlResult<TOut>
	{
		public TOut Value { get; set; }
		public Input Remainder { get; set; }
		public bool WasSuccessful { get; set; }
		public XmlResult() { }
		public XmlResult(TOut value, Input rest, bool wasSuccessful) 
		{ 
			Value = value; 
			Remainder = rest;
			WasSuccessful = wasSuccessful;
		}
	}
}