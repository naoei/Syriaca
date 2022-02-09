using Binarysharp.MemoryManagement;
using Syriaca.Client.Memory;

namespace Syriaca.Client.Plugins
{
    /// <summary>
    /// A syriaca plugin.
    /// </summary>
    public abstract class Plugin
    {
        /// <summary>
        /// The ID of this plugin.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The current Geometry Dash state.
        /// </summary>
        public GdProcessState State { get; set; }

        /// <summary>
        /// The memory reader for Geometry Dash.
        /// </summary>
        public GdReader GdReader { get; set; }

        /// <summary>
        /// The scheduler for this plugin.
        /// </summary>
        public Scheduler Scheduler { get; set; }

        /// <summary>
        /// The raw memory reader for Geometry Dash.
        /// </summary>
        public MemorySharp MemorySharp => GdReader.Memory;

        /// <summary>
        /// The name of this plugin.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Creates a scheduler for this plugin.
        /// </summary>
        public abstract Scheduler CreateScheduler();

        /// <summary>
        /// Executes after the scheduler is created.
        /// </summary>
        public virtual void OnScheduleCreated()
        {
        }
    }
}