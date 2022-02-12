using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Syriaca.Plugin.Bp.Tcp.Handlers
{
    public class DeviceList : TcpHandler
    {
        public override OpCodes OpCode => OpCodes.DeviceListing;

        public override void HandleData(byte[] data)
        {
            using var ms = new MemoryStream(data);

            var bsonReader = new BsonDataReader(ms);
            bsonReader.ReadRootValueAsArray = true;
            var des = new JsonSerializer().Deserialize<IList<object>>(bsonReader);

            BpPlugin.Devices = des;
        }
    }
}