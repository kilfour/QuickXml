using QuickXml.Speak;
using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<Node> Child(string tagName)
		{
			return
				state =>
					{
						Node child;
						var hasChild = state.NextChild(tagName, out child);
						return hasChild ? Result.Success(child, state) : Result.Failure<Node>(state);
					};
		}

		public static XmlParser<Node> Child(this XmlParser<Node> xmlParser, string tagName)
		{
			return
				state =>
					{
						var result = xmlParser(state);
						return result.WasSuccessFull ? result.Value.Child(tagName)(state) : Result.Failure<Node>(state);
					};
		}
	}
}
