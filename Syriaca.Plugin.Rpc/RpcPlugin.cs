using System.Collections.Generic;
using DiscordRPC;
using Syriaca.Client;
using Syriaca.Client.Information;
using Syriaca.Plugin.Rpc.Scenes;

namespace Syriaca.Plugin.Rpc
{
    public class RpcPlugin : Client.Plugins.Plugin
    {
        public override string Name => "Discord RPC";

        private RpcClient client;
        private RpcScene currentScene;

        public override Scheduler CreateScheduler()
        {
            var scheduler = new Scheduler(2000);
            client = new RpcClient();

            toRpcScene(State.Scene);
            scheduler.Add(client.Update);
            scheduler.Add(currentScene.Pulse);
            
            client.ChangeStatus(s =>
            {
                s.Assets = new Assets
                {
                    LargeImageKey = "gd"
                };
            });

            State.SceneChanged += e =>
            {
                onSceneChanged(e, scheduler);
            };

            return scheduler;
        }
        
        private void onSceneChanged(ValueChangedEvent<SceneInformation> obj, Scheduler scheduler)
        {
            scheduler.Clear();

            toRpcScene(obj.NewValue);

            scheduler.Add(currentScene.Pulse);
            scheduler.Add(client.Update);
        }

        private void toRpcScene(SceneInformation info)
        {
            switch (info.Scene)
            {
                case GameScene.MainMenu: 
                case GameScene.Search: 
                case GameScene.Select:
                case GameScene.Online:
                    Idle:
                    currentScene = new IdleScene(client, info.SceneData, GdReader);

                    break;

                case GameScene.Play:
                    if (info.SceneData.Count > 2)
                        currentScene = new PlayScene(client, info.SceneData, GdReader);
                    else
                        goto Idle;

                    break;
                
                default:
                    currentScene = new UnknownScene(client, info.SceneData, GdReader);

                    break;
            }
        }
    }
}