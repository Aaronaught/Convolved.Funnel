using System;
using System.IO;
using Convolved.Funnel.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Convolved.Funnel.Tests.Mapping
{
    [TestClass]
    public class PropertyMapTests
    {
        private FileContext context;
        private Mock<IField<FileContext, int>> fieldMock;
        private PropertyMap<PropertyMapTarget, int, FileContext, int> map;
        private PropertyMapTarget target;

        [TestInitialize]
        public void Initialize()
        {
            context = new FileContext("test", new MemoryStream());
            fieldMock = new Mock<IField<FileContext, int>>();
            map = new PropertyMap<PropertyMapTarget, int, FileContext, int>(t => t.Id,
                fieldMock.Object, i => i);
            target = new PropertyMapTarget();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_throws_when_expression_is_not_direct_property_access()
        {
            new PropertyMap<PropertyMapTarget, int, FileContext, int>(t => 42, fieldMock.Object, i => i);
        }

        [TestMethod]
        public void Extract_sets_property_value_to_value_selected_from_field()
        {
            fieldMock.Setup(m => m.ReadValue(It.IsAny<FileContext>())).Returns(42);
            map.Extract(context, target);
            Assert.AreEqual(42, target.Id);
        }
    }

    class PropertyMapTarget
    {
        public int Id { get; set; }
    }
}