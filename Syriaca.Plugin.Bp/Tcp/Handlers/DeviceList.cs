using Syriaca.Plugin.Bp.Utils;

namespace Syriaca.Plugin.Bp.Tcp.Handlers
{
    public class DeviceList : TcpHandler
    {
        public override OpCodes OpCode => OpCodes.DeviceListing;

        public override void HandleData(byte[] data)
        {
            var list = new TcpReader(data).ReadBson(true);

            BpPlugin.Devices = list;
        }
    }
}