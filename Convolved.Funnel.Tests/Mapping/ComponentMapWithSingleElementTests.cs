using System;
using System.IO;
using System.Threading.Tasks;
using Convolved.Funnel.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Convolved.Funnel.Tests.Mapping
{
    [TestClass]
    public class ComponentMapWithSingleElementTests
    {
        private FileContext context;
        private SectionContextStub sectionContextStub;
        private Mock<IClassMap<ChildTarget, FileContext>> childTargetMapMock;
        private ParentTarget parentTarget;
        private ChildTarget childTarget;
        private Func<SectionContext<FileContext>, bool> endOfSectionPredicate;
        private ComponentMap<ParentTarget, ChildTarget, FileContext> map;

        [TestInitialize]
        public void Initialize()
        {
            context = new FileContext("test", new MemoryStream());
            sectionContextStub = new SectionContextStub(context);
            childTargetMapMock = new Mock<IClassMap<ChildTarget,FileContext>>();
            childTargetMapMock.Setup(m => m.ExtractAsync(It.IsAny<FileContext>(), It.IsAny<ChildTarget>()))
                .Returns(Task.Delay(0));
            parentTarget = new ParentTarget();
            childTarget = new ChildTarget();
            endOfSectionPredicate = c => false;
            map = new ComponentMap<ParentTarget, ChildTarget, FileContext>(p => p.Child,
                c => sectionContextStub, () => childTarget, childTargetMapMock.Object,
                c => endOfSectionPredicate(c));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_throws_when_expression_is_not_direct_property_access()
        {
            new ComponentMap<ParentTarget, ChildTarget, FileContext>(p => new ChildTarget(),
                c => sectionContextStub, () => childTarget, childTargetMapMock.Object);
        }

        [TestMethod]
        public void When_no_records_then_extract_maps_nothing()
        {
            map.ExtractAsync(context, parentTarget).Wait();
            Assert.IsNull(parentTarget.Child);
        }

        [TestMethod]
        public void When_one_record_then_extract_maps_single_record()
        {
            sectionContextStub.RecordCount = 1;
            map.ExtractAsync(context, parentTarget).Wait();
            Assert.AreEqual(childTarget, parentTarget.Child);
            childTargetMapMock.Verify(m => m.ExtractAsync(context, childTarget));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void When_multiple_records_then_extract_throws_exception()
        {
            sectionContextStub.RecordCount = 2;
            map.ExtractAsync(context, parentTarget).RunSynchronously();
        }

        public class ParentTarget
        {
            public ChildTarget Child { get; set; }
        }

        public class ChildTarget { }
    }
}