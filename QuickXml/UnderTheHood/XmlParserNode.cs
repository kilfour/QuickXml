using System.Collections.Generic;
using System.Linq;
using System.Text;
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
					return 
                        hasChild 
                        ? Result.Success(new XmlParserNode(child), state) 
                        : Result.Failure<XmlParserNode>(state);
				};
		}

	    private readonly Dictionary<string, int> childEnumerators
	        = new Dictionary<string, int>();

	    private bool NextChild(string tagName, out Node child)
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
	                .Where(n => n.Name == tagName)
	                .ToArray();

	        if (children.Count() <= currentChildIndex)
	        {
	            childEnumerators[tagName] = 0;
	            return false;
	        }

	        child = children.ElementAt(currentChildIndex);
	        childEnumerators[tagName] = currentChildIndex + 1;
	        return true;
	    }

        public virtual XmlParser<XmlParserNode> All(string tagName)
	    {
	        return
	            state =>
	            {
	                Node child;
	                var hasChild = NextAll(tagName, out child);
	                return
	                    hasChild
	                        ? Result.Success(new XmlParserNode(child), state)
	                        : Result.Failure<XmlParserNode>(state);
	            };
        }

	    private readonly Dictionary<string, int> allEnumerators
	        = new Dictionary<string, int>();

	    
	    private bool NextAll(string tagName, out Node child)
	    {
	        child = null;

	        if (!allEnumerators.ContainsKey(tagName))
	            allEnumerators[tagName] = 0;

	        var currentChildIndex = allEnumerators[tagName];

	        var children = new List<Node>();

            GetAllChildren(node, tagName, children);

	        if (children.Count <= currentChildIndex)
	        {
	            allEnumerators[tagName] = 0;
	            return false;
	        }

	        child = children.ElementAt(currentChildIndex);
	        allEnumerators[tagName] = currentChildIndex + 1;
	        return true;
	    }

	    private void GetAllChildren(Node parent, string tagName, List<Node> accumalator)
	    {
	        var children = 
	            parent
	                .Children
	                .Where(c => c is Node)
	                .Cast<Node>();
	        foreach (var child in children)
	        {
	            if (child.Name == tagName)
	            {
	                accumalator.Add(child);
                }
                GetAllChildren(child, tagName, accumalator);
	        }
	    }

        public virtual XmlParserResult<string> GetContent(XmlParserState state)
		{
            if (node.Children.Count() == 1)
            {
                var contentNode = node.Children.SingleOrDefault() as Content;
                var content = contentNode == null ? "" : contentNode.Text;
                return Result.Success(content, state);
            }
            var builder = new StringBuilder();
		    foreach (var child in node.Children)
		    {
                builder.Append(child.AsString());
		    }
            return Result.Success(builder.ToString(), state);
		}

	    public virtual XmlParserResult<bool> HasCDataContent(XmlParserState state)
	    {
	        if (node.Children.Count() == 1)
	        {
	            var contentNode = node.Children.SingleOrDefault() as Content;
	            var content = contentNode == null ? false : contentNode.IsCData;
	            return Result.Success(content, state);
	        }
	        return Result.Success(false, state);
	    }

        public virtual XmlParser<T> Apply<T>(XmlParser<T> parser)
		{
			return state => 
			{
			    var old = state.Current;
			    state.Current = new XmlParserNode(node);
			    var result = parser(state);
			    state.Current = old;
			    return result;
			};
		}


	    public XmlParser<string> Content()
	    {
	        return GetContent;
	    }
	}
}