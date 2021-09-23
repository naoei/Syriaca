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

            currentScene = new UnknownScene(client, new Dictionary<string, object>(), GdReader);
            scheduler.Add(client.Update);
            
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

            switch (obj.NewValue.Scene)
            {
                case GameScene.MainMenu:
                case GameScene.Online:
                    Idle:
                    currentScene = new IdleScene(client, obj.NewValue.SceneData, GdReader);

                    break;

                case GameScene.Play:
                    if (obj.NewValue.SceneData.Count > 2)
                        currentScene = new PlayScene(client, obj.NewValue.SceneData, GdReader);
                    else
                        goto Idle;

                    break;

                default:
                    currentScene = new UnknownScene(client, obj.NewValue.SceneData, GdReader);

                    break;
            }

            scheduler.Add(currentScene.Pulse);
            scheduler.Add(client.Update);
        }
    }
}