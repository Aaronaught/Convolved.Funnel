using System;
using System.Threading.Tasks;

namespace Convolved.Funnel.Tests.Mapping
{
    class SectionContextStub : SectionContext<FileContext>
    {
        private int recordsRead;

        public SectionContextStub(FileContext context)
            : base(context)
        {
        }

        protected override Task<bool> ReadNextRecordCore()
        {
            if (recordsRead >= RecordCount)
                return Task.FromResult(false);
            ++recordsRead;
            return Task.FromResult(true);
        }

        public int RecordCount { get; set; }
    }
}