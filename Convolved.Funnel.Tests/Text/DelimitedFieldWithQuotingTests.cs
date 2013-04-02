using System;
using Convolved.Funnel.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Convolved.Funnel.Tests.Text
{
    [TestClass]
    public class DelimitedFieldWithQuotingTests
    {
        private TextFileContext context;
        private DelimitedField field;

        [TestInitialize]
        public void Initialize()
        {
            context = new TextFileContextStub("Here is a ``quoted value`` and ``another one``. ");
            field = new DelimitedField(new[] { " " }, "``", null);
        }

        [TestMethod]
        public void Reads_non_quoted_fields_as_normal_delimited()
        {
            var value = field.ReadValue(context);
            Assert.AreEqual("Here", value);
        }

        [TestMethod]
        public void Ignores_delimiters_inside_quoted_part()
        {
            context.CurrentLinePosition = 10;
            var value = field.ReadValue(context);
            Assert.AreEqual("quoted value", value);
        }

        [TestMethod]
        public void When_delimiter_not_after_end_quote_then_combines_unquoted_part()
        {
            context.CurrentLinePosition = 31;
            var value = field.ReadValue(context);
            Assert.AreEqual("another one.", value);
        }
    }
}
