using System;
using System.Collections.Generic;
using Syriaca.Client.Information;
using Syriaca.Client.Utils;

namespace Syriaca.Client.Memory
{
    public class GdProcessState : IHasScheduler
    {
        /// <summary>
        /// Information about the current scene.
        /// </summary>
        public SceneInformation Scene { get; private set; }

        /// <summary>
        /// The current player information.
        /// </summary>
        public PlayerState Player { get; }

        /// <summary>
        /// The scheduler that updates a given amount of time to fetch new data from Geometry Dash.
        /// </summary>
        public Scheduler Scheduler { get; } = new(50);

        private readonly GdReader reader;

        /// <summary>
        /// Called upon when switching scenes.
        /// This also includes information about the previous scene.
        /// </summary>
        public event Action<ValueChangedEvent<SceneInformation>> SceneChanged;

        public GdProcessState(GdReader reader)
        {
            this.reader = reader;
            Scene = new SceneInformation();
            Player = new PlayerState();

            scheduleActions();
        }

        /// <summary>
        /// Gets both <see cref="PlayerState.AccountId"/> and <see cref="PlayerState.UserId"/>.
        /// </summary>
        public PlayerState GetPlayerId()
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
            Logger.Log($"Scene changed from {Scene.Scene} -> {newScene.Scene} with {newScene.SceneData.Count} data attributes");
            Scene = newScene;
        }

        private Dictionary<string, object> getSceneData()
        {
            var dict = new Dictionary<string, object>();

            foreach (var (key, address) in reader.Addresses)
                try
                {
                    switch (address.Type)
                    {
                        case "string":
                            var stringValue = reader.ReadString(address);
                            dict.Add(key, stringValue);
                            Logger.Debug($"{key} - {stringValue}");
                            break;

                        case "int":
                            var intValue = reader.Read<int>(address);
                            dict.Add(key, intValue);
                            Logger.Debug($"{key} - {intValue}");
                            break;

                        case "float":
                            var floatValue = reader.Read<float>(address);
                            dict.Add(key, floatValue);
                            Logger.Debug($"{key} - {floatValue}");
                            break;

                        case "bool":
                            var boolValue = reader.Read<bool>(address);
                            dict.Add(key, boolValue);
                            Logger.Debug($"{key} - {boolValue}");
                            break;

                        default:
                            // We don't know what the type is, so maybe it's not all that important.
                            throw new Exception();
                    }
                }
                catch
                {
                    // don't do anything
                }

            return dict;
        }
    }
}