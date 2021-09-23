using System.Collections.Generic;
using Syriaca.Client.Information;
using Syriaca.Client.Memory;

namespace Syriaca.Plugin.Rpc.Scenes
{
    public class UnknownScene : RpcScene
    {
        public override IEnumerable<GameScene> Scenes => new[] { GameScene.Unknown };
        
        public UnknownScene(RpcClient client, Dictionary<string, object> sceneProperties, GdReader reader) 
            : base(client, sceneProperties, reader)
        {
            client.ChangeStatus(s =>
            {
                s.Details = string.Empty;
                s.State = "Unknown state";
                s.Timestamps = null;
            });
        }
    }
}