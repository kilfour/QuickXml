﻿using Xunit;

namespace QuickXml.Tests.Speak
{
    public class EmptyNodes
    {
        [Fact]
        public void HaveChildrenInitialized()
        {
            const string input = "<root><child/></root>";
            
            var childParser = 
                from child in XmlParse.Child("child")
                from el in child.Child("notThere").Optional()
                select 1;

            var xmlParser = 
                XmlParse.Root().Apply(childParser);

            var result = xmlParser.Parse(input);
            Assert.Equal(1, result);
        }
    }
}