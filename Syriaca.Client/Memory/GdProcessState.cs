using Syriaca.Client.Information;

namespace Syriaca.Client.Memory
{
    public class GdProcessState
    {
        public GameScene Scene { get; set; }

        public PlayerState PlayerState { get; } = new();
    }
}