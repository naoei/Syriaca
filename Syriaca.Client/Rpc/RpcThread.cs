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
        
        public RpcThread(GdProcessState processState, GdReader reader)
        {
            this.reader = reader;
            client = new RpcClient();
            
            processState.SceneChanged += onSceneChanged;
        }

        // TODO: Use reflection or something to find the correct scene.
        private void onSceneChanged(ValueChangedEvent<SceneInformation> obj)
        {
            switch (obj.NewValue.Scene)
            {
                case GameScene.MainMenu:
                case GameScene.Online:
                    new IdleScene(client, obj.NewValue.SceneData, reader);
                    break;
                
                default:
                    new UnknownScene(client, obj.NewValue.SceneData, reader);
                    break;
            }
            
            client.Update();
        }

        public void StartScheduler()
            => Scheduler.Pulse();
    }
}