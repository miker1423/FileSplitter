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

        public string[] GetFile(string name)
        {
            var directoryPath = Path.Combine(@".\Files\", name);
            var directory = Directory.GetFiles(directoryPath);
            var files = directory.Select(path => File.ReadAllText(path));
            return files.ToArray();
        }

        public bool WriteFile(string entityName, ulong index, byte[] data)
        {
            var text = Encoding.UTF8.GetString(data);
            try
            {
                var json = JsonConvert.DeserializeObject<FileDescriptor>(text);

                var directory = Directory.CreateDirectory(@".\Files\" + json.Name);
                var path = Path.Combine(directory.FullName, $"{json.Name}-{json.Part}.json");
                File.WriteAllText(path, text);

                Console.WriteLine(text);
            }
            catch (Exception)
            {
                var files = JsonConvert.DeserializeObject<string[]>(text);
                var orderedFile = files
                    .Select(json => JsonConvert.DeserializeObject<FileDescriptor>(json))
                    .OrderByDescending(file => file.Part);

                var content = new List<byte>();
                foreach (var slice in orderedFile.Select(file => file.Content))
                {
                    content.AddRange(slice);
                }

                Console.WriteLine(Encoding.UTF8.GetString(content.ToArray()));
            }

            return true;
        }
    }
}
