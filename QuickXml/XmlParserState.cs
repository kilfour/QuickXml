using System;
using System.Collections.Generic;
using Sprache;

namespace QuickXml
{
	public class XmlParserState
	{
		public Stack<Func<Input, Input>> Stack { get; set; }
		public Node CurrentNode { get; set; }

		public XmlParserState()
		{
			Stack = new Stack<Func<Input, Input>>();
		}

		public void Push<T>(Parser<T> parser)
		{
			Stack.Push(
				i =>
					{
						var result = parser(i);
						if (result.WasSuccessful)
							return result.Remainder;
						throw new ParseException(result.ToString());
					});
		}

		public Input Pop(Input input)
		{
			var func = Stack.Pop();
			var result = func(input);
			return result;
		}

		public void End(Input input)
		{
			while (Stack.Count > 0)
				input = Pop(input);
		}
	}
}