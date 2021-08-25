using System.Diagnostics;
using System.IO;

namespace Syriaca.Client.Memory
{
    public class GdReader : MemoryReader
    {
        public GdReader(Process process) 
            : base(process)
        {
        }

        protected override AddressDictionary CreateAddressDictionary() => AddressDictionary.Parse(File.ReadAllText("Addresses.txt"));
    }
}