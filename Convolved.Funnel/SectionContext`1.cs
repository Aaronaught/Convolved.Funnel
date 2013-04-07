using System;
using System.Threading.Tasks;

namespace Convolved.Funnel
{
    public abstract class SectionContext<TContext>
    {
        private readonly TContext fileContext;

        public SectionContext(TContext fileContext)
        {
            Ensure.ArgumentNotNull(fileContext, "fileContext");
            this.fileContext = fileContext;
        }

        public async Task<bool> ReadNextRecord()
        {
            bool result = await ReadNextRecordCore();
            if (result)
                ++CurrentRecordCount;
            return result;
        }

        protected abstract Task<bool> ReadNextRecordCore();

        public int CurrentRecordCount { get; private set; }

        public TContext FileContext
        {
            get { return fileContext; }
        }
    }
}