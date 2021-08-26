using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Syriaca.Client.Information;
using Syriaca.Client.Memory;
using Syriaca.Client.Utils;

namespace Syriaca.Client
{
    public static class Program
    {
        private static Process gdProcess;
        private static GdProcessState processState;
        
        public static void Main(string[] args)
        {
            GetGdProcess(args);

            processState = new GdProcessState(gdProcess);
            
            processState.Scheduler.Pulse();
            processState.SceneChanged += OnSceneChanged;
            
            Logger.Debug("w");
            Logger.Info("w");
            Logger.Log("w");
            Logger.Warn("w");
            Logger.Error("w");
            CreateLoop(processState.Scheduler);
        }

        private static void OnSceneChanged(ValueChangedEvent<SceneInformation> scene)
        {
            Console.WriteLine("Old: " + scene.OldValue.Scene);
            Console.WriteLine("New: " + scene.NewValue.Scene);
        }

        private static void CreateLoop(Scheduler scheduler)
        {
            while (true)
            {
                if (scheduler.Stopwatch.ElapsedMilliseconds < scheduler.Delay)
                    continue;

                scheduler.Stopwatch.Restart();
                scheduler.Pulse();
            }
        }
        
        private static void GetGdProcess(IReadOnlyCollection<string> args)
        {
            if (args.Count > 0 && args.Any(a => a == "--opengd" || a == "-o"))
            {
                // TODO: Allow custom installation paths
                const string path = @"C:\Program Files (x86)\Steam\steamapps\common\Geometry Dash";

                var processStartInfo = new ProcessStartInfo(path + @"\GeometryDash.exe")
                {
                    WorkingDirectory = path,
                    UseShellExecute = false
                };

                gdProcess = Process.Start(processStartInfo);

                // doesn't open immediately so we will have to wait a bit
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            else
            {
                try
                {
                    gdProcess = Process.GetProcessesByName("GeometryDash")[0];
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}