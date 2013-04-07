using System;
using System.IO;
using Convolved.Funnel.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Convolved.Funnel.Tests.Mapping
{
    [TestClass]
    public class ClassMapTests
    {
        private FileContext context;
        private Mock<IField<FileContext, string>> fieldMock;
        private ClassMap<Target, FileContext, string> map;

        [TestInitialize]
        public void Initialize()
        {
            context = new FileContext("test.txt", new MemoryStream());
            fieldMock = new Mock<IField<FileContext, string>>();
            map = new ClassMap<Target, FileContext, string>();
        }

        [TestMethod]
        public void Maps_all_properties_using_selectors()
        {
            fieldMock.SetupSequence(m => m.ReadValue(It.IsAny<FileContext>()))
                .Returns("42")
                .Returns("foo")
                .Returns("2013-04-05");
            map.Property(t => t.Id, fieldMock.Object, s => int.Parse(s));
            map.Property(t => t.Name, fieldMock.Object, s => s);
            map.Property(t => t.Date, fieldMock.Object, s => DateTime.Parse(s));
            var target = new Target();
            map.ExtractAsync(context, target).Wait();
            Assert.AreEqual(42, target.Id);
            Assert.AreEqual("foo", target.Name);
            Assert.AreEqual(new DateTime(2013, 4, 5), target.Date);
        }

        public class Target
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }
    }
}