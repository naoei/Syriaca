using System;
using System.Collections.Generic;
using System.Diagnostics;
using Syriaca.Client.Information;

namespace Syriaca.Client.Memory
{
    public class GdProcessState : IHasScheduler
    {
        public SceneInformation Scene { get; private set; }
        public PlayerState PlayerState { get; }

        private readonly GdReader reader;
        public Scheduler Scheduler { get; } = new(50, typeof(GdProcessState));

        public event Action<ValueChangedEvent<SceneInformation>> SceneChanged;

        public GdProcessState(Process process)
        {
            Scene = new SceneInformation();
            PlayerState = new PlayerState();
            reader = new GdReader(process);
            
            scheduleActions();
        }

        public void StartScheduler()
            => Scheduler.Pulse();

        public (int user, int account) GetPlayerId()
            => reader.ReadPlayerIds();
        
        private void scheduleActions()
        {
            Scheduler.Add(getCurrentScene);
        }

        private void getCurrentScene()
        {
            var currentScene = reader.ReadCurrentScene();

            if (currentScene == Scene.Scene)
                return;

            var newScene = new SceneInformation(currentScene, getSceneData());
            
            SceneChanged?.Invoke(new ValueChangedEvent<SceneInformation>(Scene, newScene));
            Scene = newScene;
        }

        // TODO
        private Dictionary<string, object> getSceneData()
        {
            return new ();
        }
    }
}