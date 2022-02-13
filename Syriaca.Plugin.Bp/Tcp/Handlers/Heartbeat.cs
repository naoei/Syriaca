using System;
using Syriaca.Client.Utils;
using Syriaca.Plugin.Bp.Utils;

namespace Syriaca.Plugin.Bp.Tcp.Handlers
{
    public class Heartbeat : TcpHandler
    {
        public override OpCodes OpCode => OpCodes.Heartbeat;

        public override void HandleData(byte[] data)
        {
            new TcpBuilder().Start(OpCodes.Heartbeat).End();
        }
    }
}