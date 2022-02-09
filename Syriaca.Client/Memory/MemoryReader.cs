using System;
using System.Diagnostics;
using System.Text;
using Binarysharp.MemoryManagement;

namespace Syriaca.Client.Memory
{
    /// <summary>
    /// Allows for reading memory regions in a <seealso cref="Process" />.
    /// </summary>
    public abstract class MemoryReader
    {
        /// <summary>
        /// The process that is currently being accessed.
        /// </summary>
        public Process Process { get; }
        /// <summary>
        /// The memory reader.
        /// </summary>
        public MemorySharp Memory { get; }

        /// <summary>
        /// All of the addresses available for this <see cref="MemoryReader"/>.
        /// </summary>
        public AddressDictionary Addresses { get; }

        protected MemoryReader(Process process)
        {
            Memory = new MemorySharp(Process = process);
            Addresses = CreateAddressDictionary();
        }

        /// <summary>
        /// Creates all of the necessary memory Addresses and their offsets.
        /// </summary>
        /// <returns>An <seealso cref="AddressDictionary" />.</returns>
        protected abstract AddressDictionary CreateAddressDictionary();

        /// <summary>
        /// Reads any type of value in the <see cref="Process" />'s memory region.
        /// </summary>
        /// <param name="addressEntryName">The address name to fetch offset data from.</param>
        /// <param name="baseAddress">The base address.</param>
        /// <typeparam name="T">
        /// The type to read the value in-memory to.
        /// Note that if you want to read a string, please use the <see cref="ReadString(string,System.IntPtr)" /> method.
        /// </typeparam>
        /// <returns>The value in memory.</returns>
        public T Read<T>(string addressEntryName, IntPtr baseAddress)
            where T : struct =>
            Read<T>(Addresses[addressEntryName], baseAddress);

        /// <summary>
        /// Reads any type of value in the <see cref="Process" />'s memory region.
        /// </summary>
        /// <param name="entry">A custom address entry to read the offsets from.</param>
        /// <param name="baseAddress">The base address.</param>
        /// <typeparam name="T">
        /// The type to read the value in-memory to.
        /// Note that if you want to read a string, please use the <see cref="ReadString(string,System.IntPtr)" /> method.
        /// </typeparam>
        /// <returns>The value in memory.</returns>
        public T Read<T>(Address entry, IntPtr baseAddress)
            where T : struct =>
            Read<T>(baseAddress, entry.Offsets);

        /// <summary>
        /// Reads any type of value in the <see cref="Process" />'s memory region.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="offsets">The offsets to read to get to the value in-memory.</param>
        /// <typeparam name="T">
        /// The type to read the value in memory to.
        /// Note that if you want to read a string, please use the <see cref="ReadString(string,System.IntPtr)" /> method.
        /// </typeparam>
        /// <returns>The value in memory.</returns>
        public T Read<T>(IntPtr baseAddress, params int[] offsets)
        {
            var address = forwardAddress(baseAddress, offsets);

            return Memory.Read<T>(address + offsets[^1], false);
        }

        /// <summary>
        /// Reads a <see cref="string" /> in UTF8 encoding in the <see cref="Process" />'s memory region.
        /// </summary>
        /// <param name="addressEntryName">The address name to fetch offset data from.</param>
        /// <param name="baseAddress">The base address.</param>
        /// <returns>The string value in memory.</returns>
        public string ReadString(string addressEntryName, IntPtr baseAddress)
            => ReadString(Addresses[addressEntryName], baseAddress);

        /// <summary>
        /// Reads a <see cref="string" /> in UTF8 encoding in the <see cref="Process" />'s memory region.
        /// </summary>
        /// <param name="entry">A custom address entry to read the offsets from.</param>
        /// <param name="baseAddress">The base address.</param>
        /// <returns>The string value in memory.</returns>
        public string ReadString(Address entry, IntPtr baseAddress)
            => ReadString(baseAddress, entry.Offsets);

        /// <summary>
        /// Reads a <see cref="string" /> in UTF8 encoding in the <see cref="Process" />'s memory region.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="offsets">The offsets to read to get to the value in-memory.</param>
        /// <returns>The string value in memory.</returns>
        public string ReadString(IntPtr baseAddress, params int[] offsets)
        {
            var address = forwardAddress(baseAddress, offsets);

            return Memory.ReadString(address + offsets[^1], Encoding.UTF8, false);
        }

        private IntPtr forwardAddress(IntPtr address, int[] offsets)
        {
            for (var i = 0; i < offsets.Length - 1; i++)
                address = Memory.Read<IntPtr>(address + offsets[i], false);

            return address;
        }
    }
}