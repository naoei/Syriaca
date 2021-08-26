using System;
using System.Diagnostics;
using Syriaca.Client.Information;

namespace Syriaca.Client.Memory
{
    public class GdProcessState
    {
        public GameScene Scene { get; private set; }
        public PlayerState PlayerState { get; }

        private readonly GdReader reader;

        public GdProcessState(Process process)
        {
            Scene = GameScene.Unknown;
            PlayerState = new PlayerState();

            reader = new GdReader(process);
        }

        public event Action<ValueChangedEvent<SceneInformation>> SceneChanged;

        public (int user, int account) GetPlayerId()
            => reader.ReadPlayerIds();
    }
}