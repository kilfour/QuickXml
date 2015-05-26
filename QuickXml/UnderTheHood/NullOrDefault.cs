namespace QuickXml.UnderTheHood
{
    public static class NullOrDefault
    {
        public static T For<T>()
        {
            return typeof(T).IsValueType ? default(T) : (T)(object)(null);
        }
    }
}