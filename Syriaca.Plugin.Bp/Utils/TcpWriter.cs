using System.IO;

namespace Syriaca.Plugin.Bp.Utils
{
    public class TcpWriter : BinaryWriter
    {
        private MemoryStream stream => OutStream as MemoryStream;
        
        public TcpWriter()
            : base(new MemoryStream())
        {
        }

        public byte[] ToArray()
            => stream.ToArray();
    }
}