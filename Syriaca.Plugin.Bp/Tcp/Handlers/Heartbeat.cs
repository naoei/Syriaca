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
            Logger.Info("Heartbeat received.");
            new TcpBuilder().Start(OpCodes.Heartbeat).End();
        }
    }
}