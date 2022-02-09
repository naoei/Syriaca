using System.Collections.Generic;
using System.Linq;
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
        private static readonly List<Scheduler> schedulers = new();

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
            schedulers.Add(processState.Scheduler);

            var store = new PluginStore();

            foreach (var plugin in store.AvailablePlugins)
            {
                plugin.State = processState;
                plugin.GdReader = reader;

                Logger.Log("Successfully loaded: " + plugin.Name);

                schedulers.Add(plugin.Scheduler = plugin.CreateScheduler());
                plugin.OnScheduleCreated();
            }

            RunSchedulers();
        }

        // ReSharper disable once FunctionNeverReturns
        private static void RunSchedulers()
        {
            foreach (var s in schedulers)
                s.Pulse(); // Start all of the schedulers.

            while (true)
                foreach (var s in schedulers)
                {
                    if (s.Stopwatch.ElapsedMilliseconds < s.Delay)
                        continue;

                    s.Pulse();
                }
        }
    }
}