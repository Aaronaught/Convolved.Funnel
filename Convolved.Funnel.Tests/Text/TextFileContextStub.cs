using System;
using System.IO;
using System.Text;
using Convolved.Funnel.Text;

namespace Convolved.Funnel.Tests.Text
{
    class TextFileContextStub : TextFileContext
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        public TextFileContextStub(string text)
            : base("Dummy.txt", CreateStream(text), Encoding)
        {
            ReadNextLineAsync().Wait();
        }

        private static Stream CreateStream(string text)
        {
            return new MemoryStream(Encoding.GetBytes(text));
        }
    }
}