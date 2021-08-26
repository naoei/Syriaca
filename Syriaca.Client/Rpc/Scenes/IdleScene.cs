using System.Collections.Generic;
using Syriaca.Client.Information;
using Syriaca.Client.Memory;

namespace Syriaca.Client.Rpc.Scenes
{
    public class IdleScene : RpcScene
    {
        public override IEnumerable<GameScene> Scenes => new[] { GameScene.MainMenu, GameScene.Online };
        
        public IdleScene(RpcClient client, Dictionary<string, object> sceneProperties, GdReader reader) 
            : base(client, sceneProperties, reader)
        {
            client.ChangeStatus(s =>
            {
                s.Details = string.Empty;
                s.State = "In menus";
                s.Timestamps = null;
            });
        }
    }
}