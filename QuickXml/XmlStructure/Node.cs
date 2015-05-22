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
	}
}