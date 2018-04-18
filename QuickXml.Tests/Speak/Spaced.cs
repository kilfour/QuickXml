using Xunit;

namespace QuickXml.Tests.Speak
{
    public class Spaced
    {
        [Fact]
        public void CanParse()
        {
            const string input = "< root > < child / > < / root >";

            var childParser =
                from child in XmlParse.Child("child")
                from el in child.Child("notThere").Optional()
                select 1;

            var xmlParser =
                from root in XmlParse.Root()
                from child in childParser
                select child;

            var result = xmlParser.Parse(input);
            Assert.Equal(1, result);
        }

       
    }
}