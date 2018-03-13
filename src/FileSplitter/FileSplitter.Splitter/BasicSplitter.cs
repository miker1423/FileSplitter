using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace FileSplitter.Splitter
{
    public class BasicSplitter
    {
        public IEnumerable<IEnumerable<byte>> Split(byte[] data, int slices)
        {
            var partitions = new List<byte>[slices];
            var maxSize = (int)Math.Ceiling(data.Length / (double)slices);
            int k = 0;

            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<byte>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= data.Length)
                    {
                        break;
                    }

                    partitions[i].Add(data[j]);
                }

                k += maxSize;
            }

            return partitions;
        }

        public bool WriteFile(string entityName, ulong index, byte[] data)
        {
            var text = Encoding.UTF8.GetString(data);
            var json = JsonConvert.DeserializeObject<FileDescriptor>(text);

            var directory = Directory.CreateDirectory(@".\Files\" + json.Name);
            var path = Path.Combine(directory.FullName, $"{json.Name}-{json.Part}.json");
            File.WriteAllText(path, text);

            Console.WriteLine(text);

            return true;
        }
    }
}
