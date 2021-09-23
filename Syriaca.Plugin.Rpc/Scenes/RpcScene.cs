using System.Collections.Generic;
using Syriaca.Client.Information;
using Syriaca.Client.Memory;

namespace Syriaca.Plugin.Rpc.Scenes
{
    public abstract class RpcScene
    {
        public RpcClient Client { get; }
        public Dictionary<string, object> SceneProperties { get; }
        public GdReader Reader { get; }
        public abstract IEnumerable<GameScene> Scenes { get; }

        protected RpcScene(RpcClient client, Dictionary<string, object> sceneProperties, GdReader reader)
        {
            Client = client;
            SceneProperties = sceneProperties;
            Reader = reader;
        }

        public virtual void Pulse()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}