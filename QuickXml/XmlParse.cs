using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace QuickXml
{
	public static class XmlParse
	{
		public static XmlParser<Node> Into(string tagName)
		{
			return
				(input, state) =>
					{
						var result = TagParser.StartTag(tagName)(input);
						state.Push(TagParser.EndTag(tagName));
						state.CurrentNode = result.Value;
						return new XmlResult<Node>(result.Value, result.Remainder, result.WasSuccessful);
					};
		}

		public static XmlParser<string> Attribute(string attributeName)
		{
			return
				(input, state) => 
					new XmlResult<string>(
						state.CurrentNode.StringOrDefault(attributeName), 
						input, 
						true);
		}

		public static XmlParser<object> Up()
		{
			return (input, state) => new XmlResult<object>(null, state.Pop(input), true);
		}

		public static XmlParser<string> Content(string tagName)
		{
			return
				(input, state) =>
					{
						var result = TagParser.Content(tagName)(input);
						if(result.WasSuccessful)
							return new XmlResult<string>(result.Value, result.Remainder, result.WasSuccessful);
						return new XmlResult<string>(null, result.Remainder, result.WasSuccessful);
					};
		}

		public static XmlParser<T> Or<T>(this XmlParser<T> xmlParser, XmlParser<T> other)
		{
			return
				(input, state) =>
				{
					var result = xmlParser(input, state);
					if(result.WasSuccessful)
						return new XmlResult<T>(result.Value, result.Remainder, result.WasSuccessful);
					result = other(input, state);
					return new XmlResult<T>(result.Value, result.Remainder, result.WasSuccessful);
				};
		}

		public static XmlParser<IEnumerable<T>> Many<T>(this XmlParser<T> parser)
		{
			return (input, state) =>
			       	{
			       		var innerInput = input;
			       		var success = true;
			       		var value = new List<T>();
			       		IXmlResult<T> result = null;
			       		while (success)
			       		{
			       			result = parser(innerInput, state);
			       			if (result.WasSuccessful)
			       			{
			       				innerInput = result.Remainder;
			       				value.Add(result.Value);
			       			}
			       			else
			       				success = false;
			       		}
			       		return new XmlResult<IEnumerable<T>>(value, result.Remainder, true);
			       	};
		}

		public static XmlParser<T> OrDefault<T>(this XmlParser<T> parser)
		{
			return (input, state) =>
			{
				var result = parser(input, state);
				if(result.WasSuccessful)
					return new XmlResult<T>(result.Value, result.Remainder, result.WasSuccessful);
				var parseResult = Parse.AnyChar.Except(Parse.Chars('>', '<', '/', '"')).Many()(input);
				return new XmlResult<T>(default(T), parseResult.Remainder, true);
			};
		}

		public static XmlParser<T> Or<T>(this XmlParser<T> parser, T value)
		{
			return (input, state) =>
			{
				var result = parser(input, state);
				if (result.WasSuccessful)
					return new XmlResult<T>(result.Value, result.Remainder, result.WasSuccessful);
				var parseResult = Parse.AnyChar.Except(Parse.Chars('>', '<', '/', '"')).Many()(input);
				return new XmlResult<T>(value, parseResult.Remainder, true);
			};
		}

		public static XmlParser<int> Int(this XmlParser<string> parser)
		{
			return (input, state) =>
			{
				var result = parser(input, state);
				if (result.WasSuccessful)
				{
					int i;
					if(int.TryParse(result.Value, out i))
						return new XmlResult<int>(i, result.Remainder, result.WasSuccessful);
				}

				return new XmlResult<int>(0, input, false);
			};
		}

		public static XmlParser<Action<T>> Content<T>(string tagName, Action<T, string> action)
		{
			return
				(input, state) =>
				{
					var result = TagParser.Content(tagName)(input);
					return new XmlResult<Action<T>>(t => action(t, result.Value), result.Remainder, result.WasSuccessful);
				};
		}

		public static XmlParser<T> Apply<T>(T value, params Action<T>[] actions)
		{
			return
				(input, state) =>
				{
					foreach (var action in actions)
					{
						action(value);
					}
					return new XmlResult<T>(value, input, true);
				};
		}

		public static XmlParser<T> Apply<T>(T value, params XmlParser<Action<T>>[] parsers)
		{
			return
				(input, state) =>
					{
						var actions = parsers.Skip(1).Aggregate(parsers.First(), (current, parser1) => current.Or(parser1)).Many();
						var result = actions(input, state);
						foreach (var action in result.Value)
						{
							action(value);
						}
						return new XmlResult<T>(value, result.Remainder, result.WasSuccessful);
					};
		}

		public static XmlParser<T> InAnyOrder<T>(T value, params XmlParser<Action<T>>[] parsers)
		{
			return
				(input, state) =>
				{
					var actions = parsers.Skip(1).Aggregate(parsers.First(), (current, parser1) => current.Or(parser1)).Many();
					var result = actions(input, state);
					foreach (var action in result.Value)
					{
						action(value);
					}
					return new XmlResult<T>(value, input, true);
				};
		}
	}
}