using System;
using Convolved.Funnel.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Convolved.Funnel.Tests.Text
{
    [TestClass]
    public class FixedWidthFieldTests
    {
        private TextFileContext context;
        private FixedWidthField field;

        [TestInitialize]
        public void Initialize()
        {
            context = new TextFileContextStub("Hello world!");
            field = new FixedWidthField(5);
        }

        [TestMethod]
        public void Reads_length_characters_from_current_position()
        {
            context.CurrentLinePosition = 6;
            var value = field.ReadValue(context);
            Assert.AreEqual("world", value);
        }

        [TestMethod]
        public void Advances_current_position_by_length()
        {
            field.ReadValue(context);
            Assert.AreEqual(5, context.CurrentLinePosition);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfLineException))]
        public void When_length_from_current_position_exceeds_line_length_then_throws_exception()
        {
            context.CurrentLinePosition = 10;
            field.ReadValue(context);
        }
    }
}
