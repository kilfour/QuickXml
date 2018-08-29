using System.Collections.Generic;
using System.Text;

namespace QuickXml.XmlStructure
{
	public class Node : Item
	{
		public string Name;
		public IEnumerable<Item> Children;
		public Dictionary<string, string> Attributes;

	    public override string AsString()
	    {
	        var builder = new StringBuilder();
	        builder.AppendFormat("<{0}", Name);
	        foreach (var attribute in Attributes)
	        {
                builder.AppendFormat(" {0}=\"{1}\"", attribute.Key, attribute.Value);
	        }
	        builder.Append(">");
	        foreach (var child in Children)
	        {
	            builder.Append(child.AsString());
	        }
            builder.AppendFormat("</{0}>", Name);
	        return builder.ToString();
	    }

	    public override string ToQuickXmlWrite(Node parent, int level)
	    {
	        var builder = new StringBuilder();
            var indent = new string('\t', level);
	        builder.AppendFormat("{0}from {1} in {2}.Tag(\"{1}\")", indent, Name, parent == null ? "input" : parent.Name);
	        builder.AppendLine();
	        if (Attributes.Count > 0)
	            builder.AppendFormat("{0}\t", indent);
            foreach (var attribute in Attributes)
	        {
	            builder.AppendFormat(".Attribute(\"{0}\", \"{1}\")", attribute.Key, attribute.Value);
	        }
	        if (Attributes.Count > 0)
	            builder.AppendLine();
            foreach (var child in Children)
	        {
	            builder.Append(child.ToQuickXmlWrite(this, level + 1));
	        }
	        return builder.ToString();
	    }
    }
}