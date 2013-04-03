using System;
using Convolved.Funnel.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Convolved.Funnel.Tests.Mapping
{
    [TestClass]
    public class PropertyMapperTests
    {
        private PropertyMapper<PropertyMapperTarget> mapper;
        private PropertyMapperTarget target;

        [TestInitialize]
        public void Initialize()
        {
            mapper = PropertyMapper.Get<PropertyMapperTarget>();
            target = new PropertyMapperTarget();
        }

        [TestMethod]
        public void GetValue_reads_current_value_from_named_property()
        {
            target.Id = 42;
            var value = mapper.GetValue(target, "Id");
            Assert.AreEqual(42, value);
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyNotMappedException))]
        public void GetValue_throws_exception_when_property_does_not_exist()
        {
            mapper.GetValue(target, "foo");
        }

        [TestMethod]
        public void SetValue_writes_new_value_to_named_property()
        {
            mapper.SetValue(target, "Id", 88);
            Assert.AreEqual(88, target.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyNotMappedException))]
        public void SetValue_throws_exception_when_property_does_not_exist()
        {
            mapper.SetValue(target, "foo", "bar");
        }
    }

    class PropertyMapperTarget
    {
        public int Id { get; set; }
    }
}