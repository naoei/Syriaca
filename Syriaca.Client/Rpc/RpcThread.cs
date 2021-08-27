using System.Collections.Generic;
using DiscordRPC;
using Syriaca.Client.Information;
using Syriaca.Client.Memory;
using Syriaca.Client.Rpc.Scenes;

namespace Syriaca.Client.Rpc
{
    public class RpcThread : IHasScheduler
    {
        public Scheduler Scheduler { get; } = new(2000);
        private readonly RpcClient client;
        private readonly GdReader reader;
        private RpcScene currentScene;

        public RpcThread(GdProcessState processState, GdReader reader)
        {
            this.reader = reader;
            client = new RpcClient();

            currentScene = new UnknownScene(client, new Dictionary<string, object>(), reader);
            Scheduler.Add(client.Update);

            client.ChangeStatus(s =>
            {
                s.Assets = new Assets
                {
                    LargeImageKey = "gd"
                };
            });

            processState.SceneChanged += onSceneChanged;
        }

        // TODO: Use reflection or something to find the correct scene.
        private void onSceneChanged(ValueChangedEvent<SceneInformation> obj)
        {
            Scheduler.Clear();

            switch (obj.NewValue.Scene)
            {
                case GameScene.MainMenu:
                case GameScene.Online:
                    Idle:
                    currentScene = new IdleScene(client, obj.NewValue.SceneData, reader);

                    break;

                case GameScene.Play:
                    if (obj.NewValue.SceneData.Count > 2)
                        currentScene = new PlayScene(client, obj.NewValue.SceneData, reader);
                    else
                        goto Idle;

                    break;

                default:
                    currentScene = new UnknownScene(client, obj.NewValue.SceneData, reader);

                    break;
            }

            Scheduler.Add(currentScene.Pulse);
            Scheduler.Add(client.Update);
        }

        public void StartScheduler()
            => Scheduler.Pulse();
    }
}