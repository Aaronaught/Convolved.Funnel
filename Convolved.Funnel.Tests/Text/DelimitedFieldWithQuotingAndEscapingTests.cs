using System;
using Convolved.Funnel.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Convolved.Funnel.Tests.Text
{
    [TestClass]
    public class DelimitedFieldWithQuotingAndEscapingTests
    {
        private TextFileContext context;
        private DelimitedField field;

        [TestInitialize]
        public void Initialize()
        {
            context = new TextFileContextStub("Hi, I''m Ray 'Boom Boom' Mancini");
            field = new DelimitedField(new[] { " " }, "'", "'");
        }

        [TestMethod]
        public void When_quoted_field_contains_no_escaped_quotes_then_read_quoted_value()
        {
            context.CurrentLinePosition = 13;
            var value = field.ReadValue(context);
            Assert.AreEqual("Boom Boom", value);
            Assert.AreEqual(25, context.CurrentLinePosition);
        }

        [TestMethod]
        public void When_quoted_field_contains_escaped_quotes_then_read_quoted_value_excluding_escape_token()
        {
            context = new TextFileContextStub("Testing 'quoted with ''escaped'' part'");
            context.CurrentLinePosition = 8;
            var value = field.ReadValue(context);
            Assert.AreEqual("quoted with 'escaped' part", value);
            Assert.AreEqual(38, context.CurrentLinePosition);
        }

        [TestMethod]
        public void When_non_quoted_field_contains_escaped_quote_then_read_as_quote_only()
        {
            context.CurrentLinePosition = 4;
            var value = field.ReadValue(context);
            Assert.AreEqual("I'm", value);
        }
    }
}
