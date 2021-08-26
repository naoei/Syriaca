using System.Linq;

namespace Syriaca.Client.Memory
{
    /// <summary>
    /// Information on an address.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// The name of the address, also used for searching in <see cref="AddressDictionary"/>.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Offsets to get to the specific value.
        /// </summary>
        public int[] Offsets { get; set; }

        /// <summary>
        /// The value type of the Address
        /// </summary>
        public string Type { get; set; }
        
        public override string ToString() 
            => $"{Name} - {{ {string.Join(", ", Offsets.Select(o => $"0x{o:X}"))} }} - {Type}";
    }
}