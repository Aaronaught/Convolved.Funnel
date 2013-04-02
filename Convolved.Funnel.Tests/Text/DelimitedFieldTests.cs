using System;
using Convolved.Funnel.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Convolved.Funnel.Tests.Text
{
    [TestClass]
    public class DelimitedFieldTests
    {
        private TextFileContext context;
        private DelimitedField field;

        [TestInitialize]
        public void Initialize()
        {
            context = new TextFileContextStub("One, two, three||four");
            field = new DelimitedField(new[] { ",", " ", "||" }, null, null);
        }

        [TestMethod]
        public void Reads_value_up_to_next_delimiter()
        {
            context.CurrentLinePosition = 5;
            var value = field.ReadValue(context);
            Assert.AreEqual("two", value);
        }

        [TestMethod]
        public void When_delimiter_is_multiple_characters_then_reads_up_to_first_character()
        {
            context.CurrentLinePosition = 10;
            var value = field.ReadValue(context);
            Assert.AreEqual("three", value);
        }

        [TestMethod]
        public void When_delimiter_is_multiple_characters_then_advances_current_position_past_end_of_delimiter()
        {
            context.CurrentLinePosition = 10;
            var value = field.ReadValue(context);
            Assert.AreEqual(17, context.CurrentLinePosition);
        }

        [TestMethod]
        public void When_not_at_delimiter_then_advances_current_position_past_next_delimiter()
        {
            field.ReadValue(context);
            Assert.AreEqual(4, context.CurrentLinePosition);
        }

        [TestMethod]
        public void When_at_delimiter_then_advances_current_position_past_current_delimiter()
        {
            context.CurrentLinePosition = 4;
            field.ReadValue(context);
            Assert.AreEqual(5, context.CurrentLinePosition);
        }

        [TestMethod]
        public void When_no_more_delimiters_before_end_of_line_then_reads_rest_of_line()
        {
            context.CurrentLinePosition = 17;
            var value = field.ReadValue(context);
            Assert.AreEqual("four", value);
            Assert.AreEqual(21, context.CurrentLinePosition);
        }
    }
}
