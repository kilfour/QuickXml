using System.Collections.Generic;
using System.Linq;
using QuickXml.XmlStructure;

namespace QuickXml.UnderTheHood
{
	public class XmlParserNode 
	{
		private readonly Node node;

		public XmlParserNode(Node node)
		{
			this.node = node;
		}

		public virtual XmlParser<string> Attribute(string attributeName)
		{
			return
				state =>
				{
					if (node.Attributes.ContainsKey(attributeName))
						return Result.Success(node.Attributes[attributeName].ToString(), state);
					return Result.Failure<string>(state);
				};
		}

		public virtual XmlParser<XmlParserNode> Child(string tagName)
		{
			return
				state =>
				{
					Node child;
					var hasChild = NextChild(tagName, out child);
					return hasChild ? Result.Success(new XmlParserNode(child), state) : Result.Failure<XmlParserNode>(state);
				};
		}

		public virtual XmlParserResult<string> GetContent(XmlParserState state)
		{
			var content = ((Content) node.Children.Single()).Text;
			return Result.Success(content, state);
		}

		public virtual XmlParser<T> Apply<T>(XmlParser<T> parser)
		{
			return Wrap(parser);
		}

		private readonly Dictionary<string, int> childEnumerators 
			= new Dictionary<string, int>();

		private readonly Dictionary<string, int> snapshot = new Dictionary<string, int>();

		public XmlParserNode SnapShot()
		{
			snapshot.Clear();
			foreach (var kv in childEnumerators)
			{
				snapshot.Add(kv.Key, kv.Value);
			}
			return this;
		}

		public XmlParserNode Reset()
		{
			childEnumerators.Clear();
			foreach (var kv in snapshot)
			{
				childEnumerators.Add(kv.Key, kv.Value);
			}
			return this;
		}

		public bool NextChild(string tagName, out Node child)
		{
			child = null;

			if (!childEnumerators.ContainsKey(tagName))
				childEnumerators[tagName] = 0;

			var currentChildIndex = childEnumerators[tagName];

			var children =
				node
					.Children
					.Where(c => c is Node)
					.Cast<Node>()
					.Where(n => n.Name == tagName);

			if (children.Count() <= currentChildIndex)
			{	
				childEnumerators[tagName] = 0;
				return false;
			}

			child = children.ElementAt(currentChildIndex);
			childEnumerators[tagName] = currentChildIndex + 1;
			return true;
		}

		private XmlParser<T> Wrap<T>(XmlParser<T> parser)
		{
			return
				state =>
					{
						var old = state.Current;
						state.Current = new XmlParserNode(node);
						var result = parser(state);
						state.Current = old;
						return result;
				};
		}
	}
}