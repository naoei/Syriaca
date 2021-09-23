using Binarysharp.MemoryManagement;
using Syriaca.Client.Memory;

namespace Syriaca.Client.Plugins
{
    public abstract class Plugin
    {
        public int Id { get; internal set; }

        public GdProcessState State { get; internal set; }
        
        public GdReader GdReader { get; internal set; }
        
        public MemorySharp MemorySharp => GdReader.Memory;
        
        public abstract string Name { get; }

        public abstract Scheduler CreateScheduler();
    }
}