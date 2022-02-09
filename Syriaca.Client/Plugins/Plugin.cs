using Binarysharp.MemoryManagement;
using Syriaca.Client.Memory;

namespace Syriaca.Client.Plugins
{
    public abstract class Plugin
    {
        public int Id { get; set; }

        public GdProcessState State { get; set; }

        public GdReader GdReader { get; set; }

        public Scheduler Scheduler { get; set; }

        public MemorySharp MemorySharp => GdReader.Memory;

        public abstract string Name { get; }

        public abstract Scheduler CreateScheduler();

        public virtual void OnScheduleCreated()
        {
        }
    }
}