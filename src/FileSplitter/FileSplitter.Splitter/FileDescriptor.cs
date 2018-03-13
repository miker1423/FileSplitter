using System;
using System.Collections.Generic;
using System.Text;

namespace FileSplitter.Splitter
{
    public class FileDescriptor
    {
        public int Part { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }
    }
}
