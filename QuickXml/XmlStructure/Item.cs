namespace QuickXml.XmlStructure
{
    public abstract class Item
    {
        public abstract string AsString();

        public abstract string ToQuickXmlWrite(Node parent, int level);
    }
}