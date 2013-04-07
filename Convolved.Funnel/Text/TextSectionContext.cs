using System;
using System.Threading.Tasks;

namespace Convolved.Funnel.Text
{
    public class TextSectionContext : SectionContext<TextFileContext>
    {
        public TextSectionContext(TextFileContext context)
            : base(context)
        {
        }

        protected override Task<bool> ReadNextRecordCore()
        {
            return FileContext.ReadNextLineAsync();
        }
    }
}