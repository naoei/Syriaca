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
        public Scheduler Scheduler { get; } = new(50);

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

        private Dictionary<string, object> getSceneData()
        {
            var dict = new Dictionary<string, object>();

            foreach (var address in reader.Addresses)
            {
                try
                {
                    switch (address.Value.Type)
                    {
                        case "string":
                            var stringValue = reader.ReadString(address.Value);
                            dict.Add(address.Key, stringValue);
                            break;
                        
                        case "int":
                            var intValue = reader.Read<int>(address.Value);
                            dict.Add(address.Key, intValue);
                            break;
                        
                        case "float":
                            var floatValue = reader.Read<float>(address.Value);
                            dict.Add(address.Key, floatValue);
                            break;
                        
                        case "bool":
                            var boolValue = reader.Read<bool>(address.Value);
                            dict.Add(address.Key, boolValue);
                            break;
                        
                        default:
                            // We don't know what the type is, so maybe it's not all that important.
                            throw new Exception();
                    }
                }
                catch (Exception e)
                {
                    // don't do anything
                }
            }

            return dict;
        }
    }
}