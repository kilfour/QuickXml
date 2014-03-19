using System.Collections.Generic;
using System.Linq;

namespace QuickXml
{

	public static class XmlParse
	{
		public static XmlParser<Node> Root()
		{
			return state =>
			       	{
			       		state.Current = state.Document.Root;
			       		return Result.Success(state.Current, state);
			       	};
		}

		public static XmlParser<string> Attribute(this XmlParser<Node> parser, string attributeName)
		{
			return
				state =>
					{
						var result = parser(state);
						return result.Value.Attribute(attributeName)(state);
					};
		}
		public static XmlParser<Node> Child(string tagName)
		{
			return
				state =>
					{
						Node child;
						var hasChild = state.NextChild(tagName, out child);
						if (hasChild)
						{
							//state.Current = child;
							return Result.Success(child, state);
						}
						return Result.Failure<Node>(state);
					};
		}



		public static XmlParser<string> Content(this XmlParser<Node> parser)
		{
			return
				state =>
					{
						var result = parser(state);
						if(result.WasSuccessFull)
						{
							var content = ((Content)result.Value.Children.Single()).Text;
							return Result.Success(content, state);	
						}
						return Result.Failure<string>(state);
					};
		}

		public static XmlParser<IEnumerable<T>> Many<T>(this XmlParser<T> parser)
		{
			return state =>
			{
				var list = new List<T>();
				var success = true;
				while (success)
				{
					var result = parser(state);
					list.Add(result.Value);
					success = result.WasSuccessFull;
				}
				return Result.Success<IEnumerable<T>>(list, state);
			};
		}

		public static XmlParser<int> Int(this XmlParser<string> parser)
		{
			return
				state =>
					{
						var result = parser(state);
						if(result.WasSuccessFull)
						{
							int value;
							if (int.TryParse(result.Value, out value))
								return Result.Success(value, state);
							throw new XmlParseException(
								string.Format(
									"'{0}' is not an int.",
									result.Value));
						}
						return Result.Failure<int>(state);
					};
		}

		public static XmlParser<decimal> Decimal(this XmlParser<string> parser)
		{
			return
				state =>
				{
					var result = parser(state);
					if (result.WasSuccessFull)
					{
						decimal value;
						if (decimal.TryParse(result.Value, out value))
							return Result.Success(value, state);
						throw new XmlParseException(
							string.Format(
								"'{0}' is not a decimal.",
								result.Value));
					}
					return Result.Failure<decimal>(state);
				};
		}

		public static XmlParser<T> OrDefault<T>(this XmlParser<T> parser)
		{
			return
				state =>
				{
					var result = parser(state);
					if (result.WasSuccessFull)
					{
						return result;
					}
					return Result.Success(default(T), state);
				};
		}

		public static XmlParser<T> Or<T>(this XmlParser<T> parser, T value)
		{
			return
				state =>
				{
					var result = parser(state);
					if (result.WasSuccessFull)
					{
						return result;
					}
					return Result.Success(value, state);
				};
		}
	}
}
