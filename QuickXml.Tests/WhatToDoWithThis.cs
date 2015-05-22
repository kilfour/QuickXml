using QuickXml.UnderTheHood;
using Xunit;

namespace QuickXml.Tests
{
    public class WhatToDoWithThis
    {
        [Fact(Skip="No longer throws")]
        public void BothFail()
        {
            const string input = "<root><third>some text</third></root>";

            var xmlParser =
                from val in XmlParse.Child("first").Or(XmlParse.Child("second")).Content()
                select val;

            Assert.Throws<XmlParserException>(() => xmlParser.Parse(input));
        }

        [Fact(Skip = "No longer throws")]
        public void ChildFailureThrows()
        {
            const string input = "<root></root>";

            var xmlParser =
                from first in XmlParse.Child("first")
                select first;

            Assert.Throws<XmlParserException>(() => xmlParser.Parse(input));
        }

        [Fact(Skip = "No longer throws")]
        public void ContentFailureThrows()
        {
            const string input = "<root></root>";

            var xmlParser =
                from first in XmlParse.Child("first").Content()
                select first;

            Assert.Throws<XmlParserException>(() => xmlParser.Parse(input));
        }

        [Fact(Skip = "No longer throws")]
        public void ContentAsIntFails()
        {
            const string input = "<root><first>bla</first></root>";

            var xmlParser =
                from first in XmlParse.Child("first").Content().Int()
                select first;

            Assert.Throws<XmlParserException>(() => xmlParser.Parse(input));
        }
    }
}