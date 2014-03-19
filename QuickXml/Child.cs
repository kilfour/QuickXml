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
						if (hasChild)
							return Result.Success(child, state);
						return Result.Failure<Node>(state);
					};
		}
	}
}
