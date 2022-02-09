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
            client = new RpcClient();

            client.ChangeStatus(s =>
            {
                s.Assets = new Assets
                {
                    LargeImageKey = "gd"
                };
            });

            return new Scheduler(2000);
        }

        public override void OnScheduleCreated()
        {
            changeRpcScene(toRpcScene(State.Scene));
            State.SceneChanged += obj => changeRpcScene(toRpcScene(obj.NewValue));
            ;
        }

        private void changeRpcScene(RpcScene scene)
        {
            Scheduler.Clear();

            currentScene?.Dispose(); // Cleanup.
            currentScene = scene;

            Scheduler.Add(currentScene.Pulse);
            Scheduler.Add(client.Update);
        }

        private RpcScene toRpcScene(SceneInformation info)
        {
            switch (info.Scene)
            {
                case GameScene.MainMenu:
                case GameScene.Search:
                case GameScene.Select:
                case GameScene.Online:
                    Idle:
                    return new IdleScene(client, info.SceneData, GdReader);

                case GameScene.Play:
                    if (info.SceneData.Count > 2)
                        return new PlayScene(client, info.SceneData, GdReader);
                    else
                        goto Idle;

                default:
                    return new UnknownScene(client, info.SceneData, GdReader);
            }
        }
    }
}