using System.Linq;
using System.Threading;
using Syriaca.Client;
using Syriaca.Client.Memory;
using Syriaca.Client.Plugins;
using Syriaca.Client.Utils;

namespace Syriaca.Runner
{
    public static class Program
    {
        private static GdReader reader;
        private static GdProcessState processState;

        public static void Main(string[] args)
        {
            var proc = ProcessUtils.GetGdProcess(args.Length > 0 && args.Any(a => a == "--opengd" || a == "-o"));

            if (proc == null)
            {
                Logger.Error("Failed to fetch GeometryDash process");

                return;
            }

            reader = new GdReader(proc);

            processState = new GdProcessState(reader);
            processState.StartScheduler();

            var store = new PluginStore();

            foreach (var plugin in store.AvailablePlugins)
            {
                plugin.State = processState;
                plugin.GdReader = reader;
                
                Logger.Log("Successfully loaded: " + plugin.Name);
                
                var scheduler = plugin.CreateScheduler();
                scheduler.Pulse(); // Starts the scheduler.
                CreateLoop(scheduler);
            }

            CreateLoop(processState.Scheduler);
        }

        private static void CreateLoop(Scheduler scheduler)
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (scheduler.Stopwatch.ElapsedMilliseconds < scheduler.Delay)
                        continue;

                    scheduler.Stopwatch.Restart();
                    scheduler.Pulse();
                }
            }).Start();
        }
    }
}