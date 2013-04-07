using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Convolved.Funnel.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Convolved.Funnel.Tests.Mapping
{
    [TestClass]
    public class ComponentMapWithSequenceTests
    {
        private FileContext context;
        private SectionContextStub sectionContextStub;
        private Mock<IClassMap<ChildTarget, FileContext>> childTargetMapMock;
        private ParentTarget parentTarget;
        private ChildTarget[] childTargets;
        private int childIndex;
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
            childTargets = new[] { new ChildTarget(), new ChildTarget() };
            endOfSectionPredicate = c => false;
            map = new ComponentMap<ParentTarget, ChildTarget, FileContext>(p => p.Children,
                c => sectionContextStub, () => childTargets[childIndex++], childTargetMapMock.Object,
                c => endOfSectionPredicate(c));
        }

        [TestMethod]
        public void When_no_records_then_extract_maps_empty_sequence()
        {
            map.ExtractAsync(context, parentTarget).Wait();
            Assert.IsNotNull(parentTarget.Children);
            Assert.IsFalse(parentTarget.Children.Any());
        }

        [TestMethod]
        public void When_multiple_records_then_extract_maps_all_records()
        {
            sectionContextStub.RecordCount = 2;
            map.ExtractAsync(context, parentTarget).Wait();
            Assert.IsNotNull(parentTarget.Children);
            var mappedChildren = parentTarget.Children.ToList();
            Assert.AreEqual(2, mappedChildren.Count);
            Assert.AreEqual(childTargets[0], mappedChildren[0]);
            Assert.AreEqual(childTargets[1], mappedChildren[1]);
            childTargetMapMock.Verify(m => m.ExtractAsync(context, childTargets[0]));
            childTargetMapMock.Verify(m => m.ExtractAsync(context, childTargets[1]));
        }

        public class ParentTarget
        {
            public IEnumerable<ChildTarget> Children { get; set; }
        }

        public class ChildTarget { }
    }
}