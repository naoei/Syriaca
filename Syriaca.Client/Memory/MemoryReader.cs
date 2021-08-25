using System;
using System.Diagnostics;
using System.Text;
using Binarysharp.MemoryManagement;

namespace Syriaca.Client.Memory
{
    public abstract class MemoryReader
    {
        public Process Process { get; }
        public MemorySharp Memory { get; }

        private readonly AddressDictionary addresses;

        protected MemoryReader(Process process)
        {
            Memory = new MemorySharp(Process = process);
            addresses = CreateAddressDictionary();
        }

        protected abstract AddressDictionary CreateAddressDictionary();

        public T Read<T>(string addressEntryName, IntPtr baseAddress)
            where T : struct =>
            Read<T>(addresses[addressEntryName], baseAddress);

        public T Read<T>(Address entry, IntPtr baseAddress)
            where T : struct =>
            Read<T>(baseAddress, entry.Offsets);

        public T Read<T>(IntPtr baseAddress, params int[] offsets)
        {
            var address = forwardAddress(baseAddress, offsets);

            return Memory.Read<T>(address + offsets[^1], false);
        }

        public string ReadString(string addressEntryName, IntPtr baseAddress)
            => ReadString(addresses[addressEntryName], baseAddress);

        public string ReadString(Address entry, IntPtr baseAddress)
        {
            var address = forwardAddress(baseAddress, entry.Offsets);

            return Memory.ReadString(address + entry.Offsets[^1], Encoding.Default, false);
        }

        private IntPtr forwardAddress(IntPtr address, int[] offsets)
        {
            for (var i = 0; i < offsets.Length - 1; i++)
                address = Memory.Read<IntPtr>(address + offsets[i], false);

            return address;
        }
    }
}