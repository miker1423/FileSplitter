using System;
using System.Collections.Generic;
using System.Text;

namespace FileSplitter.Splitter
{
    public class BasicSplitter
    {
        public IEnumerable<byte[]> Split(ReadOnlySpan<byte> data, int slices)
        {
            var spans = new List<byte[]>();
            
            return spans;
        }

        public bool WriteFile(string entityName, ulong index, byte[] data)
        {
            Console.WriteLine($"{entityName}: {index} -> {Encoding.UTF8.GetString(data)}");
            return true;
        }
    }
}
