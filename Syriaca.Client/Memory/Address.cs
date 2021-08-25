using System.Linq;

namespace Syriaca.Client.Memory
{
    public class Address
    {
        public string Name { get; set; }

        public int[] Offsets { get; set; }

        public string Type { get; set; }
        
        public override string ToString() 
            => $"{Name} - {{ {string.Join(", ", Offsets.Select(o => $"0x{o:X}"))} }} - {Type}";
    }
}