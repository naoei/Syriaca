using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
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

        public TcpBuilder Write(object obj)
        {
            using var ms = new MemoryStream();

            using (var bsonWriter = new BsonDataWriter(ms))
            {
                new JsonSerializer().Serialize(bsonWriter, obj);
            }
            
            writer.Write(ms.ToArray());
            return this;
        }

        public void End()
        {
            server.Send(writer.ToArray());
        }
    }
}