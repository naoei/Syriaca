namespace Syriaca.Plugin.Bp.Tcp.Handlers
{
    public abstract class TcpHandler
    {
        public abstract OpCodes OpCode { get; }

        public abstract void HandleData(byte[] data);
    }
}