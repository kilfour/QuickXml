namespace QuickXml.XmlStructure
{
	public class Content : Item
	{
	    public bool IsCData;
		public string Text;
	    public override string AsString()
	    {
	        return Text;
	    }

	    public override string ToQuickXmlWrite(Node parent, int level)
	    {
	        var text = Text.Trim();
	        if (string.IsNullOrEmpty(text))
	            return "";
	        return $"{new string('\t', level + 1)}.Content(\"{Text}\")\r\n";
	    }
	}
}