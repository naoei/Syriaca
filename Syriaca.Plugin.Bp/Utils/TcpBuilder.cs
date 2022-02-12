using Syriaca.Plugin.Bp.Tcp;

namespace Syriaca.Plugin.Bp.Utils
{
    public class TcpBuilder
    {
        private TcpWriter writer;
        private TcpClient server => BpPlugin.Client;

        public TcpBuilder Start(OpCodes code)
        {
            writer = new TcpWriter();
            writer.Write((byte) code);

            return this;
        }

        public void End()
        {
            server.Send(writer.ToArray());
        }
    }
}