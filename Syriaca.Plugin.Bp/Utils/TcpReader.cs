using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Syriaca.Plugin.Bp.Utils
{
    public class TcpReader : BinaryReader
    {
        public TcpReader(byte[] input) 
            : base(new MemoryStream(input))
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

        public dynamic ReadBson(bool isArray = false)
        {
            var bsonReader = new BsonDataReader(BaseStream);

            dynamic deserialized;

            if (isArray)
            {
                bsonReader.ReadRootValueAsArray = true;
                deserialized = new JsonSerializer().Deserialize<IList<object>>(bsonReader);
            }
            else
            {
                deserialized = new JsonSerializer().Deserialize(bsonReader);
            }

            return deserialized;
        }

        public T ReadBson<T>(bool isArray = false) 
            where T : class
            => ReadBson(isArray) as T;
    }
}