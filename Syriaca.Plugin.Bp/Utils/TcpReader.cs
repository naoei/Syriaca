using System.IO;
using System.Text;

namespace Syriaca.Plugin.Bp.Utils
{
    public class TcpReader : BinaryReader
    {
        public TcpReader(byte[] input) 
            : base(new MemoryStream())
        {
        }

        public TcpReader(byte[] input, Encoding encoding) 
            : base(new MemoryStream(input), encoding)
        {
        }

        public TcpReader(byte[] input, Encoding encoding, bool leaveOpen) 
            : base(new MemoryStream(input), encoding, leaveOpen)
        {
        }
    }
}